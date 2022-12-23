//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Models.Requests
{
    public class GameSessionCreateRequest
    {
        public string Name { get; set; } = string.Empty;

        public string ServerId { get; set; } = string.Empty;

        public string MetadataId { get; set; } = string.Empty;

        public string HostId { get; set; } = string.Empty;
    }
}
