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
        private readonly HttpClient _client;

        public ArcadeMetadataService(ITypedHttpClient typedClient, ILogger<ArcadeMetadataService> logger)
            : base(logger)
        {
            _client = typedClient.HttpClient;
        }

        public async Task<IList<GameMetadata>?> GetGamesMetadataAsync()
        {
            return await ServiceOperationAsync<List<GameMetadata>?>(
                nameof(GetGamesMetadataAsync),
                async () =>
                {
                    var response = await _client.GetAsync(_baseServiceUri);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadFromJsonAsync<List<GameMetadata>>();
                });
        }

        public async Task<GameMetadata?> GetGameMetadataByIdAsync(string metadataId)
        {
            return await ServiceOperationAsync<GameMetadata?>(
                nameof(GetGameMetadataByIdAsync),
                async () =>
                {
                    var fullUrl = $"{_baseServiceUri}/{metadataId}";
                    var response = await _client.GetAsync(fullUrl);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadFromJsonAsync<GameMetadata>();
                });
        }
    }
}
