//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Client.Services
{
    [ExcludeFromCodeCoverage]
    internal class HubProxy<THub> : IHubProxy<THub>
    {
        private readonly HubConnection _connection;

        public HubProxy(HubConnection connection)
        {
            _connection= connection;
        }

        public HubConnectionState State => _connection.State;

        public Task StartAsync() => _connection.StartAsync();

        public void On<T>(string methodName, Action<T> handler) =>
            _connection.On<T>(methodName, handler);

        public Task InvokeAsync(string methodName, object? arg1, CancellationToken cancellationToken = default) =>
            _connection.InvokeAsync(methodName, arg1, cancellationToken);
    }
}
