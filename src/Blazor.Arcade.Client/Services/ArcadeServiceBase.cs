﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

namespace Blazor.Arcade.Client.Services
{
    public class ArcadeServiceBase
    {
        private const int _defaultRetryAmount = 2;
        private readonly string _typeName;

        public ArcadeServiceBase(ILogger logger)
        {
            Logger = logger;
            _typeName = this.GetType().Name;
        }

        protected ILogger Logger { get; }

        protected async Task<T> ServiceOperationAsync<T>(
            string methodName,
            Func<Task<T>> operation,
            int retries = _defaultRetryAmount)
        {
            var serviceName = string.Format("/{0}/{1}", _typeName, methodName);

            try
            {
                while (retries > 0)
                {
                    Logger.LogTrace($"Begin Service Call: '{serviceName}'");
                    var result = await operation();
                    Logger.LogTrace($"End Service Call: '{serviceName}'");

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Retrying ServiceOperation: '{serviceName}' with error '{ex.Message}'");
                return await ServiceOperationAsync<T>(methodName, operation, --retries);
            }

            Logger.LogError($"Failed ServiceOperation request '{serviceName}'.");
            throw new InvalidOperationException();
        }
    }
}