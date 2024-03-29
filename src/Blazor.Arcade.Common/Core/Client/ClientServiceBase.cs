﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Common.Core.Client
{
    public class ClientServiceBase
    {
        private const int _defaultRetryAmount = 3;
        private readonly string _typeName;

        public ClientServiceBase(ILogger logger)
        {
            Logger = logger;
            _typeName = GetType().Name;
        }

        protected ILogger Logger { get; }

        [ExcludeFromCodeCoverage]
        protected async Task<T> ServiceOperationAsync<T>(
            string methodName,
            Func<Task<T>> operation,
            int retries = _defaultRetryAmount)
        {
            var serviceName = string.Format("/{0}/{1}", _typeName, methodName);

            try
            {
                Logger.LogTrace($"Begin Service Call: '{serviceName}'");
                var result = await operation();
                Logger.LogTrace($"End Service Call: '{serviceName}'");

                return result;
            }
            catch (Exception ex)
            {
                if (retries > 0)
                {
                    Logger.LogWarning($"Retrying ServiceOperation: '{serviceName}' with error '{ex.Message}'");
                    return await ServiceOperationAsync<T>(methodName, operation, --retries);
                }

                Logger.LogError($"Failed ServiceOperation request '{serviceName}'.");
                throw;
            }
        }
    }
}
