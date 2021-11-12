using HGUtils.Helpers.Configuration;
using Microsoft.Extensions.Configuration;
using SuperHeroSearch_Common.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration config) =>
            services
                .AddConfig<ApiConfig>(config);

        public static IServiceCollection AddIoC(this IServiceCollection services) =>
            services;

    }
}
