//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using System.Security.Principal;

namespace Blazor.Arcade.Service.UnitTests.Mocks
{
    internal static class ControllerContextHelper
    {
        internal const string DefaultUserName = "Test User";
        internal const string DefaultUserId = "e14e5bec-8700-4be5-9e7B-14fae1b2ba82";

        internal static ControllerContext CreateContextAnonymous()
        {
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(CreateTestRequest().Object);
            mockHttpContext.SetupGet(p => p.User).Returns(new ClaimsPrincipal());

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                new RouteData(new RouteValueDictionary()),
                new ControllerActionDescriptor()
                );

            var mockContext = new ControllerContext(actionContext);
            return mockContext;
        }

        internal static ControllerContext CreateContextWithIdentityPrincipal()
        {
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.Request).Returns(CreateTestRequest().Object);
            mockHttpContext.SetupGet(p => p.User).Returns(CreateTestIdentityPrincipal());

            var actionContext = new ActionContext(
                mockHttpContext.Object,
                new RouteData(new RouteValueDictionary()),
                new ControllerActionDescriptor()
                );

            var mockContext = new ControllerContext(actionContext);
            return mockContext;
        }

        internal static Mock<HttpRequest> CreateTestRequest(
            string scheme = "https",
            string host = "test.com",
            string path = "/test/method")
        {
            var mockRequest = new Mock<HttpRequest>();
            mockRequest.SetupGet(p => p.Scheme).Returns(scheme);
            mockRequest.SetupGet(p => p.Host).Returns(new HostString(host));
            mockRequest.SetupGet(p => p.Path).Returns(new PathString(path));

            return mockRequest;
        }

        internal static ClaimsPrincipal CreateTestIdentityPrincipal(
            string userId = DefaultUserId,
            string userName = DefaultUserName)
        {
            var identity = new GenericIdentity(userName, "");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("name", userName));
            identity.AddClaim(new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userId));

            var principal = new GenericPrincipal(identity, roles: new string[] { });
            var claims = new ClaimsPrincipal(principal);

            return claims;
        }
    }
}
