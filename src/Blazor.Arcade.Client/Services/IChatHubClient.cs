﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Client.Services
{
    public interface IChatHubClient
    {
        public Task InitializeAsync();

        public Task SendGlobalMessageAsync(ChatMessage message);

        public void AddReceiveMessageHandler(Action<ChatMessage> handler);

        public void RemoveReceiveMessageHandler(Action<ChatMessage> handler);
    }
}
