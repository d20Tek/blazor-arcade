//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
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
            var metadata = new List<GameMetadata>
            { 
                new GameMetadata { Id = "game.test.1" },
                new GameMetadata { Id = "game.test.2" }
            };
            var testResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create<List<GameMetadata>>(metadata)
            };

            var mockClient = new Mock<IArcadeService>();
            mockClient.Setup(p => p.GetAsync(_baseServiceUri))
                      .ReturnsAsync(testResponse);

            var service = new ArcadeMetadataService(mockClient.Object, _logger);

            // act
            var results = await service.GetGamesMetadataAsync();

            // assert
            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Any(p => p.Id == "game.test.1"));
        }
    }
}
