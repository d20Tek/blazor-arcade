//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Client;
using Moq.Protected;
using System.Net;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class TypedHttpClientTests
    {
        [TestMethod]
        public async Task GetAsync()
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
            var service = new TypedHttpClient(httpClient);

            // act
            var result = await service.GetAsync("http://test.com/");

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccessStatusCode);
            Assert.IsNotNull(result.Content);
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
