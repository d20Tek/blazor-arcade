//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Common.Models.Requests;

namespace Blazor.Arcade.Service.Logic
{
    internal class GameSessionActionManager : IGameSessionActionManager
    {
        private readonly IRepository<GameSession> _repo;

        public GameSessionActionManager(
            IRepository<GameSession> repository)
        {
            _repo = repository;
        }

        public async Task<IList<GameSession>> GetSessionsForServerAsync(string serverId)
        {
            var sessions = await _repo.GetPartitionItemsAsync(serverId);
            return sessions;
        }

        public async Task<GameSession> GetSessionByIdAsync(string sessionId)
        {
            sessionId.ThrowIfEmpty(nameof(sessionId));
            var session = await _repo.GetItemAsync(sessionId);
            return session;
        }

        public async Task<GameSession> CreateSessionAsync(GameSessionCreateRequest request)
        {
            var session = new GameSession
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                ServerId = request.ServerId,
                MetadataId = request.MetadataId,
                HostId = request.HostId,
                Phase = (int)GameSessionPhase.Created,
            };

            session.ValidateModel();

            var createSession = await _repo.CreateItemAsync(session);
            return createSession;
        }
    }
}
