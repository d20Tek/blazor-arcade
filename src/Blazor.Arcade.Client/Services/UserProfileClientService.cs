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
    internal class UserProfileClientService : CrudClientService<UserAccount>, IUserProfileClientService
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

        public async Task<UserAccount> GetCurrentProfileAsync(
            Task<AuthenticationState>? authState)
        {
            var profileKey = await Constants.GetUserAccountKey(authState);
            var profile = await _storageService.GetItemAsync<UserAccount>(profileKey);

            return profile ?? new UserAccount();
        }

        public async Task SetCurrentProfileAsync(
            Task<AuthenticationState>? authState, UserAccount? account)
        {
            var profileKey = await Constants.GetUserAccountKey(authState);
            if (account != null)
            {
                await _storageService.SetItemAsync<UserAccount>(profileKey, account);
            }
            else
            {
                await _storageService.RemoveItemAsync(profileKey);
            }
        }
    }
}
