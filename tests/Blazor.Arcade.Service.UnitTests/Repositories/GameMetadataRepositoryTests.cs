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
            Assert.AreEqual("arcade.game.tictactoe", metadata.Id);
            Assert.AreEqual("Tic-Tac-Toe", metadata.Name);
            Assert.AreEqual(1, metadata.NumPlayers.Min);
            Assert.AreEqual(2, metadata.NumPlayers.Max);
            Assert.AreEqual(1, metadata.Duration.Min);
            Assert.AreEqual(1, metadata.Duration.Max);
            Assert.AreEqual(100, metadata.SortOrder);
            Assert.AreEqual("/games/tic-tac-toe", metadata.Locations.GameUrl);
            Assert.AreEqual("/images/games/ttt/icon.png", metadata.Locations.IconUrl);
        }
    }
}
