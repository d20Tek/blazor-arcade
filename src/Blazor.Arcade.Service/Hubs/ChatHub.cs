//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Microsoft.AspNetCore.SignalR;

namespace Blazor.Arcade.Service.Hubs
{
    public class ChatHub : Hub
    {
        private const string _globalChannelId = "channel:global";

        [HubMethodName(nameof(SendGlobalMessage))]
        public async Task SendGlobalMessage(ChatMessage message)
        {
            if (string.IsNullOrEmpty(message.MessageId))
            {
                message.MessageId = Guid.NewGuid().ToString();
            }

            message.ChannelId = _globalChannelId;
            await Clients.All.SendAsync("onReceiveMessage", message);
        }
    }
}
