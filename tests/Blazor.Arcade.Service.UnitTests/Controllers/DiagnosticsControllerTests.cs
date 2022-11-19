//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Service.Controllers;
using Blazor.Arcade.Service.UnitTests.Mocks;
using Microsoft.Extensions.Logging;

namespace Blazor.Arcade.Service.UnitTests.Controllers
{
    [TestClass]
    public class DiagnosticsControllerTests
    {
        private readonly ILogger<DiagnosticsController> _logger = new Mock<ILogger<DiagnosticsController>>().Object;

        [TestMethod]
        public void GetDiagnostics()
        {
            // arrange
            var c = new DiagnosticsController(_logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextAnonymous(),
            };

            // act
            var results = c.Get();

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual("Ok", results.Result);
            Assert.AreEqual("https://test.com/test/method", results.EndpointUrl);
            Assert.IsTrue(results.Timestamp > 0);
            Assert.IsNull(results.CallerId);
            Assert.IsNull(results.CallerName);
        }

        [TestMethod]
        public void GetAuthDiagnostics()
        {
            // arrange
            var c = new DiagnosticsController(_logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var results = c.GetAuth();

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual("Ok", results.Result);
            Assert.AreEqual("https://test.com/test/method", results.EndpointUrl);
            Assert.IsTrue(results.Timestamp > 0);
            Assert.AreEqual(ControllerContextHelper.DefaultUserId, results.CallerId);
            Assert.AreEqual(ControllerContextHelper.DefaultUserName, results.CallerName);
        }

        [TestMethod]
        public void GetAuthDiagnostics_AsAnonymous()
        {
            // arrange
            var c = new DiagnosticsController(_logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextAnonymous(),
            };

            // act
            var results = c.GetAuth();

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual("Ok", results.Result);
            Assert.AreEqual("https://test.com/test/method", results.EndpointUrl);
            Assert.IsTrue(results.Timestamp > 0);
            Assert.AreEqual(string.Empty, results.CallerId);
            Assert.AreEqual(string.Empty, results.CallerName);
        }
    }
}