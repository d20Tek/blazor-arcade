//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Common.Models.Requests;
using Blazor.Arcade.Service.Hubs;
using Blazor.Arcade.Service.Logic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Service.UnitTests.Hubs
{
    [TestClass]
    public class GameSessionHubTests
    {
        private readonly ILogger<GameSessionHub> _logger = new Mock<ILogger<GameSessionHub>>().Object;
        private readonly Mock<IHubCallerClients> _mockClients = new Mock<IHubCallerClients>();
        private readonly Mock<ISingleClientProxy> _mockClientProxy = new Mock<ISingleClientProxy>();
        private readonly Mock<IGroupManager> _mockGroups = new Mock<IGroupManager>();

        public GameSessionHubTests()
        {
            _mockClients.Setup(clients => clients.Caller).Returns(_mockClientProxy.Object);
            _mockClients.Setup(clients => clients.All).Returns(_mockClientProxy.Object);
            _mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_mockClientProxy.Object);
        }

        [TestMethod]
        public async Task CreateSessionAsync()
        {
            // arrange
            var (actionMgr, session) = CreateMockGameSessionManager();
            var hub = InitializeGameSessionHub(actionMgr);

            var request = new GameSessionCreateRequest
            {
                Name = "Test Session",
                HostId = "test-profile-id",
                ServerId = "s1",
                MetadataId = "arcade.game.tictactoe",
            };

            // act
            var result = await hub.CreateSessionAsync(request);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Equals(session));
            _mockClientProxy.Verify(
                proxy => proxy.SendCoreAsync(
                    "onSessionChanged",
                    It.Is<object[]>(o => o != null && o.Length == 1 && o[0].GetType() == typeof(GameSession)),
                    default(CancellationToken)),
                Times.Once);
        }

        [TestMethod]
        public async Task RegisterWithGroup()
        {
            // arrange
            var (actionMgr, _) = CreateMockGameSessionManager();
            var hub = InitializeGameSessionHub(actionMgr);

            // act
            await hub.RegisterWithGroup("test-group");

            // assert
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockGroups.Verify(o => o.AddToGroupAsync(
                        It.IsAny<string>(), "test-group", It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        private (GameSessionActionManager, GameSession) CreateMockGameSessionManager()
        {
            var session = new GameSession()
            {
                Id = "test-session-id",
                Name = "Test Session",
                ServerId = "s1",
                MetadataId = "arcade.game.tictactoe",
                HostId = "test-profile-id",
                Phase = (int)GameSessionPhase.Created,
            };
            var mockRepo = new Mock<IRepository<GameSession>>();
            mockRepo.Setup(x => x.CreateItemAsync(It.IsAny<GameSession>()))
                    .ReturnsAsync(session);

            return (new GameSessionActionManager(mockRepo.Object), session);
        }

        private GameSessionHub InitializeGameSessionHub(
            GameSessionActionManager actionMgr,
            ILogger<GameSessionHub>? logger = null)
        {
            logger ??= _logger;
            var context = new Mock<HubCallerContext>();
            context.Setup(x => x.ConnectionId).Returns("test-connection-id");

            var hub = new GameSessionHub(actionMgr, logger)
            {
                Context= context.Object,
                Clients = _mockClients.Object,
                Groups = _mockGroups.Object
            };

            return hub;
        }
    }
}
