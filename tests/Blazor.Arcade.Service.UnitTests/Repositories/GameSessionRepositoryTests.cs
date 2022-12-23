//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure.Cosmos;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Service.Repositories;
using Microsoft.Extensions.Logging;

namespace Blazor.Arcade.Service.UnitTests.Repositories
{
    [TestClass]
    public class GameSessionRepositoryTests
    {
        [TestMethod]
        public void CreateRepository()
        {
            // arrange
            var logger = new Mock<ILogger<GameSessionRepository>>().Object;
            var cache = new Mock<ICacheService>().Object;
            var mockClient = new Mock<CosmosClient>();

            // act
            var repo = new GameSessionRepository(
                mockClient.Object, cache, logger);

            // assert
            Assert.IsNotNull(repo);
        }
    }
}
