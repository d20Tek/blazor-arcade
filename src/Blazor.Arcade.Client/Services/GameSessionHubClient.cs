//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Common.Models.Requests;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blazor.Arcade.Client.Services
{
    internal class GameSessionHubClient : IGameSessionHubClient
    {
        private readonly ILogger<GameSessionHubClient> _logger;
        private IHubProxy<IGameSessionHubClient> _connection;

        public GameSessionHubClient(
            IHubProxy<IGameSessionHubClient> connection,
            ILogger<GameSessionHubClient> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task InitializeAsync(string groupName)
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                using var scope = _logger.BeginScope("GameSessionHub.Initialize method");
                try
                {
                    await _connection.StartAsync();
                    _logger.LogTrace("GameSession: Connection started.");

                    await _connection.InvokeAsync("RegisterWithGroup", groupName);
                    _logger.LogTrace($"GameSession: registered for {groupName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"GameSession: {ex.Message}");
                }
            }
        }

        public async Task CreateSessionAsync(
            UserProfile profile, string metadataId, string sessionName)
        {
            using var scope = _logger.BeginScope("GameSessionHub.CreateSessionAsync method");
            try
            {
                var request = new GameSessionCreateRequest
                {
                    ServerId = profile.Server,
                    HostId = profile.Id,
                    MetadataId = metadataId,
                    Name = sessionName,
                };

                await _connection.InvokeAsync("CreateSessionAsync", request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Chat Server: {ex.Message}");
                throw;
            }
        }

        public void AddSessionChangedHandler(Action<GameSession> handler)
        {
            using var scope = _logger.BeginScope("GameSessionHub.AddSessionChangedHandler method");
            _connection.On<GameSession>("onSessionChanged", handler);
        }

        public void RemoveReceiveMessageHandler(Action<GameSession> handler)
        {
            using var scope = _logger.BeginScope("GameSessionHub.RemoveSessionChangedHandler method");
            _connection.Off<GameSession>("onSessionChanged", handler);
        }
    }
}
