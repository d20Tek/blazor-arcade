//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Components;
using Blazor.Arcade.Common.Core;
using Blazor.Arcade.Common.Models;
using Bunit;

namespace Blazor.Arcade.Client.UnitTests.Components
{
    [TestClass]
    public class GameTileTests
    {
        [TestMethod]
        public void Render()
        {
            // arrange
            var metadata = new GameMetadata
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
            };

            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<GameTile>(parameters => parameters
                    .Add(p => p.GameMetadata, metadata));

            // assert
            var expectedHtml =
@"  <div class=""nav-item px-3"">
      <a href=""/games/details/test.1"" class=""nav-link"">
        <img class=""game-tile"" src=""/images/games/test-game/testIcon.png"" >
        <span class=""game-tile-title"" >Test Game</span>
      </a>
    </div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_WithNullMetadata()
        {
            // arrange
            var ctx = new b.TestContext();

            // act
            var comp = ctx.RenderComponent<GameTile>();

            // assert
            var expectedHtml =
@"  <div class=""nav-item px-3"">
      <a href=""/games/details/"" class=""nav-link"">
        <img class=""game-tile"" >
        <span class=""game-tile-title"" ></span>
      </a>
    </div>
";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
