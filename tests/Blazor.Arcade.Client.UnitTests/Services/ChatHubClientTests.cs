//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class ChatHubClientTests
    {
        private readonly Mock<ILogger<ChatHubClient>> _mockLogger = new Mock<ILogger<ChatHubClient>>();
        private readonly Mock<IHubProxy<IChatHubClient>> _mockProxy = new Mock<IHubProxy<IChatHubClient>>();

        [TestMethod]
        public async Task Initialize()
        {
            // arrange
            _mockProxy.Setup(p => p.State).Returns(HubConnectionState.Disconnected);
            var hub = new ChatHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            await hub.InitializeAsync();

            // assert
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockProxy.Verify(o => o.StartAsync(), Times.Once);
                _mockLogger.Verify(
                    o => o.Log(
                        LogLevel.Trace,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                    Times.Once);
            }
        }

        [TestMethod]
        public async Task Initialize_NotDisconnected()
        {
            // arrange
            _mockProxy.Setup(p => p.State).Returns(HubConnectionState.Connected);
            var hub = new ChatHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            await hub.InitializeAsync();

            // assert
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockProxy.Verify(o => o.StartAsync(), Times.Never);
                _mockLogger.Verify(
                    o => o.Log(
                        LogLevel.Trace,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                    Times.Never);
            }
        }

        [TestMethod]
        public async Task Initialize_WithError()
        {
            // arrange
            _mockProxy.Setup(p => p.StartAsync()).Throws<InvalidOperationException>();
            var hub = new ChatHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            await hub.InitializeAsync();

            // assert
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockProxy.Verify(o => o.StartAsync(), Times.Once);
                _mockLogger.Verify(
                o => o.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
            }
        }

        [TestMethod]
        public async Task SendGlobalMessageAsync()
        {
            // arrange
            var message = new ChatMessage
            {
                ProfileId = "test-user-id",
                ProfileName = "Test User",
                Message = "Test message",
            };
            var hub = new ChatHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            await hub.SendGlobalMessageAsync(message);

            // assert
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockProxy.Verify(
                    o => o.InvokeAsync("SendGlobalMessage", It.IsAny<ChatMessage>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task SendGlobalMessageAsync_WithError()
        {
            // arrange
            var message = new ChatMessage
            {
                ProfileId = "test-user-id",
                ProfileName = "Test User",
                Message = "Test message",
            };

            _mockProxy.Setup(p => p.InvokeAsync(It.IsAny<string>(), It.IsAny<ChatMessage>(), It.IsAny<CancellationToken>()))
                      .Throws<InvalidOperationException>();
            var hub = new ChatHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            await hub.SendGlobalMessageAsync(message);

            // assert

        }

        [TestMethod]
        public void OnReceiveMessageHandler()
        {
            // arrange
            var hub = new ChatHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            hub.AddReceiveMessageHandler(OnReceiveMessage);

            // assert
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockProxy.Verify(
                    o => o.On<ChatMessage>("onReceiveMessage", It.IsAny<Action<ChatMessage>>()),
                    Times.Once);
            }
        }

        [ExcludeFromCodeCoverage]
        private void OnReceiveMessage(ChatMessage message) { }
    }
}
