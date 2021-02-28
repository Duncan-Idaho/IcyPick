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
                await DoExecuteAsync();
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

        private async Task DoExecuteAsync()
        {
            using var scope = services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<HeroesFetchingOperation>().ExecuteAsync();
        }
    }
}
