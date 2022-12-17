//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure.Cosmos;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Repositories;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Service.UnitTests.Repositories
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CosmosRepositoryTests
    {
        private readonly ILogger<UserProfileRepository> _logger = new Mock<ILogger<UserProfileRepository>>().Object;

        //[TestMethod]
        public async Task CrudTest_TargetingRealCosmosDb()
        {
            // arrange
            var client = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            var repo = new UserProfileRepository(client, null, _logger);

            var profile = new UserProfile
            {
                Id = "test-id-2",
                Name = "Test",
                Gender = "U",
                Server = "s1",
                UserId = "test-user-1"
            };

            try
            {
                // act 1 - create item
                var r1 = await repo.CreateItemAsync(profile);
                Assert.IsNotNull(r1);

                // act 2 - get item
                var r2 = await repo.GetItemAsync(profile.Id, profile.UserId);
                Assert.IsNotNull(r2);
                Assert.IsTrue(r1.Equals(r2));

                // act 3 - update item
                var update = new UserProfile
                {
                    Id = "test-id-2",
                    Name = "Test Update",
                    Gender = "U",
                    Server = "s111",
                    UserId = "test-user-1"
                };

                var r3 = await repo.UpdateItemAsync(update);
                Assert.IsNotNull(r3);
                Assert.IsTrue(r3.Equals(update));

                // act 4 - get partition items
                var r4 = await repo.GetPartitionItemsAsync(profile.UserId);
                Assert.IsNotNull(r4);
                Assert.IsTrue(r4.Any(x => x.Id == "test-id-2"));

                // act 5 - get partition many
                var ids = new List<string> { "test-id-2" };
                var r5 = await repo.GetItemsAsync(ids, profile.UserId);
                Assert.IsNotNull(r5);
                Assert.IsTrue(r5.Any(x => x.Id == "test-id-2"));
            }
            finally
            {
                // act 5 - delete item
                await repo.DeleteItemAsync(profile.Id, profile.UserId);
            }
        }
    }
}
