//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;
using System.Net.Http.Json;

namespace Blazor.Arcade.Client.Services
{
    public class ArcadeMetadataService : ClientServiceBase, IArcadeMetadataService
    {
        private const string _baseServiceUri = "api/v1/game-metadata";
        private readonly ITypedHttpClient _client;

        public ArcadeMetadataService(ITypedHttpClient client, ILogger<ArcadeMetadataService> logger)
            : base(logger)
        {
            _client= client;
        }

        public async Task<IList<GameMetadata>?> GetGamesMetadataAsync()
        {
            return await ServiceOperationAsync<List<GameMetadata>?>(
                nameof(GetGamesMetadataAsync),
                async () =>
                {
                    var response = await _client.GetAsync(_baseServiceUri);
                    return await response.Content.ReadFromJsonAsync<List<GameMetadata>>();
                });
        }
    }
}
