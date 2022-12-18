//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure;
using Azure.Cosmos;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Entities;
using Blazor.Arcade.Service.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Blazor.Arcade.Service.UnitTests.Repositories
{
    [TestClass]
    public class UserProfileRepositoryTests
    {
        private readonly ILogger<UserProfileRepository> _logger = new Mock<ILogger<UserProfileRepository>>().Object;
        private readonly Mock<ICacheService> _defaultCache = new Mock<ICacheService>();

        [TestMethod]
        public async Task GetItemAsync()
        {
            // arrange
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            var result = await repo.GetItemAsync("foo", "bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
        }

        [TestMethod]
        public async Task GetItemAsync_NoPartitionId()
        {
            // arrange
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            var result = await repo.GetItemAsync("foo");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task GetItemAsync_NotFound()
        {
            // arrange
            var client = CreateMockClient();
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            _ = await repo.GetItemAsync("test-id-9", "user-id-1");

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetItemAsync_OtherCosmosException()
        {
            // arrange
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.ReadItemAsync<UserProfileEntity>(
                            It.IsAny<string>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Forbidden));

            var client = CreateMockClient(mockContainer);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            _ = await repo.GetItemAsync("test-id-err", "user-id-1");

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetItemAsync_Exception()
        {
            // arrange
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.ReadItemAsync<UserProfileEntity>(
                            It.IsAny<string>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new InvalidOperationException());

            var client = CreateMockClient(mockContainer);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            _ = await repo.GetItemAsync("test-id-err", "user-id-1");

            // assert
        }

        [TestMethod]
        public async Task GetItemAsync_FromCache()
        {
            // arrange
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var mockCache = new Mock<ICacheService>();
            mockCache.Setup(x => x.Contains<UserProfileEntity>("foo")).Returns(true);
            mockCache.Setup(x => x.Get<UserProfileEntity>("foo"))
                     .Returns(expected);

            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, mockCache.Object, _logger);

            // act
            var result = await repo.GetItemAsync("foo", "bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
            mockCache.Verify(o => o.Get<UserProfileEntity>("foo"), Times.Once);
        }

        [TestMethod]
        public async Task GetItemAsync_SetToCache()
        {
            // arrange
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, _defaultCache.Object, _logger);

            // act
            var result = await repo.GetItemAsync("foo", "bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
            _defaultCache.Verify(
                o => o.Set<UserProfileEntity>(It.IsAny<string>(), It.IsAny<UserProfileEntity>()),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteItemAsync()
        {
            // arrange
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            await repo.DeleteItemAsync("foo", "bar");

            // assert
        }

        [TestMethod]
        public async Task DeleteItemAsync_WithCache()
        {
            // arrange
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, _defaultCache.Object, _logger);

            // act
            await repo.DeleteItemAsync("foo", "bar");

            // assert
            _defaultCache.Verify(
                o => o.Remove<UserProfileEntity>(It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task DeleteItemAsync_NotFound()
        {
            // arrange
            var client = CreateMockClient();
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            await repo.DeleteItemAsync("test-id-9", "user-id-1");

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task DeleteItemAsync_Exception()
        {
            // arrange
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.DeleteItemAsync<UserProfileEntity>(
                            It.IsAny<string>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Forbidden));

            var client = CreateMockClient(mockContainer);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            await repo.DeleteItemAsync("test-id-err", "user-id-1");

            // assert
        }

        [TestMethod]
        public async Task CreateItemAsync()
        {
            // arrange
            var model = new UserProfile { Id = "foo", UserId = "bar" };
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            var result = await repo.CreateItemAsync(model);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
        }

        [TestMethod]
        public async Task CreateItemAsync_WithCache()
        {
            // arrange
            var model = new UserProfile { Id = "foo", UserId = "bar" };
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, _defaultCache.Object, _logger);

            // act
            var result = await repo.CreateItemAsync(model);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
            _defaultCache.Verify(
                o => o.Set<UserProfileEntity>(It.IsAny<string>(), It.IsAny<UserProfileEntity>()),
                Times.Once);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityAlreadyExistsException))]
        public async Task CreateItemAsync_AlreadyExists()
        {
            // arrange
            var model = new UserProfile { Id = "test-id-9", UserId = "user-id-1" };
            var client = CreateMockClient();
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            await repo.CreateItemAsync(model);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task CreateItemAsync_Exception()
        {
            // arrange
            var model = new UserProfile { Id = "test-id-9", UserId = "user-id-1" };
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.CreateItemAsync<UserProfileEntity>(
                            It.IsAny<UserProfileEntity>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Forbidden));

            var client = CreateMockClient(mockContainer);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            await repo.CreateItemAsync(model);

            // assert
        }

        [TestMethod]
        public async Task UpdateItemAsync()
        {
            // arrange
            var model = new UserProfile { Id = "foo2", UserId = "bar" };
            var expected = new UserProfileEntity { ProfileId = "foo2", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            var result = await repo.UpdateItemAsync(model);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo2", result.Id);
            Assert.AreEqual("bar", result.UserId);
        }

        [TestMethod]
        public async Task UpdateItemAsync_WithCache()
        {
            // arrange
            var model = new UserProfile { Id = "foo2", UserId = "bar" };
            var expected = new UserProfileEntity { ProfileId = "foo2", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, _defaultCache.Object, _logger);

            // act
            var result = await repo.UpdateItemAsync(model);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo2", result.Id);
            Assert.AreEqual("bar", result.UserId);
            _defaultCache.Verify(
                o => o.Set<UserProfileEntity>(It.IsAny<string>(), It.IsAny<UserProfileEntity>()),
                Times.Once);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task UpdateItemAsync_AlreadyExists()
        {
            // arrange
            var model = new UserProfile { Id = "test-id-9", UserId = "user-id-1" };
            var client = CreateMockClient();
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            await repo.UpdateItemAsync(model);

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task UpdateItemAsync_Exception()
        {
            // arrange
            var model = new UserProfile { Id = "test-id-9", UserId = "user-id-1" };
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.ReplaceItemAsync<UserProfileEntity>(
                            It.IsAny<UserProfileEntity>(),
                            It.IsAny<string>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Forbidden));

            var client = CreateMockClient(mockContainer);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            await repo.UpdateItemAsync(model);

            // assert
        }

        [TestMethod]
        public async Task GetItemsAsync()
        {
            // arrange
            var expected = new UserProfileEntity { ProfileId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            var result = await repo.GetItemsAsync(new List<string> { "foo", "foo2" }, "bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetPartitionItemsAsync()
        {
            // arrange
            var expected = new List<UserProfileEntity>
            {
                new UserProfileEntity { ProfileId = "foo", UserId = "bar" },
                new UserProfileEntity { ProfileId = "foo2", UserId = "bar" },
            };

            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, null, _logger);

            // act
            var result = await repo.GetPartitionItemsAsync("bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetPartitionItemsAsync_FromCache()
        {
            // arrange
            var expected = new List<UserProfileEntity>
            {
                new UserProfileEntity { ProfileId = "foo", UserId = "bar" },
                new UserProfileEntity { ProfileId = "foo2", UserId = "bar" },
            };
            var mockCache = new Mock<ICacheService>();
            mockCache.Setup(x => x.ContainsList<UserProfileEntity>("bar")).Returns(true);
            mockCache.Setup(x => x.GetList<UserProfileEntity>("bar"))
                     .Returns(expected);

            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, mockCache.Object, _logger);

            // act
            var result = await repo.GetPartitionItemsAsync("bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            mockCache.Verify(o => o.GetList<UserProfileEntity>("bar"), Times.Once);
        }

        [TestMethod]
        public async Task GetPartitionItemsAsync_SetListCache()
        {
            // arrange
            var expected = new List<UserProfileEntity>
            {
                new UserProfileEntity { ProfileId = "foo", UserId = "bar" },
                new UserProfileEntity { ProfileId = "foo2", UserId = "bar" },
            };

            var client = CreateMockClient(expected);
            var repo = new UserProfileRepository(client, _defaultCache.Object, _logger);

            // act
            var result = await repo.GetPartitionItemsAsync("bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            _defaultCache.Verify(
                o => o.SetList<UserProfileEntity>(It.IsAny<string>(), It.IsAny<IList<UserProfileEntity>>()),
                Times.Once);
        }

        private CosmosClient CreateMockClient(UserProfileEntity? expectedItem = null)
        {
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.ReadItemAsync<UserProfileEntity>(
                            "test-id-9",
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.NotFound));

            if (expectedItem != null)
            {
                mockContainer.Setup(x => x.ReadItemAsync<UserProfileEntity>(
                                It.IsAny<string>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(CreateMockResponse(expectedItem));
            }

            mockContainer.Setup(x => x.DeleteItemAsync<UserProfileEntity>(
                            "test-id-9",
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.NotFound));

            if (expectedItem != null)
            {
                mockContainer.Setup(x => x.DeleteItemAsync<UserProfileEntity>(
                                expectedItem.Id,
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(CreateMockResponse(expectedItem));
            }

            if (expectedItem != null)
            {
                mockContainer.Setup(x => x.CreateItemAsync<UserProfileEntity>(
                                It.IsAny<UserProfileEntity>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(CreateMockResponse(expectedItem));
            }
            else
            {
                mockContainer.Setup(x => x.CreateItemAsync<UserProfileEntity>(
                                It.IsAny<UserProfileEntity>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Conflict));
            }

            if (expectedItem != null)
            {
                mockContainer.Setup(x => x.ReplaceItemAsync<UserProfileEntity>(
                                It.IsAny<UserProfileEntity>(),
                                It.IsAny<string>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(CreateMockResponse(expectedItem));
            }
            else
            {
                mockContainer.Setup(x => x.ReplaceItemAsync<UserProfileEntity>(
                                It.IsAny<UserProfileEntity>(),
                                It.IsAny<string>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.NotFound));
            }

            return CreateMockClient(mockContainer);
        }

        private CosmosClient CreateMockClient(IList<UserProfileEntity> expectedItems)
        {
            var pages = new List<Page<UserProfileEntity>>
            {
                Page<UserProfileEntity>.FromValues(expectedItems.ToArray(), null, new Mock<Response>().Object),
            };

            var results = AsyncPageable<UserProfileEntity>.FromPages(pages);
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.GetItemQueryIterator<UserProfileEntity>(
                            It.IsAny<QueryDefinition>(),
                            It.IsAny<string>(),
                            It.IsAny<QueryRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .Returns(results);

            return CreateMockClient(mockContainer);
        }

        private CosmosClient CreateMockClient(Mock<CosmosContainer> mockContainer)
        {
            var mockDbResponse = new Mock<DatabaseResponse>();
            mockDbResponse.Setup(x => x.Database).Returns(new Mock<CosmosDatabase>().Object);

            var mockClient = new Mock<CosmosClient>();
            mockClient.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
                      .Returns(mockContainer.Object);
            mockClient.Setup(x => x.CreateDatabaseIfNotExistsAsync(It.IsAny<string>(), null, null, default))
                      .ReturnsAsync(mockDbResponse.Object);

            return mockClient.Object;
        }

        private ItemResponse<T> CreateMockResponse<T>(T value)
        {
            var mockResponse = new Mock<ItemResponse<T>>();
            mockResponse.Setup(x => x.Value).Returns(value);

            return mockResponse.Object;
        }
    }
}
