//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common;
using Blazor.Arcade.Common.Core;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Service.Logic
{
    internal class UserProfileActionManager : IUserProfileActionManager
    {
        private readonly IRepository<UserProfile> _repo;
        private readonly IReadRepository<ServerMetadata> _serverMetadata;

        public UserProfileActionManager(
            IRepository<UserProfile> repository,
            IReadRepository<ServerMetadata> serverMetadata)
        {
            _repo= repository;
            _serverMetadata= serverMetadata;
        }

        public async Task<UserProfile> GetProfileForUserAsync(string profileId, string userId)
        {
            profileId.ThrowIfEmpty(nameof(profileId));
            userId.ThrowIfEmpty(nameof(userId));

            return await _repo.GetItemAsync(profileId, userId);
        }

        public async Task<IList<UserProfile>> GetProfilesForUserAsync(string userId)
        {
            userId.ThrowIfEmpty(nameof(userId));
            return await _repo.GetPartitionItemsAsync(userId);
        }

        public async Task<UserProfile> CreateProfileForUserAsync(UserProfile profile)
        {
            ArgumentNullException.ThrowIfNull(nameof(profile));

            var s = await _serverMetadata.GetById(profile.Server);
            if (s == null)
            {
                s = (await _serverMetadata.GetAll()).Last();
                profile.Server = s.Name;
            }
            
            profile.Id = IdGenerator.Instance.GetNext(s.Prefix);

            profile.ValidateModel();
            return await _repo.CreateItemAsync(profile);
        }

        public async Task<UserProfile> UpdateProfileForUserAsync(UserProfile profile)
        {
            ArgumentNullException.ThrowIfNull(nameof(profile));
            var servers = await _serverMetadata.GetAll();
            if (servers.Any(p => p.Name == profile.Server) == false)
            {
                throw new FormatException("Specified account server does not exist.");
            }

            var accountToUpdate = await _repo.GetItemAsync(profile.Id, profile.UserId);
            if (profile.Equals(accountToUpdate))
            {
                return accountToUpdate;
            }

            profile.ValidateModel();
            return await _repo.UpdateItemAsync(profile);
        }

        public async Task DeleteProfileForUserAsync(string profileId, string userId)
        {
            profileId.ThrowIfEmpty(nameof(profileId));
            userId.ThrowIfEmpty(nameof(userId));

            await _repo.DeleteItemAsync(profileId, userId);
        }
    }
}
