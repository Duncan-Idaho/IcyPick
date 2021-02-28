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

            SaveGuides(guides);
            await DownloadHeroImagesAsync(guides);
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

        void SaveGuides(IEnumerable<HeroGuide> guides)
        {
            File.WriteAllText(Path.Combine(options.CurrentValue.DataOut, "guides.json"), JsonSerializer.Serialize(guides));
        }

        async Task DownloadHeroImagesAsync(IReadOnlyList<Hero> heroes)
        {
            var heroImagesFolder = Path.Combine(options.CurrentValue.ImagesOutDir, "heroes");
            Directory.CreateDirectory(heroImagesFolder);

            await Task.WhenAll(heroes.Select(TryDownloadHeroImageAsync));

            async Task TryDownloadHeroImageAsync(Hero hero)
            {
                try
                {
                    await repository.GetHeroImageAsync(
                        hero, 
                        () => CreateHeroImageFile(hero), 
                        applicationHostLifetime.ApplicationStopping);
                    logger.LogInformation("Hero guide for found for : {Id}", hero.Id);
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Failed loading hero image of {Id}", hero.Id);
                }
            }

            Task<Stream> CreateHeroImageFile(Hero hero)
            {
                Stream ouputStream = new FileStream(
                    Path.Combine(heroImagesFolder, hero.Id + ".jpg"),
                    FileMode.Create,
                    FileAccess.Write);

                return Task.FromResult(ouputStream);
            }
        }
    }
}
