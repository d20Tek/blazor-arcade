//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Moq.Protected;
using System.Net;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class ArcadeClientServiceTests
    {
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

            var httpClient = CreateHttpClient(responseContent);
            var service = new ArcadeService(httpClient);

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

        private HttpClient CreateHttpClient(string returnedContent)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                       .Setup<Task<HttpResponseMessage>>(
                          "SendAsync",
                          ItExpr.IsAny<HttpRequestMessage>(),
                          ItExpr.IsAny<CancellationToken>()
                       )
                       .ReturnsAsync(new HttpResponseMessage()
                       {
                           StatusCode = HttpStatusCode.OK,
                           Content = new StringContent(returnedContent),
                       })
                       .Verifiable();

            // create http client with our mocked handler.
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

            return httpClient;
        }
    }
}
