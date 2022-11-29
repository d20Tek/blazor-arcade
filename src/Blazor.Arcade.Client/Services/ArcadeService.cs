//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using System.Net.Http.Json;

namespace Blazor.Arcade.Client.Services
{
    internal class ArcadeService : IArcadeService
    {
        private const string _diagUri = "/api/v1/diag/auth";
        private HttpClient _httpClient;

        public ArcadeService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<ServiceDiagnostics?> GetAuthDiagnosticsAsync()
        {
            var response = await GetAsync(_diagUri);
            return await response.Content.ReadFromJsonAsync<ServiceDiagnostics>();
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}
