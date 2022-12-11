//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Client
{
    public class TypedHttpClient : ITypedHttpClient
    {
        public TypedHttpClient(HttpClient httpClient)
        {
            HttpClient= httpClient;
        }

        public HttpClient HttpClient { get; private set; }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            var response = await HttpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}
