//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Converters;
using Blazor.Arcade.Service.Entities;

namespace Blazor.Arcade.Service.UnitTests.Converters
{
    [TestClass]
    public class GameSessionToEntityConverterTests
    {
        [TestMethod]
        public void ConvertToEntity()
        {
            // arrange
            var model = new GameSession
            {
                Id = "test-session-1",
                ServerId = "s1",
                Name = "Test Session",
                MetadataId = "arcade.games.tictactoe",
                Phase = 1,
                GameState = new Dictionary<string, object> { { "test-key", "test-value" } },
                HostId = "test-profile-1",
            };

            var conv = new GameSessionToEntityConverter();

            // act
            var result = conv.ConvertToEntity(model);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model.Id, result.SessionId);
            Assert.AreEqual(model.Id, result.Id);
            Assert.AreEqual(model.ServerId, result.ServerId);
            Assert.AreEqual(model.Name, result.Name);
            Assert.AreEqual(model.MetadataId, result.MetadataId);
            Assert.AreEqual(model.Phase, result.Phase);
            Assert.AreEqual(model.GameState, result.GameState);
            Assert.AreEqual(model.HostId, result.HostId);
            Assert.AreEqual(model.ServerId, result.PartitionId);
        }

        [TestMethod]
        public void ConvertToModel()
        {
            // arrange
            var entity = new GameSessionEntity
            {
                SessionId = "test-session-1",
                ServerId = "s1",
                Name = "Test Session",
                MetadataId = "arcade.games.tictactoe",
                Phase = 1,
                GameState = new Dictionary<string, object> { { "test-key", "test-value" } },
                HostId = "test-profile-1",
                Timestamp = 42,
                ETag = "test-etag"
            };
            var conv = new GameSessionToEntityConverter();

            // act
            var result = conv.ConvertToModel(entity);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Id, result.Id);
            Assert.AreEqual(entity.SessionId, result.Id);
            Assert.AreEqual(entity.ServerId, result.ServerId);
            Assert.AreEqual(entity.Name, result.Name);
            Assert.AreEqual(entity.MetadataId, result.MetadataId);
            Assert.AreEqual(entity.Phase, result.Phase);
            Assert.AreEqual(entity.GameState, result.GameState);
            Assert.AreEqual(entity.HostId, result.HostId);
            Assert.AreEqual(entity.PartitionId, result.ServerId);
        }
    }
}
