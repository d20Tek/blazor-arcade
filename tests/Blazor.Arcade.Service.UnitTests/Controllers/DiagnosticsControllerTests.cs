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
            Assert.IsNotNull(results.Value);
            Assert.AreEqual("Ok", results.Value.Result);
            Assert.AreEqual("https://test.com/test/method", results.Value.EndpointUrl);
            Assert.IsTrue(results.Value.Timestamp > 0);
            Assert.IsNull(results.Value.CallerId);
            Assert.IsNull(results.Value.CallerName);
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
            Assert.IsNotNull(results.Value);
            Assert.AreEqual("Ok", results.Value.Result);
            Assert.AreEqual("https://test.com/test/method", results.Value.EndpointUrl);
            Assert.IsTrue(results.Value.Timestamp > 0);
            Assert.AreEqual(ControllerContextHelper.DefaultUserId, results.Value.CallerId);
            Assert.AreEqual(ControllerContextHelper.DefaultUserName, results.Value.CallerName);
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
            Assert.IsNotNull(results.Value);
            Assert.AreEqual("Ok", results.Value.Result);
            Assert.AreEqual("https://test.com/test/method", results.Value.EndpointUrl);
            Assert.IsTrue(results.Value.Timestamp > 0);
            Assert.AreEqual(string.Empty, results.Value.CallerId);
            Assert.AreEqual(string.Empty, results.Value.CallerName);
        }
    }
}