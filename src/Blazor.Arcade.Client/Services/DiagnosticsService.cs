//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;
using System.Net.Http.Json;

namespace Blazor.Arcade.Client.Services
{
    internal class DiagnosticsService : ClientServiceBase, IDiagnosticsService
    {
        private const string _diagUri = "/api/v1/diag/auth";
        private readonly HttpClient _client;

        public DiagnosticsService(ITypedHttpClient typedClient, ILogger<DiagnosticsService> logger)
            : base(logger)
        {
            _client = typedClient.HttpClient;
        }

        public async Task<ServiceDiagnostics?> GetAuthDiagnosticsAsync()
        {
            var response = await _client.GetAsync(_diagUri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ServiceDiagnostics>();
        }
    }
}
