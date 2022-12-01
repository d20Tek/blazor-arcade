//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.Types;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Client
{
    public class Program
    {
        [ExcludeFromCodeCoverage]
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            ConfigureServices(builder.Services, builder.Configuration, builder.HostEnvironment);
            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration config,
            IWebAssemblyHostEnvironment hostEnv)
        {
            services.AddScoped(sp =>
                new HttpClient
                {
                    BaseAddress = new Uri(hostEnv.BaseAddress)
                });

            var arcadeconfig = new ArcadeConfiguration
            {
                ServiceUrl = config["ArcadeServiceUrl"]
            };

            services.AddSingleton<ArcadeConfiguration>(arcadeconfig);
            services.AddScoped<CustomAuthorizationMessageHandler>();
            services.AddHttpClient<IArcadeService, ArcadeService>(client =>
            {
                var serviceUrl = arcadeconfig.ServiceUrl;
                client.BaseAddress = new Uri(serviceUrl);
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            services.AddMsalAuthentication(options =>
            {
                config.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            services.AddClientServices();
        }
    }
}