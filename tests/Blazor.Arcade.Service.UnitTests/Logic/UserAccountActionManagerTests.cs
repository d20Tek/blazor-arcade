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
    public class UserAccountActionManagerTests
    {
        private readonly IRepository<UserAccount> _defaultRepo = new Mock<IRepository<UserAccount>>().Object;
        private readonly IReadRepository<ServerMetadata> _serverRepo = new ServerMetadataRepository();
        private readonly UserAccount _userAccount = new()
        {
            Id = "test-account-1",
            Name= "Test",
            UserId = "test-user-1"
        };

        [TestMethod]
        public async Task GetAccountForUserAsync()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(_userAccount);

            var mgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.GetAccountForUserAsync("test-account-1", "test-user-1");

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Equals(_userAccount));
        }

        [TestMethod]
        public async Task GetAccountsForUserAsync()
        {
            // arrange
            var list = new List<UserAccount>
            {
                _userAccount,
                new UserAccount { Id = "test-account-2", UserId = "test-user-1" },
                new UserAccount { Id = "test-account-3", UserId = "test-user-1" }
            };
            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.GetPartitionItemsAsync(It.IsAny<string>()))
                    .ReturnsAsync(list);

            var mgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.GetAccountsForUserAsync("test-user-1");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.First().Equals(_userAccount));
            Assert.IsTrue(result.All(p => p.UserId == "test-user-1"));
        }

        [TestMethod]
        public async Task CreateAccountForUserAsync()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.CreateItemAsync(It.IsAny<UserAccount>()))
                    .ReturnsAsync(_userAccount);

            var mgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.CreateAccountForUserAsync(_userAccount);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id.StartsWith("001-"));
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        public async Task CreateAccountForUserAsync_WithServer()
        {
            // arrange
            _userAccount.Server = "s1";

            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.CreateItemAsync(It.IsAny<UserAccount>()))
                    .ReturnsAsync(_userAccount);

            var mgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.CreateAccountForUserAsync(_userAccount);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id.StartsWith("001-"));
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        public async Task UpdateAccountForUserAsync()
        {
            // arrange
            _userAccount.Server = "s1";

            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.UpdateItemAsync(It.IsAny<UserAccount>()))
                    .ReturnsAsync(_userAccount);

            var mgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.UpdateAccountForUserAsync(_userAccount);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test-account-1", result.Id);
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        public async Task UpdateAccountForUserAsync_NoChange()
        {
            // arrange
            _userAccount.Server = "s1";

            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(_userAccount);
            mockRepo.Setup(x => x.UpdateItemAsync(It.IsAny<UserAccount>()))
                    .ReturnsAsync(_userAccount);

            var mgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            // act
            var result = await mgr.UpdateAccountForUserAsync(_userAccount);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("test-account-1", result.Id);
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
            mockRepo.Verify(
                o => o.UpdateItemAsync(It.IsAny<UserAccount>()),
                Times.Never);
        }

        [TestMethod]
        public async Task DeleteAccountForUserAsync()
        {
            // arrange
            var mockRepo = new Mock<IRepository<UserAccount>>();
            mockRepo.Setup(x => x.DeleteItemAsync(It.IsAny<string>(), It.IsAny<string>()));

            var mgr = new UserAccountActionManager(mockRepo.Object, _serverRepo);

            // act
            await mgr.DeleteAccountForUserAsync("test-account-1", "test-user-1");

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
            _userAccount.Server = "s1";
            _userAccount.Id = string.Empty;

            var mgr = new UserAccountActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.UpdateAccountForUserAsync(_userAccount);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(FormatException))]
        public async Task ValidationError_UserId()
        {
            // arrange
            _userAccount.Server = "s1";
            _userAccount.UserId = string.Empty;

            var mgr = new UserAccountActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.UpdateAccountForUserAsync(_userAccount);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(FormatException))]
        public async Task ValidationError_Name()
        {
            // arrange
            _userAccount.Server = "s1";
            _userAccount.Name = string.Empty;

            var mgr = new UserAccountActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.UpdateAccountForUserAsync(_userAccount);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(FormatException))]
        public async Task ValidationError_Server()
        {
            // arrange
            _userAccount.Server = "err";

            var mgr = new UserAccountActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.UpdateAccountForUserAsync(_userAccount);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetAccountsForUserAsync_InvalidUserID()
        {
            // arrange
            var mgr = new UserAccountActionManager(_defaultRepo, _serverRepo);

            // act
            _ = await mgr.GetAccountsForUserAsync("");

            // assert
        }
    }
}
