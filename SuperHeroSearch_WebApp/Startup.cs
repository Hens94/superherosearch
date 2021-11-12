using HGUtils.Helpers.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SuperHeroSearch_WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddConfigs(Configuration)
                .AddIoC()
                .AddHttpClients(Configuration)
                .AddControllersWithViews();

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) =>
            app
                .UseIf(
                    env.IsDevelopment(),
                    appb => appb.UseDeveloperExceptionPage()
                                .UseBrowserLink(),
                    appb => appb.UseHsts())
                .UseHttpsRedirection()
                .UseStaticFiles()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });

    }
}
