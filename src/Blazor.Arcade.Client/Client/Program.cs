//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Blazor.Arcade.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var serviceUrl = builder.Configuration["ArcadeServiceUrl"] ?? string.Empty;

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
            builder.Services.AddHttpClient<ArcadeClientService>(client =>
            {
                client.BaseAddress = new Uri(serviceUrl);
            }).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
            });

            await builder.Build().RunAsync();
        }
    }
}