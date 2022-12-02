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

        // todo: remove this api when client updates dependency.
        public async Task SendMessage(string user, string message)
            => await Clients.All.SendAsync("ReceiveMessage", user, message);

        public async Task SendGlobalMessage(ChatMessage message)
        {
            message.ChannelId = _globalChannelId;
            await Clients.All.SendAsync("onReceiveMessage", message);
        }
    }
}
