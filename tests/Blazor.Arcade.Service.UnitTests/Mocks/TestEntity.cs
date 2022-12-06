//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using System.Text.Json.Serialization;

namespace Blazor.Arcade.Service.UnitTests.Mocks
{
    public class TestEntity : CosmosStoreEntity
    {
        public string TestId { get; set; } = string.Empty;


        [JsonPropertyName(DocumentId)]
        public override string Id => TestId;

        [JsonPropertyName(DocumentPartitionId)]
        public override string PartitionId => TestId;
    }
}
