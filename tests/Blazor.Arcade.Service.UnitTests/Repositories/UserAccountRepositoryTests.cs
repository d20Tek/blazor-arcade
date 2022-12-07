//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure.Cosmos;
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Repositories;
using Microsoft.Extensions.Logging;

namespace Blazor.Arcade.Service.UnitTests.Repositories
{
    [TestClass]
    public class UserAccountRepositoryTests
    {
        private readonly ILogger<UserAccountRepository> _logger = new Mock<ILogger<UserAccountRepository>>().Object;

        [TestMethod]
        [ExpectedException(typeof(EntityNotFoundException))]
        public async Task GetItemAsync()
        {
            // arrange
            var client = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            var repo = new UserAccountRepository(client, null, _logger);

            // act
            _ = await repo.GetItemAsync("test-id-1");

            // assert
        }
    }
}
