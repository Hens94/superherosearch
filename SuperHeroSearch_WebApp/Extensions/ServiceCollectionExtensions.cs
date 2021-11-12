using Flurl;
using HGUtils.Helpers.Configuration;
using Microsoft.Extensions.Configuration;
using SuperHeroSearch_App.Contracts;
using SuperHeroSearch_App.Contracts.HttpClients;
using SuperHeroSearch_App.Services;
using SuperHeroSearch_App.Services.HttpClients;
using SuperHeroSearch_Common.Configurations;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration config) =>
            services
                .AddConfig<ApiConfig>(config);

        public static IServiceCollection AddIoC(this IServiceCollection services) =>
            services
                .AddTransient<ISuperHero, SuperHeroService>();

        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
        {
            var apiConfig = config.GetConfig<ApiConfig>() ?? throw new ArgumentNullException(nameof(ApiConfig));
            var baseAddress = Url.Combine(apiConfig.BaseUrl, apiConfig.AccessToken, "/");

            services
                .AddHttpClient<ISuperHeroHttpClient, SuperHeroHttpClientService>(client => client.BaseAddress = new Uri(baseAddress));

            return services;
        }
    }
}
