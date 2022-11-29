//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Pages;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Pages
{
    [TestClass]
    public class DiagnosticsTests
    {
        [TestMethod]
        public void InitialRender_Unauthenticated()
        {
            // arrange
            var mockClientService = new Mock<IArcadeService>();

            var ctx = new b.TestContext();
            ctx.AddTestAuthorization();
            ctx.Services.AddScoped<IArcadeService>(f => mockClientService.Object);

            // act
            var comp = ctx.RenderComponent<Diagnostics>();

            // assert
            var expectedHtml =
@"  <h3>Diagnostics</h3>
    <hr>
    <h4>Authenticated User:</h4>
    <hr>
    <p>User not logged in.</p>
    <hr>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void InitialRender_Authenticated()
        {
            // arrange
            var diag = new ServiceDiagnostics
            {
                Result = "test-ok",
                EndpointUrl = "/test/url",
                CallerId = "test-user-id",
                CallerName = "Test User",
            };

            var mockClientService = new Mock<IArcadeService>();
            mockClientService.Setup(p => p.GetAuthDiagnosticsAsync())
                             .ReturnsAsync(diag);

            var ctx = new b.TestContext();
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));
            ctx.Services.AddScoped<IArcadeService>(f => mockClientService.Object);

            // act
            var comp = ctx.RenderComponent<Diagnostics>();

            // assert
            var expectedHtml =
@"  <h3>Diagnostics</h3>
    <hr>
    <h4>Authenticated User:</h4>
    <hr>
    <div>Name: Test User</div>
    <div>http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name: Test User</div>
    <div>oid: test-user-id</div>
    <hr>
    <h4>Arcade Service Diagnostics:</h4>
    <hr>
    <div>Result: test-ok</div>
    <div>Endpoint Url: /test/url</div>
    <div>Caller Id: test-user-id</div>
    <div>Caller Name: Test User</div>
    <div diff:ignore></div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_WithClientServiceError()
        {
            // arrange
            var mockClientService = new Mock<IArcadeService>();

            var ctx = new b.TestContext();
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));
            ctx.Services.AddScoped<IArcadeService>(f => mockClientService.Object);

            // act
            var comp = ctx.RenderComponent<Diagnostics>();

            // assert
            var expectedHtml =
@"  <h3>Diagnostics</h3>
    <hr>
    <h4>Authenticated User:</h4>
    <hr>
    <div>Name: Test User</div>
    <div>http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name: Test User</div>
    <div>oid: test-user-id</div>
    <hr>
    <h4>Arcade Service Diagnostics:</h4>
    <hr>
    <p>Service diagnostics request failed.</p>
";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
