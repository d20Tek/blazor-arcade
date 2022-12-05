//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Client.Services
{
    public interface IChatHubClient
    {
        public Task InitializeAsync();

        public Task SendGlobalMessageAsync(ChatMessage message);

        public void OnReceiveMessageHandler(Action<ChatMessage> handler);
    }
}
