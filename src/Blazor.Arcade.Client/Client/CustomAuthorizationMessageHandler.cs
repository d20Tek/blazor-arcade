//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace Blazor.Arcade.Client
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager,
            IConfiguration configuration)
            : base(provider, navigationManager)
        {
            var serviceUrl = configuration["ArcadeServiceUrl"];

            ConfigureHandler(
                authorizedUrls: new[] { serviceUrl },
                scopes: new[] { "api://867d4333-437a-42c4-aa1e-830997428b5e/access_as_user" });
        }
    }
}
