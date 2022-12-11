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
    public class UserAccountControllerTests
    {
        private readonly ILogger<UserAccountController> _logger = new Mock<ILogger<UserAccountController>>().Object;
        private readonly IReadRepository<ServerMetadata> _serverRepo = new ServerMetadataRepository();
        private readonly UserAccount _userAccount = new()
        {
            Id = "test-account-1",
            Name = "Test",
            UserId = "e14e5bec-8700-4be5-9e7B-14fae1b2ba82"
        };

        [TestMethod]
        public async Task GetAccounts()
        {
            // arrange
            var list = new List<UserAccount>
            {
                _userAccount,
                new UserAccount { Id = "test-account-2", UserId = "e14e5bec-8700-4be5-9e7B-14fae1b2ba82" },
                new UserAccount { Id = "test-account-3", UserId = "e14e5bec-8700-4be5-9e7B-14fae1b2ba82" }
            };
            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.GetPartitionItemsAsync(It.IsAny<string>()))
                    .ReturnsAsync(list);

            var accountMgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserAccountController(accountMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.GetAccounts();

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(3, result.Value.Count);
            Assert.IsTrue(result.Value.First().Equals(_userAccount));
            Assert.IsTrue(result.Value.All(p => p.UserId == "e14e5bec-8700-4be5-9e7B-14fae1b2ba82"));
        }

        [TestMethod]
        public async Task GetAccountById()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(_userAccount);

            var accountMgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserAccountController(accountMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.GetAccountById("test-account-1");

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("test-account-1", result.Value.Id);
            Assert.AreEqual("e14e5bec-8700-4be5-9e7B-14fae1b2ba82", result.Value.UserId);
        }

        [TestMethod]
        public async Task CreateAccount()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.CreateItemAsync(It.IsAny<UserAccount>()))
                    .ReturnsAsync(_userAccount);

            var accountMgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserAccountController(accountMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.CreateAccount(_userAccount);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.IsTrue(result.Value.Id.StartsWith("001-"));
            Assert.AreEqual("s1", result.Value.Server);
            Assert.AreEqual("e14e5bec-8700-4be5-9e7B-14fae1b2ba82", result.Value.UserId);
        }

        [TestMethod]
        public async Task UpdateAccount()
        {
            // arrange
            _userAccount.Server = "s1";

            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.UpdateItemAsync(It.IsAny<UserAccount>()))
                    .ReturnsAsync(_userAccount);

            var accountMgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserAccountController(accountMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.UpdateAccount("test-account-1", _userAccount);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("test-account-1", result.Value.Id);
            Assert.AreEqual("s1", result.Value.Server);
            Assert.AreEqual("e14e5bec-8700-4be5-9e7B-14fae1b2ba82", result.Value.UserId);
        }

        [TestMethod]
        public async Task DeleteAccount()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserAccount>>();
            var accountMgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            var controller = new UserAccountController(accountMgr, _logger)
            {
                ControllerContext = ControllerContextHelper.CreateContextWithIdentityPrincipal(),
            };

            // act
            var result = await controller.DeleteAccount("test-account-1");

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(
                StatusCodes.Status204NoContent,
                ((StatusCodeResult)result.Result).StatusCode);
        }
    }
}
