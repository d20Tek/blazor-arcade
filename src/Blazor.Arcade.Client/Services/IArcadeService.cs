//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Client.Services
{
    public interface IArcadeService
    {
        public Task<ServiceDiagnostics?> GetAuthDiagnosticsAsync();

        public Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}
