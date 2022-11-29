//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Arcade.Service.Controllers
{
    public abstract class ArcadeControllerBase : ControllerBase
    {
        private readonly string typeName;

        public ArcadeControllerBase(ILogger logger)
        {
            this.Logger = logger;
            this.typeName = this.GetType().Name;
        }

        protected ILogger Logger { get; }

        protected async Task<ActionResult<T>> EndpointOperationAsync<T>(
            string methodName,
            Func<Task<ActionResult<T>>> operation)
        {
            var endpointName = string.Format("/{0}/{1}", this.typeName, methodName);

            try
            {
                this.Logger.LogTrace($"Begin Operation: '{endpointName}'");
                var result = await operation();
                this.Logger.LogTrace($"End Operation: '{endpointName}'");

                return result;
            }
            catch (ArgumentException ex)
            {
                var errorMessage = $"Endpoint '{endpointName}' received invalid message. '{ex.Message}'";
                this.Logger.LogWarning(errorMessage);

                return this.StatusCode(StatusCodes.Status422UnprocessableEntity, errorMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = "Internal server error";
                var failureMessage = $"Failed Operation '{endpointName}' with error '{ex.Message}'";
                this.Logger.LogError(ex, failureMessage);

                return this.StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        protected ActionResult<T> EndpointOperation<T>(string methodName, Func<ActionResult<T>> operation)
        {
            var endpointName = string.Format("/{0}/{1}", this.typeName, methodName);

            try
            {
                this.Logger.LogTrace($"Begin Operation: '{endpointName}'");
                var result = operation();
                this.Logger.LogTrace($"End Operation: '{endpointName}'");

                return result;
            }
            catch (ArgumentException ex)
            {
                var errorMessage = $"Endpoint '{endpointName}' received invalid message. '{ex.Message}'";
                this.Logger.LogWarning(errorMessage);

                return this.StatusCode(StatusCodes.Status422UnprocessableEntity, errorMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = "Internal server error";
                var failureMessage = $"Failed Operation '{endpointName}' with error '{ex.Message}'";
                this.Logger.LogError(ex, failureMessage);

                return this.StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }
    }
}
