//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Service.Repositories;

namespace Blazor.Arcade.Service.UnitTests.Repositories
{
    [TestClass]
    public class GameMetadataRepositoryTests
    {
        [TestMethod]
        public async Task GetAll()
        {
            // arrange
            var repo = new GameMetadataRepository();

            // act
            var metadata = await repo.GetAll();

            // assert
            Assert.IsNotNull(metadata);
            Assert.AreEqual(2, metadata.Count);
            Assert.IsTrue(metadata.Any(p => p.Id == "arcade.game.tictactoe"));
            Assert.IsTrue(metadata.Any(p => p.Id == "arcade.game.rockpaperscissors"));
        }

        [TestMethod]
        public async Task GetById()
        {
            // arrange
            var repo = new GameMetadataRepository();

            // act
            var metadata = await repo.GetById("arcade.game.tictactoe");

            // assert
            Assert.IsNotNull(metadata);
            if (metadata != null)
            {
                Assert.AreEqual("arcade.game.tictactoe", metadata.Id);
                Assert.AreEqual("Tic-Tac-Toe", metadata.Name);
                Assert.AreNotEqual(string.Empty, metadata.Description);
                Assert.AreEqual(1, metadata.NumPlayers.Min);
                Assert.AreEqual(2, metadata.NumPlayers.Max);
                Assert.AreEqual(1, metadata.Duration.Min);
                Assert.AreEqual(1, metadata.Duration.Max);
                Assert.AreEqual(1, metadata.Complexity);
                Assert.AreEqual(3, metadata.Tags.Count);
                Assert.AreEqual(100, metadata.SortOrder);
                Assert.AreEqual("/games/tic-tac-toe", metadata.Locations.GameUrl);
                Assert.AreEqual("/games/tic-tac-toe/lobby", metadata.Locations.GameLobbyUrl);
                Assert.AreEqual("/images/games/tic-tac-toe/icon.png", metadata.Locations.IconUrl);
                Assert.AreEqual("/images/games/tic-tac-toe/large-logo.png", metadata.Locations.LargeLogoUrl);
                Assert.AreEqual("/images/games/tic-tac-toe/banner-background.png", metadata.Locations.BannerImageUrl);
            }
        }
    }
}
