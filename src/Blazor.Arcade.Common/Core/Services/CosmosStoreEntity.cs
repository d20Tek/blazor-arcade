//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Text.Json.Serialization;

namespace Blazor.Arcade.Common.Core.Services
{
    public abstract class CosmosStoreEntity
    {
        protected const string DocumentId = "id";
        protected const string DocumentPartitionId = "_partitionid";

        [JsonPropertyName("_attachments")]
        public string Attachments { get; set; } = string.Empty;

        [JsonPropertyName(DocumentId)]
        public abstract string Id { get; }

        [JsonPropertyName("_rid")]
        public string RowId { get; set; } = string.Empty;

        [JsonPropertyName("_etag")]
        public string ETag { get; set; } = string.Empty;

        [JsonPropertyName(DocumentPartitionId)]
        public abstract string PartitionId { get; }

        [JsonPropertyName("_self")]
        public string Self { get; set; } = string.Empty;

        [JsonPropertyName("_ts")]
        public long Timestamp { get; set; }
    }
}
