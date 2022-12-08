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
        private readonly ILogger<UserAccountRepository> _logger = new Mock<ILogger<UserAccountRepository>>().Object;

        //[TestMethod]
        public async Task CrudTest_TargetingRealCosmosDb()
        {
            // arrange
            var client = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            var repo = new UserAccountRepository(client, null, _logger);

            var account = new UserAccount
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
                var r1 = await repo.CreateItemAsync(account);
                Assert.IsNotNull(r1);

                // act 2 - get item
                var r2 = await repo.GetItemAsync(account.Id, account.UserId);
                Assert.IsNotNull(r2);
                Assert.IsTrue(r1.Equals(r2));

                // act 3 - update item
                var update = new UserAccount
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
                var r4 = await repo.GetPartitionItemsAsync(account.UserId);
                Assert.IsNotNull(r4);
                Assert.IsTrue(r4.Any(x => x.Id == "test-id-2"));

                // act 5 - get partition many
                var ids = new List<string> { "test-id-2" };
                var r5 = await repo.GetItemsAsync(ids, account.UserId);
                Assert.IsNotNull(r5);
                Assert.IsTrue(r5.Any(x => x.Id == "test-id-2"));
            }
            finally
            {
                // act 5 - delete item
                await repo.DeleteItemAsync(account.Id, account.UserId);
            }
        }
    }
}
