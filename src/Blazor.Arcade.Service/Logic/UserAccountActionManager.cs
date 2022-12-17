//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common;
using Blazor.Arcade.Common.Core;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Service.Logic
{
    internal class UserAccountActionManager : IUserAccountActionManager
    {
        private readonly IRepository<UserProfile> _repo;
        private readonly IReadRepository<ServerMetadata> _serverMetadata;

        public UserAccountActionManager(
            IRepository<UserProfile> repository,
            IReadRepository<ServerMetadata> serverMetadata)
        {
            _repo= repository;
            _serverMetadata= serverMetadata;
        }

        public async Task<UserProfile> GetAccountForUserAsync(string accountId, string userId)
        {
            accountId.ThrowIfEmpty(nameof(accountId));
            userId.ThrowIfEmpty(nameof(userId));

            return await _repo.GetItemAsync(accountId, userId);
        }

        public async Task<IList<UserProfile>> GetAccountsForUserAsync(string userId)
        {
            userId.ThrowIfEmpty(nameof(userId));
            return await _repo.GetPartitionItemsAsync(userId);
        }

        public async Task<UserProfile> CreateAccountForUserAsync(UserProfile account)
        {
            ArgumentNullException.ThrowIfNull(nameof(account));

            var s = await _serverMetadata.GetById(account.Server);
            if (s == null)
            {
                s = (await _serverMetadata.GetAll()).Last();
                account.Server = s.Name;
            }
            
            account.Id = IdGenerator.Instance.GetNext(s.Prefix);

            account.ValidateModel();
            return await _repo.CreateItemAsync(account);
        }

        public async Task<UserProfile> UpdateAccountForUserAsync(UserProfile account)
        {
            ArgumentNullException.ThrowIfNull(nameof(account));
            var servers = await _serverMetadata.GetAll();
            if (servers.Any(p => p.Name == account.Server) == false)
            {
                throw new FormatException("Specified account server does not exist.");
            }

            var accountToUpdate = await _repo.GetItemAsync(account.Id, account.UserId);
            if (account.Equals(accountToUpdate))
            {
                return accountToUpdate;
            }

            account.ValidateModel();
            return await _repo.UpdateItemAsync(account);
        }

        public async Task DeleteAccountForUserAsync(string accountId, string userId)
        {
            accountId.ThrowIfEmpty(nameof(accountId));
            userId.ThrowIfEmpty(nameof(userId));

            await _repo.DeleteItemAsync(accountId, userId);
        }
    }
}
