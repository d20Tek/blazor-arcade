//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.Types;
using Blazor.Arcade.Common.Core.Client;
using Blazored.LocalStorage;
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

        internal static void ConfigureServices(
            IServiceCollection services,
            IConfiguration config,
            IWebAssemblyHostEnvironment hostEnv)
        {
            var arcadeconfig = new ArcadeConfiguration
            {
                ServiceUrl = config["ArcadeServiceUrl"],
                BaseDefaultUrl = hostEnv.BaseAddress
            };

            services.AddSingleton<ArcadeConfiguration>(arcadeconfig);

            services.AddMsalAuthentication(options =>
            {
                config.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            ConfigureHttpClients(services, arcadeconfig);
            services.AddHubProxies(arcadeconfig);
            services.AddClientServices();
            services.AddBlazoredLocalStorageAsSingleton();
        }

        internal static void ConfigureHttpClients(
            IServiceCollection services,
            ArcadeConfiguration config)
        {
            services.AddScoped(sp =>
                new HttpClient
                {
                    BaseAddress = new Uri(config.BaseDefaultUrl)
                });

            services.AddScoped<CustomAuthorizationMessageHandler>();

            var serviceUrl = config.ServiceUrl ?? string.Empty;
            services.AddHttpClient<ITypedHttpClient, TypedHttpClient>(client =>
            {
                client.BaseAddress = new Uri(serviceUrl);
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        }
    }
}