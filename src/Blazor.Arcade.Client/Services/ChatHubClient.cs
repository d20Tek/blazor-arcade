//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blazor.Arcade.Client.Services
{
    internal class ChatHubClient : IChatHubClient
    {
        private const string _globalChannelId = "channel:global";
        private readonly ILogger<ChatHubClient> _logger;
        private IHubProxy<IChatHubClient> _connection;

        public ChatHubClient(IHubProxy<IChatHubClient> connection, ILogger<ChatHubClient> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                using var scope = _logger.BeginScope("ChatHub.Initialize method");
                try
                {
                    await _connection.StartAsync();
                    _logger.LogTrace("Chat Server: Connection started.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Chat Server: {ex.Message}");
                }
            }
        }

        public void OnReceiveMessageHandler(Action<ChatMessage> handler)
        {
            using var scope = _logger.BeginScope("ChatHub.OnReceiveMessageHandler method");
            _connection.On<ChatMessage>("onReceiveMessage", handler);
        }

        public async Task SendGlobalMessageAsync(ChatMessage message)
        {
            using var scope = _logger.BeginScope("ChatHub.SendGlobalMessage method");
            try
            {
                await _connection.InvokeAsync("SendGlobalMessage", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Chat Server: {ex.Message}");
                throw;
            }
        }
    }
}
