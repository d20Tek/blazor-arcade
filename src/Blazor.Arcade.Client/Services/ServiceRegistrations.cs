//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Types;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blazor.Arcade.Client.Services
{
    internal static class ServiceRegistrations
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IDiagnosticsService, DiagnosticsService>();
            services.AddSingleton<IArcadeMetadataService, ArcadeMetadataService>();
            services.AddSingleton<IMessageBoxService, MessageBoxService>();
            services.AddSingleton<IUserProfileClientService, UserProfileClientService>();
            services.AddSingleton<IChatHubClient, ChatHubClient>();
            return services;
        }

        public static IServiceCollection AddHubProxies(
            this IServiceCollection services,
            ArcadeConfiguration config)
        {
            var chatConnection = new HubConnectionBuilder()
                .WithUrl($"{config.ServiceUrl}/api/v1/chat")
                .WithAutomaticReconnect()
                .Build();
            services.AddSingleton<IHubProxy<IChatHubClient>>(new HubProxy<IChatHubClient>(chatConnection));

            return services;
        }
    }
}
