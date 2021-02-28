using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    public class HeroesFetchingOperation
    {
        private readonly IHeroesRepository repository;
        private readonly ILogger logger;
        private readonly IHostApplicationLifetime applicationHostLifetime;

        public HeroesFetchingOperation(
            IHeroesRepository repository,
            ILogger<HeroesFetchingOperation> logger,
            IHostApplicationLifetime applicationHostLifetime)
        {
            this.repository = repository;
            this.logger = logger;
            this.applicationHostLifetime = applicationHostLifetime;
        }

        public async Task ExecuteAsync()
        {
            var heroes = await repository.GetHeroesAsync(applicationHostLifetime.ApplicationStopping);

            foreach (var heroe in heroes)
            {
                logger.LogInformation("Heroe found : {Id} as {Category} ({Guide} / {Icon})", heroe.Id, heroe.Category, heroe.GuideUri, heroe.IconUri);
            }

            var guides = (await Task.WhenAll(heroes.Select(TryGetHeroeGuideAsync)))
                .WhereNotNull();

            File.WriteAllText("guides.json", JsonSerializer.Serialize(guides));
        }

        private async Task<HeroeGuide?> TryGetHeroeGuideAsync(Heroe hero)
        {
            try
            {
                return await repository.GetHeroeGuideAsync(hero, applicationHostLifetime.ApplicationStopping);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed loading hero guide of {Id}", hero.Id);
                return null;
            }
        }
    }
}
