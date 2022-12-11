//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using System.Text.Json.Serialization;

namespace Blazor.Arcade.Service.Entities
{
    internal class UserAccountEntity : CosmosStoreEntity
    {
        public string AccountId { get; set; } = string.Empty;

        public string ServerId { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string AvatarUri { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public int Exp { get; set; } = 0;

        public string UserId { get; set; } = string.Empty;

        [JsonPropertyName(DocumentId)]
        public override string Id => AccountId;

        [JsonPropertyName(DocumentPartitionId)]
        public override string PartitionId => this.UserId;
    }
}
