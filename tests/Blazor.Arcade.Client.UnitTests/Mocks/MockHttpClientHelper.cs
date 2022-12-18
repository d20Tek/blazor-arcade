//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Client;
using Moq.Protected;
using System.Net;

namespace Blazor.Arcade.Client.UnitTests.Mocks
{
    internal class MockHttpClientHelper
    {
        public static ITypedHttpClient CreateTypedHttpClient(string returnedContent)
        {
            return new TypedHttpClient(CreateHttpClient(HttpStatusCode.OK, returnedContent));
        }

        public static ITypedHttpClient CreateTypedHttpClient_StatusCodeError(HttpStatusCode statusCode, string returnedContent = "")
        {
            return new TypedHttpClient(CreateHttpClient(statusCode, returnedContent));
        }

        public static HttpClient CreateHttpClient(string returnedContent)
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

        public static HttpClient CreateHttpClient(HttpStatusCode statusCode, string returnedContent)
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
                           StatusCode = statusCode,
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
