//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
using Blazor.Arcade.Common.Core.Client;
using Blazor.Arcade.Common.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace Blazor.Arcade.Client.UnitTests.Services
{
    [TestClass]
    public class ArcadeMetadataClientServiceTests
    {
        private const string _baseServiceUri = "api/v1/game-metadata";
        private readonly ILogger<ArcadeMetadataService> _logger = new Mock<ILogger<ArcadeMetadataService>>().Object;

        [TestMethod]
        public async Task GetGameMetadataAll()
        {
            // arrange
            // arrange
            var responseContent = @"
            [
                { ""id"": ""game.test.1"" },
                { ""id"": ""game.test.2"" }
            ]";

            var httpClient = MockHttpClientHelper.CreateHttpClient(responseContent);
            var typedClient = new TypedHttpClient(httpClient);

            var service = new ArcadeMetadataService(typedClient, _logger);

            // act
            var results = await service.GetGamesMetadataAsync();

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Any(p => p.Id == "game.test.1"));
        }
    }
}
