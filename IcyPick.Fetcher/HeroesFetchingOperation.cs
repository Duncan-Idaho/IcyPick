using IcyPick.Fetcher.Infrastructure;
using IcyPick.Fetcher.Models;
using IcyPick.Fetcher.Repositories;
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
    public class HeroesFetchingOperation : IMainService
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

            foreach (var hero in heroes)
            {
                logger.LogInformation("Hero found : {Id} ({Guide})", hero.Id, hero.GuideUri);
            }

            var guides = (await Task.WhenAll(heroes.Select(TryGetHeroGuideAsync)))
                .WhereNotNull();

            File.WriteAllText("guides.json", JsonSerializer.Serialize(guides));
        }

        private async Task<HeroGuide?> TryGetHeroGuideAsync(Hero hero)
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
}
