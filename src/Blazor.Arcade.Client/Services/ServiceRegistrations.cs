//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Client.Services
{
    internal static class ServiceRegistrations
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IArcadeMetadataService, ArcadeMetadataService>();
            return services;
        }
    }
}
