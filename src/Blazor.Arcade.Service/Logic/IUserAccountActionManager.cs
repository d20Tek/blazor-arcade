//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Service.Logic
{
    public interface IUserAccountActionManager
    {
        Task<UserProfile> GetAccountForUserAsync(string accountId, string userId);

        Task<IList<UserProfile>> GetAccountsForUserAsync(string userId);

        Task<UserProfile> CreateAccountForUserAsync(UserProfile account);

        Task<UserProfile> UpdateAccountForUserAsync(UserProfile account);

        Task DeleteAccountForUserAsync(string accountId, string userId);
    }
}
