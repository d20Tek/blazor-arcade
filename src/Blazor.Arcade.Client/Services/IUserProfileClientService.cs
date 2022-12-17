//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Arcade.Client.Services
{
    public interface IUserProfileClientService : ICrudClientService<UserProfile>
    {
        public Task<bool> HasCurrentProfileAsync(Task<AuthenticationState>? authState);

        public Task<UserProfile> GetCurrentProfileAsync(Task<AuthenticationState>? authState);

        public Task SetCurrentProfileAsync(
            Task<AuthenticationState>? authState,
            UserProfile? account);
    }
}
