//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Client.Services
{
    public interface IDiagnosticsService
    {
        public Task<ServiceDiagnostics?> GetAuthDiagnosticsAsync();
    }
}
