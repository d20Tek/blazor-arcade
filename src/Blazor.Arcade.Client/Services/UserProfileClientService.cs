//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Types;
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Arcade.Client.Services
{
    internal class UserProfileClientService : CrudClientService<UserProfile>, IUserProfileClientService
    {
        private const string _baseServiceUrl = "/api/v1/account";
        private readonly ILocalStorageService _storageService;

        public UserProfileClientService(
            ITypedHttpClient typedClient,
            ILocalStorageService storageService,
            ILogger<UserProfileClientService> logger)
            : base(_baseServiceUrl, typedClient, logger)
        {
            _storageService = storageService;
        }

        public async Task<bool> HasCurrentProfileAsync(
            Task<AuthenticationState>? authState)
        {
            var profileKey = await Constants.GetUserAccountKey(authState);
            return await _storageService.ContainKeyAsync(profileKey);
        }

        public async Task<UserProfile> GetCurrentProfileAsync(
            Task<AuthenticationState>? authState)
        {
            var profileKey = await Constants.GetUserAccountKey(authState);
            var profile = await _storageService.GetItemAsync<UserProfile>(profileKey);

            return profile ?? new UserProfile();
        }

        public async Task SetCurrentProfileAsync(
            Task<AuthenticationState>? authState, UserProfile? account)
        {
            var profileKey = await Constants.GetUserAccountKey(authState);
            if (account != null)
            {
                await _storageService.SetItemAsync<UserProfile>(profileKey, account);
            }
            else
            {
                await _storageService.RemoveItemAsync(profileKey);
            }
        }
    }
}
