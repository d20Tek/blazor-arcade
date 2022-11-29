//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Service.Repositories
{
    internal static class RepositoryRegistrations
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddSingleton<IGameMetadataRepository, GameMetadataRepository>();
            return services;
        }
    }
}
