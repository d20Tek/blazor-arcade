//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Client
{
    public interface ITypedHttpClient
    {
        public HttpClient HttpClient { get; }

        public Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}
