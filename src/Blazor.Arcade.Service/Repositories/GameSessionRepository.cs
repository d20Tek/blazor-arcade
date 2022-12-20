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
    internal class GameSessionRepository : CosmosRepository<GameSession, GameSessionEntity>
    {
        private const string _cosmosDb = "blazor-arcade-cdb";
        private const string _containerName = "game-sessions";
        private const string _partitionDef = "/_partitionid";
        private static readonly GameSessionToEntityConverter conv = new GameSessionToEntityConverter();

        public GameSessionRepository(
            CosmosClient cosmosClient,
            ICacheService? cache,
            ILogger<GameSessionRepository> logger)
            : base(cosmosClient, _cosmosDb, _containerName, _partitionDef, conv, cache, logger)
        {
        }
    }
}
