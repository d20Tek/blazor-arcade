//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Models
{
    public class ChatMessage
    {
        public string MessageId { get; set; } = string.Empty;

        public string ChannelId { get; set; } = string.Empty;

        public string AccountId { get; set; } = string.Empty;

        public string AccountName { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
