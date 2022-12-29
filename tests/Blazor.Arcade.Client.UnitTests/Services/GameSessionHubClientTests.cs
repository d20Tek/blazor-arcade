//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Common.Models.Requests;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class GameSessionHubClientTests
    {
        private readonly Mock<ILogger<GameSessionHubClient>> _mockLogger = 
            new Mock<ILogger<GameSessionHubClient>>();
        private readonly Mock<IHubProxy<IGameSessionHubClient>> _mockProxy =
            new Mock<IHubProxy<IGameSessionHubClient>>();

        [TestMethod]
        public async Task Initialize()
        {
            // arrange
            _mockProxy.Setup(p => p.State).Returns(HubConnectionState.Disconnected);
            var hub = new GameSessionHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            await hub.InitializeAsync("test-group");

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
                    Times.AtLeastOnce);
            }
        }

        [TestMethod]
        public async Task Initialize_NotDisconnected()
        {
            // arrange
            _mockProxy.Setup(p => p.State).Returns(HubConnectionState.Connected);
            var hub = new GameSessionHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            await hub.InitializeAsync("test-group");

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
            var hub = new GameSessionHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            await hub.InitializeAsync("test-group");

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
        public async Task CreateSessionAsync()
        {
            // arrange
            var profile = new UserProfile
            {
                Id = "test-profile-id",
                Name = "Test User",
                Server = "s1",
                UserId = "test-user-id",
            };

            var session = new GameSession
            {
                Id = "test-session-id",
                Name = "Test Session",
                MetadataId = "game1",
                HostId = "test-profile-1",
                ServerId = "s1",
                Phase = 0,
            };

            _mockProxy.Setup(x => x.InvokeAsync<GameSession>(
                        "CreateSessionAsync",
                        It.IsAny<GameSessionCreateRequest>(),
                        It.IsAny<CancellationToken>()))
                      .ReturnsAsync(session);
            var hub = new GameSessionHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            var result = await hub.CreateSessionAsync(profile, "game1", "Test Session");

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Equals(session));
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockProxy.Verify(
                    o => o.InvokeAsync<GameSession>(
                        "CreateSessionAsync",
                        It.IsAny<GameSessionCreateRequest>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task CreateSessionAsync_WithError()
        {
            // arrange
            var profile = new UserProfile
            {
                Id = "test-profile-id",
                Name = "Test User",
                Server = "s1",
                UserId = "test-user-id",
            };

            _mockProxy.Setup(p => p.InvokeAsync<GameSession>(
                        It.IsAny<string>(),
                        It.IsAny<GameSessionCreateRequest>(),
                        It.IsAny<CancellationToken>()))
                      .Throws<InvalidOperationException>();
            var hub = new GameSessionHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            var result = await hub.CreateSessionAsync(profile, "game1", "Test Session");

            // assert
        }

        [TestMethod]
        public void AddSessionChangedHandler()
        {
            // arrange
            var hub = new GameSessionHubClient(_mockProxy.Object, _mockLogger.Object);

            // act
            hub.AddSessionChangedHandler(OnSessionChanged);

            // assert
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockProxy.Verify(
                    o => o.On<GameSession>("onSessionChanged", It.IsAny<Action<GameSession>>()),
                    Times.Once);
            }
        }

        [TestMethod]
        public void RemoveReceiveMessageHandler()
        {
            // arrange
            var hub = new GameSessionHubClient(_mockProxy.Object, _mockLogger.Object);
            hub.AddSessionChangedHandler(OnSessionChanged);

            // act
            hub.RemoveReceiveMessageHandler(OnSessionChanged);

            // assert
            Verify();

            [ExcludeFromCodeCoverage]
            void Verify()
            {
                _mockProxy.Verify(
                    o => o.Off<GameSession>("onSessionChanged", It.IsAny<Action<GameSession>>()),
                    Times.Once);
            }
        }

        [ExcludeFromCodeCoverage]
        private void OnSessionChanged(GameSession session) { }
    }
}
