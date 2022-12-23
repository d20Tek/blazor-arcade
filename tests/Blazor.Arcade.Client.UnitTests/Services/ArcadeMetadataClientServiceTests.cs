//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
using Blazor.Arcade.Common.Core.Client;
using Microsoft.Extensions.Logging;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class ArcadeMetadataClientServiceTests
    {
        private readonly ILogger<ArcadeMetadataService> _logger = new Mock<ILogger<ArcadeMetadataService>>().Object;

        [TestMethod]
        public async Task GetGameMetadataAll()
        {
            // arrange
            var responseContent = @"
            [
                { ""id"": ""game.test.1"" },
                { ""id"": ""game.test.2"" }
            ]";

            var typedClient = CreateMockClient(responseContent);
            var service = new ArcadeMetadataService(typedClient, _logger);

            // act
            var results = await service.GetGamesMetadataAsync();

            // assert
            Assert.IsNotNull(results);
            if (results != null)
            {
                Assert.AreEqual(2, results.Count);
                Assert.IsTrue(results.Any(p => p.Id == "game.test.1"));
            }
        }

        [TestMethod]
        public async Task GetGameMetadataById()
        {
            // arrange
            var responseContent = @"{ ""id"": ""game.test.1"" }";
            var typedClient = CreateMockClient(responseContent);
            var service = new ArcadeMetadataService(typedClient, _logger);

            // act
            var result = await service.GetGameMetadataByIdAsync("game.test.1");

            // assert
            Assert.IsNotNull(result);
            if (result != null)
            {
                Assert.AreEqual("game.test.1", result.Id);
            }
        }

        private ITypedHttpClient CreateMockClient(string content)
        {
            var httpClient = MockHttpClientHelper.CreateHttpClient(content);
            var typedClient = new TypedHttpClient(httpClient);

            return typedClient;
        }
    }
}
