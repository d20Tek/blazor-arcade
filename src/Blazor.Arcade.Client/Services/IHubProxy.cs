//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blazor.Arcade.Client.Services
{
    public interface IHubProxy<THub>
    {
        public HubConnectionState State { get; }

        public Task StartAsync();

        public void On<T>(string methodName, Action<T> handler);

        public void Off<T>(string methodName, Action<T> handler);

        public Task InvokeAsync(string methodName, object? arg1, CancellationToken cancellationToken = default);

        public Task<TResult> InvokeAsync<TResult>(string methodName, object? arg1, CancellationToken cancellationToken = default);
    }
}
