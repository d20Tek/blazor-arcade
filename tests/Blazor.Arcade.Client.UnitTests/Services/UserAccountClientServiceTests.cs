//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class UserAccountClientServiceTests
    {
        private readonly ILogger<UserAccountClientService> _logger =
            new Mock<ILogger<UserAccountClientService>>().Object;

        [TestMethod]
        public async Task GetAccounts()
        {
            // arrange
            var responseContent = @"
            [
                { ""id"": ""test-account-1"", ""name"": ""User1"", ""server"": ""s1"", ""userId"": ""test-user-1"" },
                { ""id"": ""test-account-2"", ""userId"": ""test-user-1"" },
                { ""id"": ""test-account-3"", ""userId"": ""test-user-1"" }
            ]";

            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new UserAccountClientService(httpClient, _logger);

            // act
            var results = await service.GetEntitiesAsync();

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
            Assert.IsInstanceOfType(results.First(), typeof(UserAccount));
            Assert.IsTrue(results.Any(p => p.Id == "test-account-1"));
            Assert.IsTrue(results.Any(p => p.Id == "test-account-3"));
            Assert.IsTrue(results.All(p => p.UserId == "test-user-1"));
        }

        [TestMethod]
        public async Task GetAccount()
        {
            // arrange
            var responseContent = @"
                { ""id"": ""test-account-1"", ""name"": ""User1"", ""server"": ""s1"", ""userId"": ""test-user-1"" }
            ";

            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new UserAccountClientService(httpClient, _logger);

            // act
            var result = await service.GetEntityAsync("test-account-1");

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserAccount));
            Assert.AreEqual("test-account-1", result.Id);
            Assert.AreEqual("User1", result.Name);
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        public async Task CreateAccount()
        {
            // arrange
            var responseContent = @"
                { ""id"": ""test-account-1"", ""name"": ""User1"", ""server"": ""s1"", ""userId"": ""test-user-1"" }
            ";

            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new UserAccountClientService(httpClient, _logger);

            var newAccount = new UserAccount
            {
                Name = "User1",
                Gender = "M",
                Avatar = "/test/av.png"
            };

            // act
            var result = await service.CreateEntityAsync(newAccount);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserAccount));
            Assert.AreEqual("test-account-1", result.Id);
            Assert.AreEqual("User1", result.Name);
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityAlreadyExistsException))]
        public async Task CreateAccount_AlreadyExists()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient_StatusCodeError(HttpStatusCode.Conflict);
            var service = new UserAccountClientService(httpClient, _logger);

            var newAccount = new UserAccount
            {
                Id = "existing-account",
                Name = "User1",
                Gender = "M",
                Avatar = "/test/av.png"
            };

            // act
            var result = await service.CreateEntityAsync(newAccount);

            // assert
        }

        [TestMethod]
        public async Task UpdateAccount()
        {
            // arrange
            var responseContent = @"
                { ""id"": ""test-account-1"", ""name"": ""User1"", ""server"": ""s2"", ""userId"": ""test-user-1"" }
            ";

            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new UserAccountClientService(httpClient, _logger);

            var account = new UserAccount
            {
                Id = "test-account-1",
                Name = "User1",
                Server = "s2",
                Gender = "M",
                Avatar = "/test/av.png",
                UserId = "test-user-1"
            };

            // act
            var result = await service.UpdateEntityAsync(account.Id, account);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserAccount));
            Assert.AreEqual("test-account-1", result.Id);
            Assert.AreEqual("User1", result.Name);
            Assert.AreEqual("s2", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task UpdateAccount_NotFound()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient_StatusCodeError(HttpStatusCode.NotFound);
            var service = new UserAccountClientService(httpClient, _logger);

            var account = new UserAccount
            {
                Id = "existing-account",
                Name = "User1",
                Gender = "M",
                Avatar = "/test/av.png"
            };

            // act
            _ = await service.UpdateEntityAsync(account.Id, account);

            // assert
        }

        [TestMethod]
        public async Task DeleteAccount()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(string.Empty);
            var service = new UserAccountClientService(httpClient, _logger);

            // act
            await service.DeleteEntityAsync("test-account-1");

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task DeleteAccount_NotFound()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient_StatusCodeError(HttpStatusCode.NotFound);
            var service = new UserAccountClientService(httpClient, _logger);

            // act
            await service.DeleteEntityAsync("test-account-1");

            // assert
        }
    }
}
