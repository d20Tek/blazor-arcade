//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure.Cosmos;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Service.Repositories
{
    internal static class RepositoryRegistrations
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddSingleton<IReadRepository<GameMetadata>, GameMetadataRepository>();
            services.AddSingleton<IReadRepository<ServerMetadata>, ServerMetadataRepository>();
            services.AddSingleton<ICacheService, MemoryCacheService>(); 
            services.AddSingleton<IRepository<UserAccount>, UserAccountRepository>();

            return services;
        }

        public static IServiceCollection AddCosmosClient(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString) == false)
            {
                services.AddSingleton<CosmosClient>(sp =>
                {
                    return new CosmosClient(connectionString);
                });
            }
            return services;
        }
    }
}
