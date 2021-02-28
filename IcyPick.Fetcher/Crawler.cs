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
        private readonly ILogger logger;

        public Crawler(IHostApplicationLifetime applicationHostLifetime, ILogger<Crawler> logger)
        {
            this.applicationHostLifetime = applicationHostLifetime;
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var task = DoExecuteAsync(stoppingToken);
            task.ContinueWith(_ => applicationHostLifetime.StopApplication(), stoppingToken);
            return task;
        }

        private async Task DoExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Started !");
            await Task.Delay(3000, stoppingToken);
            logger.LogInformation("Stopped !");
        }
    }
}
