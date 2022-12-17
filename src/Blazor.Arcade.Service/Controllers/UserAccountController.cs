//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Logic;
using Blazor.Arcade.Service.Misc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blazor.Arcade.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/account")]
    public class UserAccountController : ArcadeControllerBase
    {
        private readonly IUserAccountActionManager _actionMgr;

        public UserAccountController(
            IUserAccountActionManager actionMgr,
            ILogger<UserAccountController> logger)
            : base(logger)
        {
            _actionMgr = actionMgr;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserProfile>>> GetAccounts()
        {
            return await EndpointOperationAsync<List<UserProfile>>(nameof(GetAccounts), async () =>
            {
                var userId = AuthClaims.GetAuthUserId(User);
                var results = await _actionMgr.GetAccountsForUserAsync(userId);
                return results.ToList();
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile?>> GetAccountById(string id)
        {
            return await EndpointOperationAsync<UserProfile?>(nameof(GetAccountById), async () =>
            {
                var userId = AuthClaims.GetAuthUserId(User);
                return await _actionMgr.GetAccountForUserAsync(id, userId);
            });
        }

        [HttpPost]
        public async Task<ActionResult<UserProfile>> CreateAccount([FromBody]UserProfile account)
        {
            return await EndpointOperationAsync<UserProfile>(nameof(CreateAccount), async () =>
            {
                account.UserId = AuthClaims.GetAuthUserId(User);
                return await _actionMgr.CreateAccountForUserAsync(account);
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserProfile>> UpdateAccount(
            string id,
            [FromBody] UserProfile account)
        {
            return await EndpointOperationAsync<UserProfile>(nameof(UpdateAccount), async () =>
            {
                account.Id = id;
                account.UserId = AuthClaims.GetAuthUserId(User);
                return await _actionMgr.UpdateAccountForUserAsync(account);
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserProfile>> DeleteAccount(string id)
        {
            return await EndpointOperationAsync<UserProfile>(nameof(DeleteAccount), async () =>
            {
                var userId = AuthClaims.GetAuthUserId(User);
                await _actionMgr.DeleteAccountForUserAsync(id, userId);
                return StatusCode(StatusCodes.Status204NoContent);
            });
        }
    }
}
