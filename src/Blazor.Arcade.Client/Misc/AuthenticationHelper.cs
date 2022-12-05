//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Arcade.Misc
{
    public static class AuthenticationHelper
    {
        public static async Task<bool> IsUserAuthenticated(Task<AuthenticationState>? authStateTask)
        {
            if (authStateTask == null) return false;

            var authState = await authStateTask;
            return authState.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
