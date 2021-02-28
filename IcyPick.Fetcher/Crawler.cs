using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    public class Crawler : BackgroundService
    {
        private readonly IHostApplicationLifetime applicationHostLifetime;
        private readonly IServiceProvider services;
        private readonly ILogger logger;

        public Crawler(
            IHostApplicationLifetime applicationHostLifetime,
            IServiceProvider services,
            ILogger<Crawler> logger)
        {
            this.applicationHostLifetime = applicationHostLifetime;
            this.services = services;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await DoExecuteAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Service failed");
            }
            finally
            {
                applicationHostLifetime.StopApplication();
            }
        }

        private async Task DoExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = services.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IHeroesRepository>();

            var heroes = await repository.GetHeroesAsync(stoppingToken);

            foreach(var heroe in heroes)
            {
                logger.LogInformation("Heroe found : {Name} as {Category} ({Guide} / {Icon})", heroe.Name, heroe.Category, heroe.GuideUri, heroe.IconUri);
            }
        }
    }
}
