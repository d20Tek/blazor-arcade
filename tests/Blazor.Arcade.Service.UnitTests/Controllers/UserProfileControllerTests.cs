//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Controllers;
using Blazor.Arcade.Service.Logic;
using Blazor.Arcade.Service.Repositories;
using Blazor.Arcade.Service.UnitTests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blazor.Arcade.Service.UnitTests.Controllers
{
    [TestClass]
    public class UserProfileControllerTests
    {
        private readonly ILogger<UserProfileController> _logger = new Mock<ILogger<UserProfileController>>().Object;
        private readonly IReadRepository<ServerMetadata> _serverRepo = new ServerMetadataRepository();
        private readonly UserProfile _userProfile = new()
        {
            Id = "test-profile-1",
            Name = "Test",
            UserId = "e14e5bec-8700-4be5-9e7B-14fae1b2ba82"
        };

        [TestMethod]
        public async Task GetProfiles()
        {
            // arrange
            var list = new List<UserProfile>
            {
                _userProfile,
                new UserProfile { Id = "test-profile-2", UserId = "e14e5bec-8700-4be5-9e7B-14fae1b2ba82" },
                new UserProfile { Id = "test-profile-3", UserId = "e14e5bec-8700-4be5-9e7B-14fae1b2ba82" }
            };
            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.GetPartitionItemsAsync(It.IsAny<string>()))
                    .ReturnsAsync(list);

            var profileMgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserProfileController(profileMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.GetProfiles();

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            if (result.Value != null)
            {
                Assert.AreEqual(3, result.Value.Count);
                Assert.IsTrue(result.Value.First().Equals(_userProfile));
                Assert.IsTrue(result.Value.All(p => p.UserId == "e14e5bec-8700-4be5-9e7B-14fae1b2ba82"));
            }
        }

        [TestMethod]
        public async Task GetProfilesById()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(_userProfile);

            var profileMgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserProfileController(profileMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.GetProfileById("test-profile-1");

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            if (result.Value != null)
            {
                Assert.AreEqual("test-profile-1", result.Value.Id);
                Assert.AreEqual("e14e5bec-8700-4be5-9e7B-14fae1b2ba82", result.Value.UserId);
            }
        }

        [TestMethod]
        public async Task CreateProfile()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.CreateItemAsync(It.IsAny<UserProfile>()))
                    .ReturnsAsync(_userProfile);

            var profileMgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserProfileController(profileMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.CreateProfile(_userProfile);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            if (result.Value != null)
            {
                Assert.IsTrue(result.Value.Id.StartsWith("001-"));
                Assert.AreEqual("s1", result.Value.Server);
                Assert.AreEqual("e14e5bec-8700-4be5-9e7B-14fae1b2ba82", result.Value.UserId);
            }
        }

        [TestMethod]
        public async Task UpdateProfile()
        {
            // arrange
            _userProfile.Server = "s1";

            var mockRepo = new Mock<IRepository<UserProfile>>();
            mockRepo.Setup(x => x.UpdateItemAsync(It.IsAny<UserProfile>()))
                    .ReturnsAsync(_userProfile);

            var profileMgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserProfileController(profileMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.UpdateProfile("test-profile-1", _userProfile);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            if (result.Value != null)
            {
                Assert.AreEqual("test-profile-1", result.Value.Id);
                Assert.AreEqual("s1", result.Value.Server);
                Assert.AreEqual("e14e5bec-8700-4be5-9e7B-14fae1b2ba82", result.Value.UserId);
            }
        }

        [TestMethod]
        public async Task DeleteProfile()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserProfile>>();
            var profileMgr = new UserProfileActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserProfileController(profileMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.DeleteProfile("test-profile-1");

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            if (result.Result != null)
            {
                Assert.AreEqual(
                    StatusCodes.Status204NoContent,
                    ((StatusCodeResult)result.Result).StatusCode);
            }
        }
    }
}
