//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Arcade.Service.Controllers
{
    public abstract class ArcadeControllerBase : ControllerBase
    {
        private readonly string typeName;

        public ArcadeControllerBase(ILogger logger)
        {
            Logger = logger;
            typeName = GetType().Name;
        }

        protected ILogger Logger { get; }

        protected async Task<ActionResult<T>> EndpointOperationAsync<T>(
            string methodName,
            Func<Task<ActionResult<T>>> operation)
        {
            var endpointName = string.Format("/{0}/{1}", typeName, methodName);

            try
            {
                Logger.LogTrace($"Begin Operation: '{endpointName}'");
                var result = await operation();
                Logger.LogTrace($"End Operation: '{endpointName}'");

                return result;
            }
            catch (ArgumentException ex)
            {
                var errorMessage = $"Endpoint '{endpointName}' received invalid message. '{ex.Message}'";
                Logger.LogWarning(errorMessage);

                return StatusCode(StatusCodes.Status422UnprocessableEntity, errorMessage);
            }
            catch (EntityAlreadyExistsException)
            {
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (EntityNotFoundException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                var errorMessage = "Internal server error";
                var failureMessage = $"Failed Operation '{endpointName}' with error '{ex.Message}'";
                Logger.LogError(ex, failureMessage);

                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }

        protected ActionResult<T> EndpointOperation<T>(string methodName, Func<ActionResult<T>> operation)
        {
            var endpointName = string.Format("/{0}/{1}", typeName, methodName);

            try
            {
                Logger.LogTrace($"Begin Operation: '{endpointName}'");
                var result = operation();
                Logger.LogTrace($"End Operation: '{endpointName}'");

                return result;
            }
            catch (ArgumentException ex)
            {
                var errorMessage = $"Endpoint '{endpointName}' received invalid message. '{ex.Message}'";
                Logger.LogWarning(errorMessage);

                return StatusCode(StatusCodes.Status422UnprocessableEntity, errorMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = "Internal server error";
                var failureMessage = $"Failed Operation '{endpointName}' with error '{ex.Message}'";
                Logger.LogError(ex, failureMessage);

                return StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
            }
        }
    }
}
