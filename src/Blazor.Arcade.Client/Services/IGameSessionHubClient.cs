//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Client.Services
{
    public interface IGameSessionHubClient
    {
        public Task InitializeAsync(string groupName);

        public Task CreateSessionAsync(UserProfile profile, string metadataId, string sessionName);

        public void AddSessionChangedHandler(Action<GameSession> handler);

        public void RemoveReceiveMessageHandler(Action<GameSession> handler);
    }
}
