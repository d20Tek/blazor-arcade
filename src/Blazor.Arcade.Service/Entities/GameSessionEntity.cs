//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using System.Text.Json.Serialization;

namespace Blazor.Arcade.Service.Entities
{
    internal class GameSessionEntity : CosmosStoreEntity
    {
        public string SessionId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string ServerId { get; set; } = string.Empty;

        public string MetadataId { get; set; } = string.Empty;

        public string HostId { get; set; } = string.Empty;

        public int Phase { get; set; }

        public Dictionary<string, object> GameState { get; set; } = new Dictionary<string, object>();

        [JsonPropertyName(DocumentId)]
        public override string Id => SessionId;

        [JsonPropertyName(DocumentPartitionId)]
        public override string PartitionId => ServerId;
    }
}
