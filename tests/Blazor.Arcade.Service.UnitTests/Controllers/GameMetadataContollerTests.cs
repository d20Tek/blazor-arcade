//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Controllers;
using Microsoft.Extensions.Logging;

namespace Blazor.Arcade.Service.UnitTests.Controllers
{
    [TestClass]
    public class GameMetadataContollerTests
    {
        private readonly ILogger<GameMetadataController> _logger = new Mock<ILogger<GameMetadataController>>().Object;

        [TestMethod]
        public async Task GetGameMetadataAll()
        {
            // arrange
            var games = new List<GameMetadata>
            {
                new GameMetadata { Id = "one" },
                new GameMetadata { Id = "two" },
                new GameMetadata { Id = "three" },
            };
            var mockRepo = new Mock<IReadRepository<GameMetadata>>();
            mockRepo.Setup(p => p.GetAll()).ReturnsAsync(games);

            var c = new GameMetadataController(mockRepo.Object, _logger);

            // act
            var results = await c.GetGameMetadataAll();

            // assert
            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Value);
            Assert.AreEqual(3, results.Value.Count);
        }

        [TestMethod]
        public async Task GetGameMetadataById()
        {
            // arrange
            var game = new GameMetadata { Id = "one", Name = "Test One" };
            var mockRepo = new Mock<IReadRepository<GameMetadata>>();
            mockRepo.Setup(p => p.GetById(It.IsAny<string>())).ReturnsAsync(game);

            var c = new GameMetadataController(mockRepo.Object, _logger);

            // act
            var result = await c.GetGameMetadataById("one");

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("one", result.Value.Id);
        }

        [TestMethod]
        public async Task GetGameMetadataById_MissingItem()
        {
            // arrange
            var mockRepo = new Mock<IReadRepository<GameMetadata>>();
            var c = new GameMetadataController(mockRepo.Object, _logger);

            // act
            var result = await c.GetGameMetadataById("foo");

            // assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Value);
        }
    }
}
