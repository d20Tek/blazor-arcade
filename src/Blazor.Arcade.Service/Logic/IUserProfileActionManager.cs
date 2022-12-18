//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Service.Logic
{
    public interface IUserProfileActionManager
    {
        Task<UserProfile> GetProfileForUserAsync(string profileId, string userId);

        Task<IList<UserProfile>> GetProfilesForUserAsync(string userId);

        Task<UserProfile> CreateProfileForUserAsync(UserProfile profile);

        Task<UserProfile> UpdateProfileForUserAsync(UserProfile profile);

        Task DeleteProfileForUserAsync(string profileId, string userId);
    }
}
