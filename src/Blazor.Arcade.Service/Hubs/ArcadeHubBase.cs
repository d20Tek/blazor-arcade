//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Service.Hubs
{
    public abstract class ArcadeHubBase : Hub
    {
        private readonly string _typeName;

        public ArcadeHubBase(ILogger logger)
        {
            Logger = logger;
            _typeName = this.GetType().Name;
        }

        protected ILogger Logger { get; }

        protected async Task AddToGroupAsync(string groupName)
        {
            groupName.ThrowIfEmpty(nameof(groupName));
            await this.Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        protected async Task SendGroupMessageAsync<T>(
            string groupName, string messageName, T messageBody)
        {
            groupName.ThrowIfEmpty(nameof(groupName));
            messageName.ThrowIfEmpty(nameof(messageName));

            await Clients.Group(groupName).SendAsync(messageName, messageBody);
        }

        [ExcludeFromCodeCoverage]
        protected async Task HubOperationAsync(string hubMethod, Func<Task> operation)
        {
            var operationName = string.Format("/{0}.{1}", _typeName, hubMethod);
            try
            {
                Logger.LogTrace($"Begin HubOperation '{operationName}'");
                await operation();
                Logger.LogTrace($"End HubOperation '{operationName}'");
            }
            catch (ArgumentException ex)
            {
                var msg = string.Format(
                    $"Hub '{operationName}' received invalid input. '{ex.Message}'");
                this.Logger.LogWarning(msg);

                await Clients.Caller.SendAsync("onOperationError", msg);
                throw;
            }
            catch (Exception ex)
            {
                var msg = string.Format(
                    $"Hub '{operationName}' operation failed with unexpected error: '{ex.Message}'");
                this.Logger.LogError(ex, msg);

                await Clients.Caller.SendAsync("onOperationError", msg);
                throw;
            }
        }

        [ExcludeFromCodeCoverage]
        protected async Task<T> HubOperationAsync<T>(string hubMethod, Func<Task<T>> operation)
        {
            var operationName = string.Format("/{0}.{1}", _typeName, hubMethod);
            try
            {
                Logger.LogTrace($"Begin HubOperation '{operationName}'");
                var result = await operation();
                Logger.LogTrace($"End HubOperation '{operationName}'");

                return result;
            }
            catch (ArgumentException ex)
            {
                var msg = string.Format(
                    $"Hub '{operationName}' received invalid input. '{ex.Message}'");
                this.Logger.LogWarning(msg);

                await Clients.Caller.SendAsync("onOperationError", msg);
                throw;
            }
            catch (Exception ex)
            {
                var msg = string.Format(
                    $"Hub '{operationName}' operation failed with unexpected error: '{ex.Message}'");
                this.Logger.LogError(ex, msg);

                await Clients.Caller.SendAsync("onOperationError", msg);
                throw;
            }
        }
    }
}
