//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Components;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
using Blazor.Arcade.Common.Core;
using Blazor.Arcade.Common.Models;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests.Components
{
    [TestClass]
    public class GameListTests
    {
        [TestMethod]
        public void Render()
        {
            // arrange
            var mockMetadata = CreateMockMetadataService();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(mockMetadata.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<GameList>();

            // assert
            var expectedHtml =
@"<div class=""nav-item px-3"" >
    <a href=""/games/details/test.1"" class=""nav-link"">
      <img class=""game-tile"" src=""/images/games/test-game/testIcon.png"" >
      <span class=""game-tile-title"" >Test Game</span>
    </a>
  </div>
  <div class=""nav-item px-3"" >
    <a href=""/games/details/test.2"" class=""nav-link"">
      <img class=""game-tile"" src=""/images/games/test-game-2/testIcon.png"" >
      <span class=""game-tile-title"" >Another Game</span>
    </a>
  </div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_WithNullGameList()
        {
            // arrange

            var mockMetadata = new Mock<IArcadeMetadataService>();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(mockMetadata.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<GameList>();

            // assert
            var expectedHtml = @"";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void OnAuthStateChanged()
        {
            // arrange
            var mockMetadata = CreateMockMetadataService();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(mockMetadata.Object);
            ctx.AddTestAuthorization();

            var user = new ClaimsPrincipal(new MockIdentity());
            var authState = new AuthenticationState(user);

            // act
            var comp = ctx.RenderComponent<GameList>();
            comp.InvokeAsync(() => comp.Instance.OnAuthStateChanged(Task.FromResult(authState)));

            // assert
            Assert.IsNotNull(comp);
            Assert.IsNotNull(comp.Instance._games);
            if (comp.Instance._games != null)
            {
                Assert.AreEqual(2, comp.Instance._games.Count);
            }
        }

        [TestMethod]
        public void OnAuthStateChanged_WithNullIdentity()
        {
            // arrange
            var mockMetadata = CreateMockMetadataService();

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(mockMetadata.Object);
            ctx.AddTestAuthorization();

            var user = new ClaimsPrincipal();
            var authState = new AuthenticationState(user);

            // act
            var comp = ctx.RenderComponent<GameList>();
            comp.InvokeAsync(() => comp.Instance.OnAuthStateChanged(Task.FromResult(authState)));

            // assert
            Assert.IsNotNull(comp);
            Assert.IsNull(comp.Instance._games);
        }

        private Mock<IArcadeMetadataService> CreateMockMetadataService()
        {
            var listGames = new List<GameMetadata>
                {
                    new GameMetadata
                    {
                        Id = "test.1",
                        Name = "Test Game",
                        SortOrder = 100,
                        NumPlayers = new ValueRange(1, 2),
                        Locations = new GameLocationMetadata
                        {
                            GameUrl = "/test-game",
                            IconUrl = "/images/games/test-game/testIcon.png"
                        }
                    },
                    new GameMetadata
                    {
                        Id = "test.2",
                        Name = "Another Game",
                        SortOrder = 200,
                        NumPlayers = new ValueRange(1, 2),
                        Locations = new GameLocationMetadata
                        {
                            GameUrl = "/test-game-2",
                            IconUrl = "/images/games/test-game-2/testIcon.png"
                        }
                    },
                };

            var mockMetadata = new Mock<IArcadeMetadataService>();
            mockMetadata.Setup(p => p.GetGamesMetadataAsync())
                        .ReturnsAsync(listGames);

            return mockMetadata;
        }
    }
}
