//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Logic;
using Blazor.Arcade.Service.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Service.UnitTests.Logic
{
    [TestClass]
    public class UserProfileActionManagerTests
    {
        private readonly IRepository<UserProfile> _defaultRepo = new Mock<IRepository<UserProfile>>().Object;
        private readonly IReadRepository<ServerMetadata> _serverRepo = new ServerMetadataRepository();
        private readonly UserProfile _userProfile = new()
        {
            Id = "test-profile-1",
            Name= "Test",
            UserId = "test-user-1"
        };

        [TestMethod]
        public async Task GetProfileForUserAsync()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(_userProfile);

            var mgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.GetProfileForUserAsync("test-profile-1", "test-user-1");

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Equals(_userProfile));
        }

        [TestMethod]
        public async Task GetProfilesForUserAsync()
        {
            // arrange
            var list = new List<UserProfile>
            {
                _userProfile,
                new UserProfile { Id = "test-profile-2", UserId = "test-user-1" },
                new UserProfile { Id = "test-profile-3", UserId = "test-user-1" }
            };
            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.GetPartitionItemsAsync(It.IsAny<string>()))
                    .ReturnsAsync(list);

            var mgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.GetProfilesForUserAsync("test-user-1");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.First().Equals(_userProfile));
            Assert.IsTrue(result.All(p => p.UserId == "test-user-1"));
        }

        [TestMethod]
        public async Task CreateProfileForUserAsync()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.CreateItemAsync(It.IsAny<UserProfile>()))
                    .ReturnsAsync(_userProfile);

            var mgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.CreateProfileForUserAsync(_userProfile);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id.StartsWith("001-"));
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        public async Task CreateProfileForUserAsync_WithServer()
        {
            // arrange
            _userProfile.Server = "s1";

            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.CreateItemAsync(It.IsAny<UserProfile>()))
                    .ReturnsAsync(_userProfile);

            var mgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.CreateProfileForUserAsync(_userProfile);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id.StartsWith("001-"));
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        public async Task UpdateProfileForUserAsync()
        {
            // arrange
            _userProfile.Server = "s1";

            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.UpdateItemAsync(It.IsAny<UserProfile>()))
                    .ReturnsAsync(_userProfile);

            var mgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.UpdateProfileForUserAsync(_userProfile);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test-profile-1", result.Id);
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        public async Task UpdateProfileForUserAsync_NoChange()
        {
            // arrange
            _userProfile.Server = "s1";

            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(_userProfile);
            mockRepo.Setup(x => x.UpdateItemAsync(It.IsAny<UserProfile>()))
                    .ReturnsAsync(_userProfile);

            var mgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.UpdateProfileForUserAsync(_userProfile);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test-profile-1", result.Id);
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
            mockRepo.Verify(
                o => o.UpdateItemAsync(It.IsAny<UserProfile>()),
                Times.Never);
        }

        [TestMethod]
        public async Task DeleteProfileForUserAsync()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.DeleteItemAsync(It.IsAny<string>(), It.IsAny<string>()));

            var mgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            // act
            await mgr.DeleteProfileForUserAsync("test-profile-1", "test-user-1");

            // assert
            mockRepo.Verify(
                o => o.DeleteItemAsync(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(FormatException))]
        public async Task ValidationError_Id()
        {
            // arrange
            _userProfile.Server = "s1";
            _userProfile.Id = string.Empty;

            var mgr = new UserProfileActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.UpdateProfileForUserAsync(_userProfile);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(FormatException))]
        public async Task ValidationError_UserId()
        {
            // arrange
            _userProfile.Server = "s1";
            _userProfile.UserId = string.Empty;

            var mgr = new UserProfileActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.UpdateProfileForUserAsync(_userProfile);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(FormatException))]
        public async Task ValidationError_Name()
        {
            // arrange
            _userProfile.Server = "s1";
            _userProfile.Name = string.Empty;

            var mgr = new UserProfileActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.UpdateProfileForUserAsync(_userProfile);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(FormatException))]
        public async Task ValidationError_Server()
        {
            // arrange
            _userProfile.Server = "err";

            var mgr = new UserProfileActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.UpdateProfileForUserAsync(_userProfile);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetProfilesForUserAsync_InvalidUserID()
        {
            // arrange
            var mgr = new UserProfileActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.GetProfilesForUserAsync("");

            // assert
        }
    }
}
