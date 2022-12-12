//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;

namespace Blazor.Arcade.Client.Services
{
    internal class UserAccountClientService : CrudClientService<UserAccount>
    {
        private const string _baseServiceUrl = "/api/v1/account";

        public UserAccountClientService(
            ITypedHttpClient typedClient,
            ILogger<UserAccountClientService> logger)
            : base(_baseServiceUrl, typedClient, logger)
        {
        }
    }
}
