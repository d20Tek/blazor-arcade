//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using System.Text.Json;

namespace Blazor.Arcade.Service.Repositories
{
    public class ServerMetadataRepository : IReadRepository<ServerMetadata>
    {
        private const string _gameMetadataFile = "./Data/game-servers.json";
        private static readonly JsonSerializerOptions _options = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
        };
        private IList<ServerMetadata> _serverMetadata = new List<ServerMetadata>();

        public async Task<IList<ServerMetadata>> GetAll()
        {
            await EnsureMetadataLoaded();
            return _serverMetadata;
        }

        public async Task<ServerMetadata?> GetById(string name)
        {
            await EnsureMetadataLoaded();
            return _serverMetadata.FirstOrDefault(m => m.Name == name);
        }

        private async Task EnsureMetadataLoaded()
        {
            if (_serverMetadata.Any() == false)
            {
                var stream = File.OpenRead(_gameMetadataFile);

                var list =
                    await JsonSerializer.DeserializeAsync<IList<ServerMetadata>>(stream, _options) ??
                    throw new FormatException();

                _serverMetadata = list.OrderBy(m => m.Name).ToList();
            }
        }
    }
}
