//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Security.Claims;

namespace Blazor.Arcade.Service.Misc
{
    internal class AuthClaims
    {
        private const string _claimsIdType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        private const string _claimsNameType = "name";

        internal static string GetAuthUserId(ClaimsPrincipal user) =>
            user.FindFirst(_claimsIdType)?.Value ?? string.Empty;

        internal static string GetAuthUserName(ClaimsPrincipal user) =>
            user.FindFirst(_claimsNameType)?.Value ?? string.Empty;
    }
}
