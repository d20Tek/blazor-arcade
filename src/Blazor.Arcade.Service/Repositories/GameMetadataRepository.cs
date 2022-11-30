//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using System.Text.Json;

namespace Blazor.Arcade.Service.Repositories
{
    internal class GameMetadataRepository : IGameMetadataRepository
    {
        private const string _gameMetadataFile = "./Data/game-metadata.json";
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
        };
        private IList<GameMetadata> _gameMetadata = new List<GameMetadata>();

        public async Task<IList<GameMetadata>> GetAll()
        {
            await EnsureMetadataLoaded();
            return _gameMetadata;
        }

        public async Task<GameMetadata?> GetById(string id)
        {
            await EnsureMetadataLoaded();
            return _gameMetadata.FirstOrDefault(m => m.Id == id);
        }

        private async Task EnsureMetadataLoaded()
        {
            if (_gameMetadata.Any() == false)
            {
                var stream = File.OpenRead(_gameMetadataFile);

                var list =
                    await JsonSerializer.DeserializeAsync<IList<GameMetadata>>(stream, _options) ??
                    throw new FormatException();

                _gameMetadata = list.OrderBy(m => m.SortOrder).ThenBy(m => m.Name).ToList();
            }
        }
    }
}
