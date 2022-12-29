//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.SignalR.Client;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Client.Services
{
    [ExcludeFromCodeCoverage]
    internal class HubProxy<THub> : IHubProxy<THub>, IDisposable
    {
        private bool _disposedValue;
        private readonly HubConnection _connection;
        private readonly Dictionary<Tuple<string, object>, IDisposable> _handlerDictionary =
            new Dictionary<Tuple<string, object>, IDisposable>();

        public HubProxy(HubConnection connection)
        {
            _connection= connection;
        }

        public HubConnectionState State => _connection.State;

        public Task StartAsync() => _connection.StartAsync();

        public void On<T>(string methodName, Action<T> handler)
        {
            var disposer = _connection.On<T>(methodName, handler);
            var key = new Tuple<string, object>(methodName, handler);
            _handlerDictionary.Add(key, disposer);
        }

        public void Off<T>(string methodName, Action<T> handler)
        {
            var key = new Tuple<string, object>(methodName, handler);
            if (_handlerDictionary.Remove(key, out var disposer) == true)
            {
                disposer.Dispose();
            }
        }

        public async Task InvokeAsync(
            string methodName, object? arg1, CancellationToken cancellationToken = default) =>
            await _connection.InvokeAsync(methodName, arg1, cancellationToken);

        public async Task<TResult> InvokeAsync<TResult>(
            string methodName, object? arg1, CancellationToken cancellationToken = default) =>
            await _connection.InvokeAsync<TResult>(methodName, arg1, cancellationToken);


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var disposable in _handlerDictionary.Values)
                    {
                        disposable.Dispose();
                    }
                    _handlerDictionary.Clear();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
