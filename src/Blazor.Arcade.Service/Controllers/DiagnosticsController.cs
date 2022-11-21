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
    public class DiagnosticsController : ControllerBase
    {
        private readonly ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(ILogger<DiagnosticsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetDiagnostics")]
        public ServiceDiagnostics Get()
        {
            return CreateBaseDiagnostics();
        }

        [Authorize]
        [HttpGet("auth", Name = "GetAuthDiagnostics")]
        public ServiceDiagnostics GetAuth()
        {
            string userId = AuthClaims.GetAuthUserId(User);
            string userName = AuthClaims.GetAuthUserName(User);
            Console.WriteLine($"Request-UserId: {userId}; Request-UserName: {userName}");

            var diag = CreateBaseDiagnostics();
            diag.CallerId = userId;
            diag.CallerName = userName;

            return diag;
        }

        private ServiceDiagnostics CreateBaseDiagnostics()
        {
            return new ServiceDiagnostics
            {
                Result = "Ok",
                EndpointUrl = this.HttpContext.Request.GetDisplayUrl(),
                Timestamp = DateTimeOffset.UtcNow.Ticks,
            };
        }
    }
}
