//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Service.Logic
{
    internal interface IUserAccountActionManager
    {
        Task<UserAccount> GetAccountForUserAsync(string accountId, string userId);

        Task<IList<UserAccount>> GetAccountsForUserAsync(string userId);

        Task<UserAccount> CreateAccountForUserAsync(UserAccount account);

        Task<UserAccount> UpdateAccountForUserAsync(UserAccount account);

        Task DeleteAccountForUserAsync(string accountId, string userId);
    }
}
