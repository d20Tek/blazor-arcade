//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Common.Models.Requests;
using Blazor.Arcade.Service.Logic;

namespace Blazor.Arcade.Service.Hubs
{
    public class TestHub : ArcadeHubBase
    {
        private const string _onSessionChangedMessage = "onSessionChanged";
        private readonly IGameSessionActionManager _actionMgr;

        public TestHub(
            IGameSessionActionManager actionMgr,
            ILogger<TestHub> logger)
            : base(logger)
        {
            _actionMgr = actionMgr;
        }

        public async Task RegisterWithGroup(string groupName)
        {
            await this.HubOperationAsync(
                nameof(RegisterWithGroup),
                async () =>
                {
                    await AddToGroupAsync(groupName);
                });
        }

        public async Task<GameSession> CreateSessionAsync(GameSessionCreateRequest newRequest)
        {
            return await HubOperationAsync<GameSession>(
                nameof(CreateSessionAsync),
                async () =>
                {
                    var result = await _actionMgr.CreateSessionAsync(newRequest);
                    await SendGroupMessageAsync(newRequest.ServerId, _onSessionChangedMessage, result);

                    return result;
                });
        }
    }
}
