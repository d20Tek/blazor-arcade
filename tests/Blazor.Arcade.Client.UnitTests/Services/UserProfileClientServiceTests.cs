//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class UserProfileClientServiceTests
    {
        private readonly ILogger<UserProfileClientService> _logger =
            new Mock<ILogger<UserProfileClientService>>().Object;
        private readonly ILocalStorageService _storageService =
            new Mock<ILocalStorageService>().Object;
        private readonly ITypedHttpClient _nullClient =
            new Mock<ITypedHttpClient>().Object;


        [TestMethod]
        public async Task GetProfiles()
        {
            // arrange
            var responseContent = @"
            [
                { ""id"": ""test-profile-1"", ""name"": ""User1"", ""server"": ""s1"", ""userId"": ""test-user-1"" },
                { ""id"": ""test-profile-2"", ""userId"": ""test-user-1"" },
                { ""id"": ""test-profile-3"", ""userId"": ""test-user-1"" }
            ]";

            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            // act
            var results = await service.GetEntitiesAsync();

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
            Assert.IsInstanceOfType(results.First(), typeof(UserProfile));
            Assert.IsTrue(results.Any(p => p.Id == "test-profile-1"));
            Assert.IsTrue(results.Any(p => p.Id == "test-profile-3"));
            Assert.IsTrue(results.All(p => p.UserId == "test-user-1"));
        }

        [TestMethod]
        public async Task GetProfile()
        {
            // arrange
            var responseContent = @"
                { ""id"": ""test-profile-1"", ""name"": ""User1"", ""server"": ""s1"", ""userId"": ""test-user-1"" }
            ";

            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            // act
            var result = await service.GetEntityAsync("test-profile-1");

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserProfile));
            Assert.AreEqual("test-profile-1", result.Id);
            Assert.AreEqual("User1", result.Name);
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        public async Task GetProfile_WithMissingId()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient_StatusCodeError(HttpStatusCode.NotFound);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            // act
            var result = await service.GetEntityAsync("test-profile-1");

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateProfile()
        {
            // arrange
            var responseContent = @"
                { ""id"": ""test-profile-1"", ""name"": ""User1"", ""server"": ""s1"", ""userId"": ""test-user-1"" }
            ";

            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            var newProfile = new UserProfile
            {
                Name = "User1",
                Gender = "M",
                Avatar = "/test/av.png"
            };

            // act
            var result = await service.CreateEntityAsync(newProfile);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserProfile));
            Assert.AreEqual("test-profile-1", result.Id);
            Assert.AreEqual("User1", result.Name);
            Assert.AreEqual("s1", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityAlreadyExistsException))]
        public async Task CreateProfile_AlreadyExists()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient_StatusCodeError(HttpStatusCode.Conflict);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            var newProfile = new UserProfile
            {
                Id = "existing-profile",
                Name = "User1",
                Gender = "M",
                Avatar = "/test/av.png"
            };

            // act
            var result = await service.CreateEntityAsync(newProfile);

            // assert
        }

        [TestMethod]
        public async Task UpdateProfile()
        {
            // arrange
            var responseContent = @"
                { ""id"": ""test-profile-1"", ""name"": ""User1"", ""server"": ""s2"", ""userId"": ""test-user-1"" }
            ";

            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(responseContent);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            var profile = new UserProfile
            {
                Id = "test-profile-1",
                Name = "User1",
                Server = "s2",
                Gender = "M",
                Avatar = "/test/av.png",
                UserId = "test-user-1"
            };

            // act
            var result = await service.UpdateEntityAsync(profile.Id, profile);

            // assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserProfile));
            Assert.AreEqual("test-profile-1", result.Id);
            Assert.AreEqual("User1", result.Name);
            Assert.AreEqual("s2", result.Server);
            Assert.AreEqual("test-user-1", result.UserId);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task UpdateProfile_NotFound()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient_StatusCodeError(HttpStatusCode.NotFound);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            var profile = new UserProfile
            {
                Id = "existing-profile",
                Name = "User1",
                Gender = "M",
                Avatar = "/test/av.png"
            };

            // act
            _ = await service.UpdateEntityAsync(profile.Id, profile);

            // assert
        }

        [TestMethod]
        public async Task DeleteProfile()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient(string.Empty);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            // act
            await service.DeleteEntityAsync("test-profile-1");

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task DeleteProfile_NotFound()
        {
            // arrange
            var httpClient = MockHttpClientHelper.CreateTypedHttpClient_StatusCodeError(HttpStatusCode.NotFound);
            var service = new UserProfileClientService(httpClient, _storageService, _logger);

            // act
            await service.DeleteEntityAsync("test-profile-1");

            // assert
        }

        [TestMethod]
        public async Task HasCurrentProfileAsync()
        {
            // arrange
            var storage = new Mock<ILocalStorageService>();
            storage.Setup(x => x.ContainKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(true);

            var authState = CreateAuthenticatedUser();
            var service = new UserProfileClientService(_nullClient, storage.Object, _logger);

            // act
            var result = await service.HasCurrentProfileAsync(authState);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task HasCurrentProfileAsync_False()
        {
            // arrange
            var authState = CreateAuthenticatedUser();
            var service = new UserProfileClientService(_nullClient, _storageService, _logger);

            // act
            var result = await service.HasCurrentProfileAsync(authState);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task GetCurrentProfileAsync()
        {
            // arrange
            var profile = new UserProfile
            {
                Id = "test-profile-1",
                Name = "User1",
                Server = "s2",
                Gender = "M",
                Avatar = "/test/av.png",
                UserId = "test-user-1"
            };

            var storage = new Mock<ILocalStorageService>();
            storage.Setup(x => x.GetItemAsync<UserProfile>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(profile);

            var authState = CreateAuthenticatedUser();
            var service = new UserProfileClientService(_nullClient, storage.Object, _logger);

            // act
            var result = await service.GetCurrentProfileAsync(authState);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Equals(profile));
        }

        [TestMethod]
        public async Task GetCurrentProfileAsync_WithMissingKey()
        {
            var authState = CreateAuthenticatedUser();
            var service = new UserProfileClientService(_nullClient, _storageService, _logger);

            // act
            var result = await service.GetCurrentProfileAsync(authState);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.Id);
        }

        [TestMethod]
        public async Task SetCurrentProfileAsync()
        {
            // arrange
            var profile = new UserProfile
            {
                Id = "test-profile-1",
                Name = "User1",
                Server = "s2",
                Gender = "M",
                Avatar = "/test/av.png",
                UserId = "test-user-1"
            };

            var eventCalled = false;
            var storage = new Mock<ILocalStorageService>();
            storage.Setup(x => x.SetItemAsync<UserProfile>(
                It.IsAny<string>(), It.IsAny<UserProfile>(), It.IsAny<CancellationToken>()));

            var authState = CreateAuthenticatedUser();
            var service = new UserProfileClientService(_nullClient, storage.Object, _logger);
            service.OnCurrentProfileChanged += (p) => { eventCalled = true; };

            // act
            await service.SetCurrentProfileAsync(authState, profile);

            // assert
            storage.Verify(
                o => o.SetItemAsync<UserProfile>(It.IsAny<string>(), It.IsAny<UserProfile>(), It.IsAny<CancellationToken>()),
                Times.Once);
            Assert.IsTrue(eventCalled);
        }

        [TestMethod]
        public async Task SetCurrentProfileAsync_ClearProfile()
        {
            // arrange
            var storage = new Mock<ILocalStorageService>();

            var eventCalled = false;
            var authState = CreateAuthenticatedUser();
            var service = new UserProfileClientService(_nullClient, storage.Object, _logger);
            service.OnCurrentProfileChanged += (p) => { eventCalled = true; };

            // act
            await service.SetCurrentProfileAsync(authState, null);

            // assert
            storage.Verify(
                o => o.RemoveItemAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Once);
            Assert.IsTrue(eventCalled);
        }

        private Task<AuthenticationState> CreateAuthenticatedUser()
        {
            var principal = new ClaimsPrincipal(new MockIdentity());
            var authState = new AuthenticationState(principal);

            return Task.FromResult(authState);
        }
    }
}
