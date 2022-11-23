//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Misc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Arcade.Service.Controllers
{
    [ApiController]
    [Route("api/v1/diag")]
    public class DiagnosticsController : ArcadeControllerBase
    {
        private const string _okResponse = "Ok";

        public DiagnosticsController(ILogger<DiagnosticsController> logger)
            : base(logger)
        {
        }

        [HttpGet(Name = "GetDiagnostics")]
        public ActionResult<ServiceDiagnostics> Get()
        {
            return EndpointOperation<ServiceDiagnostics>("GetDiagnostics", () =>
            {
                return CreateBaseDiagnostics();
            });
        }

        [Authorize]
        [HttpGet("auth", Name = "GetAuthDiagnostics")]
        public ActionResult<ServiceDiagnostics> GetAuth()
        {
            return EndpointOperation<ServiceDiagnostics>("GetAuthDiagnostics", () =>
            {
                string userId = AuthClaims.GetAuthUserId(User);
                string userName = AuthClaims.GetAuthUserName(User);
                Console.WriteLine($"Request-UserId: {userId}; Request-UserName: {userName}");

                var diag = CreateBaseDiagnostics();
                diag.CallerId = userId;
                diag.CallerName = userName;

                return diag;
            });
        }

        private ServiceDiagnostics CreateBaseDiagnostics()
        {
            return new ServiceDiagnostics
            {
                Result = _okResponse,
                EndpointUrl = this.HttpContext.Request.GetDisplayUrl(),
                Timestamp = DateTimeOffset.UtcNow.Ticks,
            };
        }
    }
}
