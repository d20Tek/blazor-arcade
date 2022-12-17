//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure.Cosmos;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Converters;
using Blazor.Arcade.Service.Entities;

namespace Blazor.Arcade.Service.Repositories
{
    internal class UserAccountRepository : CosmosRepository<UserProfile, UserAccountEntity>
    {
        private const string _cosmosDb = "blazor-arcade-cdb";
        private const string _userAccountsContainer = "user-accounts";
        private const string _partitionDef = "/_partitionid";
        private static readonly UserAccountToEntityConverter conv = new UserAccountToEntityConverter();

        public UserAccountRepository(
            CosmosClient cosmosClient,
            ICacheService? cache,
            ILogger<UserAccountRepository> logger)
            : base(cosmosClient, _cosmosDb, _userAccountsContainer, _partitionDef, conv, cache, logger)
        {
        }
    }
}
