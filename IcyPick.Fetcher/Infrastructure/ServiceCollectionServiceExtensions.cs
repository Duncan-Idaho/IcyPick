using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
