//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Arcade.Misc
{
    internal static class AuthenticationHelper
    {
        private const string _idClaim = "oid";

        public static async Task<bool> IsUserAuthenticated(Task<AuthenticationState>? authStateTask)
        {
            if (authStateTask == null) return false;

            var authState = await authStateTask;
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

        public static async Task<string> GetUserId(Task<AuthenticationState>? authStateTask)
        {
            if (authStateTask == null) return string.Empty;

            var authState = await authStateTask;
            var userId = authState.User.FindFirst(_idClaim)?.Value ?? string.Empty;
            return userId;
        }
    }
}
