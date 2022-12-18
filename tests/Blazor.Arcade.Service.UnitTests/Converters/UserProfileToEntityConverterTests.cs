//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Converters;
using Blazor.Arcade.Service.Entities;

namespace Blazor.Arcade.Service.UnitTests.Converters
{
    [TestClass]
    public class UserProfileToEntityConverterTests
    {
        [TestMethod]
        public void ConvertToEntity()
        {
            // arrange
            var model = new UserProfile
            {
                Id = "test-profile-1",
                Server = "s1",
                Avatar = "test-av",
                Name = "Test User",
                Gender = "M",
                Exp = 5,
                UserId = "test-user-1"
            };
            var conv = new UserProfileToEntityConverter();

            // act
            var result = conv.ConvertToEntity(model);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model.Id, result.ProfileId);
            Assert.AreEqual(model.Id, result.Id);
            Assert.AreEqual(model.Server, result.ServerId);
            Assert.AreEqual(model.Avatar, result.AvatarUri);
            Assert.AreEqual(model.Name, result.DisplayName);
            Assert.AreEqual(model.Gender, result.Gender);
            Assert.AreEqual(model.Exp, result.Exp);
            Assert.AreEqual(model.UserId, result.UserId);
            Assert.AreEqual(model.UserId, result.PartitionId);
        }

        [TestMethod]
        public void ConvertToModel()
        {
            // arrange
            var entity = new UserProfileEntity
            {
                ProfileId = "test-profile-1",
                ServerId = "s1",
                AvatarUri = "test-av",
                DisplayName = "Test User",
                Exp = 5,
                UserId = "test-user-1",
                Timestamp = 42,
                ETag = "test-etag"
            };
            var conv = new UserProfileToEntityConverter();

            // act
            var result = conv.ConvertToModel(entity);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Id, result.Id);
            Assert.AreEqual(entity.ProfileId, result.Id);
            Assert.AreEqual(entity.ServerId, result.Server);
            Assert.AreEqual(entity.AvatarUri, result.Avatar);
            Assert.AreEqual(entity.DisplayName, result.Name);
            Assert.AreEqual(entity.Gender, result.Gender);
            Assert.AreEqual(entity.Exp, result.Exp);
            Assert.AreEqual(entity.UserId, result.UserId);
            Assert.AreEqual(entity.PartitionId, result.UserId);
        }
    }
}
