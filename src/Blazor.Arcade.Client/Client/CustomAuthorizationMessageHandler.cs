using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace Blazor.Arcade.Client.Client
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://localhost:7027" },
                scopes: new[] { "api://867d4333-437a-42c4-aa1e-830997428b5e/access_as_user" });
        }
    }
}
