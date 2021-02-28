﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Crawler>();
                    services.AddSingleton<IHeroesRepository, IcyVeinsRepository>();
                    services.AddHttpClient();
                    services.AddOptions<IcyVeinsRepositoryOptions>().BindConfiguration("IcyVeins").ValidateDataAnnotations();
                    services.AddScoped<HeroesFetchingOperation>();
                });
    }
}
