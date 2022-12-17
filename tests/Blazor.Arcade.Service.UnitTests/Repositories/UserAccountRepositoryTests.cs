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
    public class UserAccountRepositoryTests
    {
        private readonly ILogger<UserAccountRepository> _logger = new Mock<ILogger<UserAccountRepository>>().Object;
        private readonly Mock<ICacheService> _defaultCache = new Mock<ICacheService>();

        [TestMethod]
        public async Task GetItemAsync()
        {
            // arrange
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, null, _logger);

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
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, null, _logger);

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
            var repo = new UserAccountRepository(client, null, _logger);

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
            mockContainer.Setup(x => x.ReadItemAsync<UserAccountEntity>(
                            It.IsAny<string>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Forbidden));

            var client = CreateMockClient(mockContainer);
            var repo = new UserAccountRepository(client, null, _logger);

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
            mockContainer.Setup(x => x.ReadItemAsync<UserAccountEntity>(
                            It.IsAny<string>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new InvalidOperationException());

            var client = CreateMockClient(mockContainer);
            var repo = new UserAccountRepository(client, null, _logger);

            // act
            _ = await repo.GetItemAsync("test-id-err", "user-id-1");

            // assert
        }

        [TestMethod]
        public async Task GetItemAsync_FromCache()
        {
            // arrange
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var mockCache = new Mock<ICacheService>();
            mockCache.Setup(x => x.Contains<UserAccountEntity>("foo")).Returns(true);
            mockCache.Setup(x => x.Get<UserAccountEntity>("foo"))
                     .Returns(expected);

            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, mockCache.Object, _logger);

            // act
            var result = await repo.GetItemAsync("foo", "bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
            mockCache.Verify(o => o.Get<UserAccountEntity>("foo"), Times.Once);
        }

        [TestMethod]
        public async Task GetItemAsync_SetToCache()
        {
            // arrange
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, _defaultCache.Object, _logger);

            // act
            var result = await repo.GetItemAsync("foo", "bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
            _defaultCache.Verify(
                o => o.Set<UserAccountEntity>(It.IsAny<string>(), It.IsAny<UserAccountEntity>()),
                Times.Once);
        }

        [TestMethod]
        public async Task DeleteItemAsync()
        {
            // arrange
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, null, _logger);

            // act
            await repo.DeleteItemAsync("foo", "bar");

            // assert
        }

        [TestMethod]
        public async Task DeleteItemAsync_WithCache()
        {
            // arrange
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, _defaultCache.Object, _logger);

            // act
            await repo.DeleteItemAsync("foo", "bar");

            // assert
            _defaultCache.Verify(
                o => o.Remove<UserAccountEntity>(It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task DeleteItemAsync_NotFound()
        {
            // arrange
            var client = CreateMockClient();
            var repo = new UserAccountRepository(client, null, _logger);

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
            mockContainer.Setup(x => x.DeleteItemAsync<UserAccountEntity>(
                            It.IsAny<string>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Forbidden));

            var client = CreateMockClient(mockContainer);
            var repo = new UserAccountRepository(client, null, _logger);

            // act
            await repo.DeleteItemAsync("test-id-err", "user-id-1");

            // assert
        }

        [TestMethod]
        public async Task CreateItemAsync()
        {
            // arrange
            var model = new UserProfile { Id = "foo", UserId = "bar" };
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, null, _logger);

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
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, _defaultCache.Object, _logger);

            // act
            var result = await repo.CreateItemAsync(model);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Id);
            Assert.AreEqual("bar", result.UserId);
            _defaultCache.Verify(
                o => o.Set<UserAccountEntity>(It.IsAny<string>(), It.IsAny<UserAccountEntity>()),
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
            var repo = new UserAccountRepository(client, null, _logger);

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
            mockContainer.Setup(x => x.CreateItemAsync<UserAccountEntity>(
                            It.IsAny<UserAccountEntity>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Forbidden));

            var client = CreateMockClient(mockContainer);
            var repo = new UserAccountRepository(client, null, _logger);

            // act
            await repo.CreateItemAsync(model);

            // assert
        }

        [TestMethod]
        public async Task UpdateItemAsync()
        {
            // arrange
            var model = new UserProfile { Id = "foo2", UserId = "bar" };
            var expected = new UserAccountEntity { AccountId = "foo2", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, null, _logger);

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
            var expected = new UserAccountEntity { AccountId = "foo2", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, _defaultCache.Object, _logger);

            // act
            var result = await repo.UpdateItemAsync(model);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("foo2", result.Id);
            Assert.AreEqual("bar", result.UserId);
            _defaultCache.Verify(
                o => o.Set<UserAccountEntity>(It.IsAny<string>(), It.IsAny<UserAccountEntity>()),
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
            var repo = new UserAccountRepository(client, null, _logger);

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
            mockContainer.Setup(x => x.ReplaceItemAsync<UserAccountEntity>(
                            It.IsAny<UserAccountEntity>(),
                            It.IsAny<string>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Forbidden));

            var client = CreateMockClient(mockContainer);
            var repo = new UserAccountRepository(client, null, _logger);

            // act
            await repo.UpdateItemAsync(model);

            // assert
        }

        [TestMethod]
        public async Task GetItemsAsync()
        {
            // arrange
            var expected = new UserAccountEntity { AccountId = "foo", UserId = "bar" };
            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, null, _logger);

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
            var expected = new List<UserAccountEntity>
            {
                new UserAccountEntity { AccountId = "foo", UserId = "bar" },
                new UserAccountEntity { AccountId = "foo2", UserId = "bar" },
            };

            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, null, _logger);

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
            var expected = new List<UserAccountEntity>
            {
                new UserAccountEntity { AccountId = "foo", UserId = "bar" },
                new UserAccountEntity { AccountId = "foo2", UserId = "bar" },
            };
            var mockCache = new Mock<ICacheService>();
            mockCache.Setup(x => x.ContainsList<UserAccountEntity>("bar")).Returns(true);
            mockCache.Setup(x => x.GetList<UserAccountEntity>("bar"))
                     .Returns(expected);

            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, mockCache.Object, _logger);

            // act
            var result = await repo.GetPartitionItemsAsync("bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            mockCache.Verify(o => o.GetList<UserAccountEntity>("bar"), Times.Once);
        }

        [TestMethod]
        public async Task GetPartitionItemsAsync_SetListCache()
        {
            // arrange
            var expected = new List<UserAccountEntity>
            {
                new UserAccountEntity { AccountId = "foo", UserId = "bar" },
                new UserAccountEntity { AccountId = "foo2", UserId = "bar" },
            };

            var client = CreateMockClient(expected);
            var repo = new UserAccountRepository(client, _defaultCache.Object, _logger);

            // act
            var result = await repo.GetPartitionItemsAsync("bar");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            _defaultCache.Verify(
                o => o.SetList<UserAccountEntity>(It.IsAny<string>(), It.IsAny<IList<UserAccountEntity>>()),
                Times.Once);
        }

        private CosmosClient CreateMockClient(UserAccountEntity? expectedItem = null)
        {
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.ReadItemAsync<UserAccountEntity>(
                            "test-id-9",
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.NotFound));

            if (expectedItem != null)
            {
                mockContainer.Setup(x => x.ReadItemAsync<UserAccountEntity>(
                                It.IsAny<string>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(CreateMockResponse(expectedItem));
            }

            mockContainer.Setup(x => x.DeleteItemAsync<UserAccountEntity>(
                            "test-id-9",
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.NotFound));

            if (expectedItem != null)
            {
                mockContainer.Setup(x => x.DeleteItemAsync<UserAccountEntity>(
                                expectedItem.Id,
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(CreateMockResponse(expectedItem));
            }

            if (expectedItem != null)
            {
                mockContainer.Setup(x => x.CreateItemAsync<UserAccountEntity>(
                                It.IsAny<UserAccountEntity>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(CreateMockResponse(expectedItem));
            }
            else
            {
                mockContainer.Setup(x => x.CreateItemAsync<UserAccountEntity>(
                                It.IsAny<UserAccountEntity>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.Conflict));
            }

            if (expectedItem != null)
            {
                mockContainer.Setup(x => x.ReplaceItemAsync<UserAccountEntity>(
                                It.IsAny<UserAccountEntity>(),
                                It.IsAny<string>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ReturnsAsync(CreateMockResponse(expectedItem));
            }
            else
            {
                mockContainer.Setup(x => x.ReplaceItemAsync<UserAccountEntity>(
                                It.IsAny<UserAccountEntity>(),
                                It.IsAny<string>(),
                                It.IsAny<PartitionKey>(),
                                It.IsAny<ItemRequestOptions>(),
                                It.IsAny<CancellationToken>()))
                             .ThrowsAsync(new CosmosException("testError", (int)HttpStatusCode.NotFound));
            }

            return CreateMockClient(mockContainer);
        }

        private CosmosClient CreateMockClient(IList<UserAccountEntity> expectedItems)
        {
            var pages = new List<Page<UserAccountEntity>>
            {
                Page<UserAccountEntity>.FromValues(expectedItems.ToArray(), null, new Mock<Response>().Object),
            };

            var results = AsyncPageable<UserAccountEntity>.FromPages(pages);
            var mockContainer = new Mock<CosmosContainer>();
            mockContainer.Setup(x => x.GetItemQueryIterator<UserAccountEntity>(
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
