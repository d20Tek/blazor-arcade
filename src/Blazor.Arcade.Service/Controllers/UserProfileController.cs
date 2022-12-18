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
    [Route("api/v1/profile")]
    public class UserProfileController : ArcadeControllerBase
    {
        private readonly IUserProfileActionManager _actionMgr;

        public UserProfileController(
            IUserProfileActionManager actionMgr,
            ILogger<UserProfileController> logger)
            : base(logger)
        {
            _actionMgr = actionMgr;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserProfile>>> GetProfiles()
        {
            return await EndpointOperationAsync<List<UserProfile>>(nameof(GetProfiles), async () =>
            {
                var userId = AuthClaims.GetAuthUserId(User);
                var results = await _actionMgr.GetProfilesForUserAsync(userId);
                return results.ToList();
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile?>> GetProfileById(string id)
        {
            return await EndpointOperationAsync<UserProfile?>(nameof(GetProfileById), async () =>
            {
                var userId = AuthClaims.GetAuthUserId(User);
                return await _actionMgr.GetProfileForUserAsync(id, userId);
            });
        }

        [HttpPost]
        public async Task<ActionResult<UserProfile>> CreateProfile([FromBody]UserProfile profile)
        {
            return await EndpointOperationAsync<UserProfile>(nameof(CreateProfile), async () =>
            {
                profile.UserId = AuthClaims.GetAuthUserId(User);
                return await _actionMgr.CreateProfileForUserAsync(profile);
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserProfile>> UpdateProfile(
            string id,
            [FromBody] UserProfile profile)
        {
            return await EndpointOperationAsync<UserProfile>(nameof(UpdateProfile), async () =>
            {
                profile.Id = id;
                profile.UserId = AuthClaims.GetAuthUserId(User);
                return await _actionMgr.UpdateProfileForUserAsync(profile);
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserProfile>> DeleteProfile(string id)
        {
            return await EndpointOperationAsync<UserProfile>(nameof(DeleteProfile), async () =>
            {
                var userId = AuthClaims.GetAuthUserId(User);
                await _actionMgr.DeleteProfileForUserAsync(id, userId);
                return StatusCode(StatusCodes.Status204NoContent);
            });
        }
    }
}
