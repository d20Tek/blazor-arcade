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
        private readonly ITypedHttpClient _client;

        public DiagnosticsService(ITypedHttpClient client, ILogger<DiagnosticsService> logger)
            : base(logger)
        {
            _client = client;
        }

        public async Task<ServiceDiagnostics?> GetAuthDiagnosticsAsync()
        {
            var response = await _client.GetAsync(_diagUri);
            return await response.Content.ReadFromJsonAsync<ServiceDiagnostics>();
        }
    }
}
