//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Models
{
    public class ChatMessage
    {
        public string MessageId { get; set; } = string.Empty;

        public string ChannelId { get; set; } = string.Empty;

        public string ProfileId { get; set; } = string.Empty;

        public string ProfileName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
