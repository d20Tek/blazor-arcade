//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics.CodeAnalysis;

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
                ProfileId = "test-profile-1",
                ProfileName = "Test User",
                Message = "Hi there!",
                UserId = "test-user-1"
            };
            var hub = InitializeChatHub();

            // act
            await hub.SendGlobalMessage(message);

            // assert
            _mockClients.Verify(clients => clients.All, Times.Once);
            _mockClientProxy.Verify(
                proxy => proxy.SendCoreAsync(
                    "onReceiveMessage",
                    It.Is<object[]>(o => o != null && o.Length == 1 && ValidateChatMessage(o[0], message)),
                    default(CancellationToken)),
                Times.Once);
        }

        [ExcludeFromCodeCoverage]
        private bool ValidateChatMessage(object objMessage, ChatMessage expected)
        {
            var message = (ChatMessage)objMessage;
            return message.MessageId != string.Empty &&
                   message.ChannelId == "channel:global" &&
                   message.ProfileId == expected.ProfileId &&
                   message.ProfileName == expected.ProfileName &&
                   message.Message == expected.Message &&
                   message.UserId == expected.UserId;
        }

        private ChatHub InitializeChatHub()
        {
            var hub = new ChatHub
            {
                Clients = _mockClients.Object,
                Groups = _mockGroups.Object
            };

            return hub;
        }
    }
}
