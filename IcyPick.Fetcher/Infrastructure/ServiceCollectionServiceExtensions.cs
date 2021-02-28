using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace IcyPick.Fetcher.Infrastructure
{
    public static class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddMainService
            <[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>
            (this IServiceCollection services)
            where TImplementation : class, IMainService
        {
            services.AddHostedService<MainServiceHoster>();

            return services.AddScoped<IMainService, TImplementation>();
        }
    }
}
