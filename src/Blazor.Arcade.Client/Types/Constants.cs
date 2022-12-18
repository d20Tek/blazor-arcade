//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Misc;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Arcade.Client.Types
{
    internal static class Constants
    {
        // local storage keys
        public const string AccountKeyBase = "CurrentUserAccount";

        public static async Task<string> GetUserAccountKey(Task<AuthenticationState>? AuthState)
        {
            var userId = await AuthenticationHelper.GetUserId(AuthState);
            return $"{userId}.{AccountKeyBase}";
        }
    }
}
