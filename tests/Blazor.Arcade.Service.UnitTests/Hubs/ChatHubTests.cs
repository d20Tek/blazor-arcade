//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Blazor.Arcade.Service.UnitTests.Hubs
{
    [TestClass]
    public class ChatHubTests
    {
        private readonly Mock<IHubCallerClients> _mockClients = new Mock<IHubCallerClients>();
        private readonly Mock<IClientProxy> _mockClientProxy = new Mock<IClientProxy>();
        private readonly Mock<IGroupManager> _mockGroups = new Mock<IGroupManager>();

        public ChatHubTests()
        {
            _mockClients.Setup(clients => clients.Caller).Returns(_mockClientProxy.Object);
            _mockClients.Setup(clients => clients.All).Returns(_mockClientProxy.Object);
            _mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_mockClientProxy.Object);
        }

        [TestMethod]
        public async Task SendGlobalMessage()
        {
            // arrange
            var message = new ChatMessage
            {
                MessageId = "global-message-1",
                AccountId = "test-user-1",
                AccountName = "Test User",
                Message = "Hi there!"
            };
            var hub = InitializeChatHub();

            // act
            await hub.SendGlobalMessage(message);

            // assert
            _mockClients.Verify(clients => clients.All, Times.Once);
            _mockClientProxy.Verify(
                proxy => proxy.SendCoreAsync(
                    "onReceiveMessage",
                    It.Is<object[]>(o => o != null && o.Length == 1 && o[0].GetType() == typeof(ChatMessage)),
                    default(CancellationToken)),
                Times.Once);
        }

        [TestMethod]
        public async Task SendMessage()
        {
            // arrange
            var hub = InitializeChatHub();

            // act
            await hub.SendMessage("Test User", "Hi there!");

            // assert
            _mockClients.Verify(clients => clients.All, Times.Once);
            _mockClientProxy.Verify(
                proxy => proxy.SendCoreAsync(
                    "ReceiveMessage",
                    It.Is<object[]>(o => o != null && o.Length == 2 && o[0].ToString() == "Test User" && o[1].ToString() == "Hi there!"),
                    default(CancellationToken)),
                Times.Once);
        }

        private ChatHub InitializeChatHub()
        {
            var hub = new ChatHub();
            hub.Clients = _mockClients.Object;
            hub.Groups = _mockGroups.Object;

            return hub;
        }
    }
}
