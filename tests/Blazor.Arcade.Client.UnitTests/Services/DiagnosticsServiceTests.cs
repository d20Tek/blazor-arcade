//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
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
            var responseContent = @"
            {
                ""result"": ""Ok"",
                ""endpointUrl"": ""https://test.com/api"",
                ""callerId"": ""test-user-id"",
                ""callerName"": ""Test User"",
                ""timestamp"": 1234
            }";

            var typedClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new DiagnosticsService(typedClient, _logger);

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
