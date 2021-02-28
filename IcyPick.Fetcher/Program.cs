using IcyPick.Fetcher.Infrastructure;
using IcyPick.Fetcher.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                    services.AddMainService<HeroesFetchingOperation>();
                    services.AddSingleton<IHeroesRepository, IcyVeinsRepository>();
                    services.AddHttpClient();
                    services.AddOptions<IcyVeinsRepositoryOptions>().BindConfiguration("IcyVeins").ValidateDataAnnotations();
                });
    }
}
