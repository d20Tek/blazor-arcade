//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Service.UnitTests.Hubs
{
    [TestClass]
    public class ArcadeHubBaseTests
    {
        public sealed class TestHub : ArcadeHubBase
        {
            public TestHub()
                : base(new Mock<ILogger<TestHub>>().Object)
            {
            }

            public async Task<bool> SucceedAsync()
            {
                return await HubOperationAsync<bool>("Succeed", () =>
                {
                    return Task.FromResult<bool>(true);
                });
            }

            [ExcludeFromCodeCoverage]
            public async Task<bool> MessageFormatErrorAsync()
            {
                return await HubOperationAsync<bool>("MessageFormatError", () =>
                {
                    throw new ArgumentException("test-arg");
                });
            }

            [ExcludeFromCodeCoverage]
            public async Task<bool> UnexpectedErrorAsync()
            {
                return await HubOperationAsync<bool>("UnexpectedError", () =>
                {
                    throw new InvalidOperationException();
                });
            }

            [ExcludeFromCodeCoverage]
            public async Task<bool> NotFoundErrorAsync()
            {
                return await HubOperationAsync<bool>("NotFoundError", () =>
                {
                    throw new EntityNotFoundException("test", "value");
                });
            }

            public async Task SucceedVoidAsync()
            {
                await HubOperationAsync("Succeed", () =>
                {
                    return Task.CompletedTask;
                });
            }

            [ExcludeFromCodeCoverage]
            public async Task MessageFormatErrorVoidAsync()
            {
                await HubOperationAsync("MessageFormatError", () =>
                {
                    throw new ArgumentException("test-arg");
                });
            }

            [ExcludeFromCodeCoverage]
            public async Task UnexpectedErrorVoidAsync()
            {
                await HubOperationAsync("UnexpectedError", () =>
                {
                    throw new InvalidOperationException();
                });
            }

            [ExcludeFromCodeCoverage]
            public async Task NotFoundErrorVoidAsync()
            {
                await HubOperationAsync("NotFoundError", () =>
                {
                    throw new EntityNotFoundException("test", "value");
                });
            }
        }

        [TestMethod]
        public async Task HubOperationAsync_Success()
        {
            // arrange
            var hub = InitializeTestHub();

            // act
            var result = await hub.SucceedAsync();

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentException))]
        public async Task HubOperationAsync_MessageFormatError()
        {
            // arrange
            var hub = InitializeTestHub();

            // act
            _ = await hub.MessageFormatErrorAsync();

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task HubOperationAsync_UnexpectedError()
        {
            // arrange
            var hub = InitializeTestHub();

            // act
            _ = await hub.UnexpectedErrorAsync();

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task EndpointOperationAsync_AlreadyExistsError()
        {
            // arrange
            var hub = InitializeTestHub();

            // act
            _ = await hub.NotFoundErrorAsync();

            // assert
        }

        [TestMethod]
        public async Task HubOperationAsync2_Success()
        {
            // arrange
            var hub = InitializeTestHub();

            // act
            await hub.SucceedVoidAsync();

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentException))]
        public async Task HubOperationAsync2_MessageFormatError()
        {
            // arrange
            var hub = InitializeTestHub();

            // act
            await hub.MessageFormatErrorVoidAsync();

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task HubOperationAsync2_UnexpectedError()
        {
            // arrange
            var hub = InitializeTestHub();

            // act
            await hub.UnexpectedErrorVoidAsync();

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task EndpointOperationAsync2_AlreadyExistsError()
        {
            // arrange
            var hub = InitializeTestHub();

            // act
            await hub.NotFoundErrorVoidAsync();

            // assert
        }

        private TestHub InitializeTestHub()
        {
            var context = new Mock<HubCallerContext>();
            context.Setup(x => x.ConnectionId).Returns("test-connection-id");

            var _mockClientProxy = new Mock<ISingleClientProxy>();
            var _mockClients = new Mock<IHubCallerClients>();
            var _mockGroups = new Mock<IGroupManager>();
            _mockClients.Setup(clients => clients.Caller).Returns(_mockClientProxy.Object);
            _mockClients.Setup(clients => clients.All).Returns(_mockClientProxy.Object);
            _mockClients.Setup(clients => clients.Group(It.IsAny<string>())).Returns(_mockClientProxy.Object);

            var hub = new TestHub()
            {
                Context = context.Object,
                Clients = _mockClients.Object,
                Groups = _mockGroups.Object
            };

            return hub;
        }
    }
}
