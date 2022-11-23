//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using System.Net.Http.Json;

namespace Blazor.Arcade.Client.Services
{
    public class ArcadeClientService : IArcadeClientService
    {
        public HttpClient _httpClient;

        public ArcadeClientService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<ServiceDiagnostics?> GetAuthDiagnosticsAsync()
        {
            var response = await _httpClient.GetAsync("/api/v1/diag/auth");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ServiceDiagnostics>();
        }
    }
}
