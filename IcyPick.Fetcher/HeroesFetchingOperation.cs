using IcyPick.Fetcher.Infrastructure;
using IcyPick.Fetcher.Models;
using IcyPick.Fetcher.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    public class HeroesFetchingOperation : IMainService
    {
        readonly IOptionsMonitor<HeroesFetchingOperationOptions> options;
        readonly IHeroesRepository repository;
        readonly ILogger logger;
        readonly IHostApplicationLifetime applicationHostLifetime;

        public HeroesFetchingOperation(
            IOptionsMonitor<HeroesFetchingOperationOptions> options,
            IHeroesRepository repository,
            ILogger<HeroesFetchingOperation> logger,
            IHostApplicationLifetime applicationHostLifetime)
        {
            this.options = options;
            this.repository = repository;
            this.logger = logger;
            this.applicationHostLifetime = applicationHostLifetime;
        }

        public async Task ExecuteAsync()
        {
            var heroes = await GetHeroes();
            var guides = await GetGuides(heroes);
            var masterTiers = await repository.GetHeroTiersAsync("master", applicationHostLifetime.ApplicationStopping);
            var generalTiers = await repository.GetHeroTiersAsync("general", applicationHostLifetime.ApplicationStopping);
            var aramTiers = await repository.GetHeroTiersAsync("aram", applicationHostLifetime.ApplicationStopping);
            var maps = guides.SelectMany(guide => guide.MapPreference.AllMaps).Distinct();

            SaveGuides(new Output.Data(
                guides.Select(CreateOutpuHero).ToList(),
                maps.Select(Output.Map.FromModel).ToList()));
            await DownloadImagesAsync(guides, "heroes");
            await DownloadImagesAsync(maps, "maps");

            Output.Hero CreateOutpuHero(HeroGuide guide)
                => new Output.Hero(
                    guide.Id,
                    guide.Name,
                    guide.Category,
                    guide.GuideUri,
                    Output.HeroMapPreference.FromModel(guide.MapPreference),
                    Output.HeroSynergiesAndCounter.FromModel(guide.SynergiesAndCounter),
                    ExtractTierRecommendation(guide.Id, generalTiers),
                    ExtractTierRecommendation(guide.Id, masterTiers),
                    ExtractTierRecommendation(guide.Id, aramTiers));

            Dictionary<string, Output.TierRecommendation> ExtractTierRecommendation(string heroId, IReadOnlyList<Tier> tiers)
            {
                var recommendations = from tier in tiers
                                      from hero in tier.Heroes
                                      where hero.Id == heroId
                                      select (tier, hero);

                return recommendations.ToDictionary(
                    recommendation => recommendation.hero.Condition ?? "default",
                    recommendation => new Output.TierRecommendation(
                        recommendation.tier.Name,
                        recommendation.hero.BanRecommended));
            }
        }

        async Task<IReadOnlyList<Hero>> GetHeroes()
        {
            var heroes = await repository.GetHeroesAsync(applicationHostLifetime.ApplicationStopping);

            foreach (var hero in heroes)
            {
                logger.LogInformation("Hero found : {Id} ({Guide})", hero.Id, hero.GuideUri);
            }

            return heroes;
        }

        async Task<IReadOnlyList<HeroGuide>> GetGuides(IReadOnlyList<Hero> heroes)
        {
            return (await Task.WhenAll(heroes.Select(TryGetHeroGuideAsync)))
                .WhereNotNull()
                .ToList();

            async Task<HeroGuide?> TryGetHeroGuideAsync(Hero hero)
            {
                try
                {
                    var result = await repository.GetHeroGuideAsync(hero, applicationHostLifetime.ApplicationStopping);
                    logger.LogInformation("Hero guide for found for : {Id}", hero.Id);

                    return result;
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Failed loading hero guide of {Id}", hero.Id);
                    return null;
                }
            }
        }

        void SaveGuides(Output.Data data)
        {
            File.WriteAllText(options.CurrentValue.DataOut, JsonSerializer.Serialize(data, new JsonSerializerOptions()
            {
                WriteIndented = true
            }));
        }

        async Task DownloadImagesAsync(IEnumerable<IEntityWithImage> entities, string folder)
        {
            var imagesFolder = Path.Combine(options.CurrentValue.ImagesOutDir, folder);
            Directory.CreateDirectory(imagesFolder);

            await Task.WhenAll(entities.Select(TryDownloadImagesAsync));

            async Task TryDownloadImagesAsync(IEntityWithImage entity)
            {
                try
                {
                    await repository.DownloadImageAsync(
                        entity, 
                        () => CreateImageFile(entity), 
                        applicationHostLifetime.ApplicationStopping);
                    logger.LogInformation("{Type} image found for : {Id}", entity.GetType().Name, entity.Id);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Failed loading {Type} image of {Id}", entity.GetType().Name, entity.Id);
                }
            }

            Task<Stream> CreateImageFile(IEntityWithImage entity)
            {
                Stream ouputStream = new FileStream(
                    Path.Combine(imagesFolder, entity.Id + ".jpg"),
                    FileMode.Create,
                    FileAccess.Write);

                return Task.FromResult(ouputStream);
            }
        }
    }
}
