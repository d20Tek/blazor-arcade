//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Common.Models.Requests;

namespace Blazor.Arcade.Service.Logic
{
    public interface IGameSessionActionManager
    {
        public Task<IList<GameSession>> GetSessionsForServerAsync(string serverId);

        public Task<GameSession> GetSessionByIdAsync(string sessionId);

        public Task<GameSession> CreateSessionAsync(GameSessionCreateRequest request);
    }
}
