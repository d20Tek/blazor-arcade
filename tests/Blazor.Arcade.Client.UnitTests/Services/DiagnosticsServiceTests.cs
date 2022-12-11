//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class DiagnosticsServiceTests
    {
        private const string _baseServiceUri = "/api/v1/diag/auth";
        private readonly ILogger<DiagnosticsService> _logger = new Mock<ILogger<DiagnosticsService>>().Object;

        [TestMethod]
        public async Task GetAuthDiagnosticsAsync()
        {
            // arrange
            var diags = new ServiceDiagnostics
            {
                Result = "Ok",
                EndpointUrl = "https://test.com/api",
                CallerId = "test-user-id",
                CallerName = "Test User",
                Timestamp = 1234
            };

            var testResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create<ServiceDiagnostics>(diags)
            };

            var mockClient = new Mock<ITypedHttpClient>();
            mockClient.Setup(p => p.GetAsync(_baseServiceUri))
                      .ReturnsAsync(testResponse);

            var service = new DiagnosticsService(mockClient.Object, _logger);

            // act
            var result = await service.GetAuthDiagnosticsAsync();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Ok", result.Result);
            Assert.AreEqual("https://test.com/api", result.EndpointUrl);
            Assert.AreEqual("test-user-id", result.CallerId);
            Assert.AreEqual("Test User", result.CallerName);
            Assert.AreEqual(1234, result.Timestamp);
        }
    }
}
