//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Pages;
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;


namespace Blazor.Arcade.Client.UnitTests.Pages
{
    [TestClass]
    public class GameDetailsTests
    {
        [TestMethod]
        public void Render_Initial()
        {
            // arrange
            var mockProfileService = InitializeProfileService();
            var mockMetadataService = new Mock<IArcadeMetadataService>();
            var mockSessionHub = new Mock<IGameSessionHubClient>();

            var ctx = new b.TestContext();
            ctx.AddTestAuthorization();
            ctx.Services.AddSingleton<IUserProfileClientService>(mockProfileService.Object);
            ctx.Services.AddSingleton<IArcadeMetadataService>(mockMetadataService.Object);
            ctx.Services.AddSingleton<IGameSessionHubClient>(mockSessionHub.Object);

            // act
            var comp = ctx.RenderComponent<GameDetails>(parameters => parameters
                .Add(p => p.MetadataId, "test-game"));

            // assert
            var expectedHtml =
@"
<div class=""header-top-bar"" >
  <h4 class=""px-3 header-brand"" >Game Details</h4>
</div>
<div class=""banner-background mt-2 p-3"" style=""background: linear-gradient(0deg, rgba(0, 0, 0, 0.8) 0%, rgba(0, 0, 0, 0.8) 100%), url('') center center / cover no-repeat;"" >
  <div class=""row justify-content-center"" >
    <div class=""col-lg-6 col-xl-4 col-sm-12 my-2 text-center"" >
      <img class=""text-center"" height=""180"" >
    </div>
    <div class=""col-lg-6 col-xl-4 col-sm-12 my-2 text-center"" >
      <div class=""overflow-auto my-2"" ></div>
      <div class=""my-2"" >
        <span class=""mx-2"" >
          <span class=""oi oi-person me-1"" aria-hidden=""true"" ></span>
          <span ></span>
        </span>
        <span class=""mx-2"" >
          <span class=""oi oi-clock me-1"" aria-hidden=""true"" ></span>
          <span >
            min</span>
        </span>
        <span class=""mx-2"" >
          <span >Difficulty:
          </span>
          <span >
            / 10</span>
        </span>
      </div>
    </div>
  </div>
  <div class=""row justify-content-center"" >
    <div class=""col-lg-6 col-xl-4 col-sm-12 my-2"" >
      <img class=""mx-2 float-start"" >
      <div class=""mx-2 fw-bold"" >About Game:</div>
      <div class=""mx-2 overflow-auto"" style=""max-height: 200px;"" ></div>
    </div>
    <div class=""col-lg-6 col-xl-4 col-sm-12 my-2 text-center"" >
      <button class=""btn btn-primary"" >Create Game Session</button>
    </div>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_WithMetadata()
        {
            // arrange
            var mockProfileService = InitializeProfileService();
            var mockMetadataService = InitializeMetadataService();
            var mockSessionHub = new Mock<IGameSessionHubClient>();

            var ctx = new b.TestContext();
            ctx.AddTestAuthorization();
            ctx.Services.AddSingleton<IUserProfileClientService>(mockProfileService.Object);
            ctx.Services.AddSingleton<IArcadeMetadataService>(mockMetadataService.Object);
            ctx.Services.AddSingleton<IGameSessionHubClient>(mockSessionHub.Object);

            // act
            var comp = ctx.RenderComponent<GameDetails>(parameters => parameters
                .Add(p => p.MetadataId, "test-game"));

            // assert
            var expectedHtml =
@"
<div class=""header-top-bar"" >
  <h4 class=""px-3 header-brand"" >Tic-Tac-Toe</h4>
</div>
<div class=""banner-background mt-2 p-3"" style=""background: linear-gradient(0deg, rgba(0, 0, 0, 0.8) 0%, rgba(0, 0, 0, 0.8) 100%), 
    url('/images/games/tic-tac-toe/banner-background.png') center center / cover no-repeat;"" >
  <div class=""row justify-content-center"" >
    <div class=""col-lg-6 col-xl-4 col-sm-12 my-2 text-center"" >
      <img class=""text-center"" height=""180"" src=""/images/games/tic-tac-toe/large-logo.png"" >
    </div>
    <div class=""col-lg-6 col-xl-4 col-sm-12 my-2 text-center"" >
      <div class=""overflow-auto my-2"" >
        <span class=""badge badge-pill-outline"" >Classic</span>
        <span class=""badge badge-pill-outline"" >Easy</span>
        <span class=""badge badge-pill-outline"" >Pencil &amp; Paper</span>
      </div>
      <div class=""my-2"" >
        <span class=""mx-2"" >
          <span class=""oi oi-person me-1"" aria-hidden=""true"" ></span>
          <span >1 - 2</span>
        </span>
        <span class=""mx-2"" >
          <span class=""oi oi-clock me-1"" aria-hidden=""true"" ></span>
          <span >1 min</span>
        </span>
        <span class=""mx-2"" >
          <span >Difficulty:
          </span>
          <span >1 / 10</span>
        </span>
      </div>
    </div>
  </div>
  <div class=""row justify-content-center"" >
    <div class=""col-lg-6 col-xl-4 col-sm-12 my-2"" >
      <img class=""mx-2 float-start"" src=""/images/games/tic-tac-toe/icon.png"" >
      <div class=""mx-2 fw-bold"" >About Game:</div>
      <div class=""mx-2 overflow-auto"" style=""max-height: 200px;"" >The classic pen and paper game of Xs and Os. Get three of your pieces in a row, column, or diagonally to win. Configure it to play multiple rounds of this fast paced game with your friends.</div>
    </div>
    <div class=""col-lg-6 col-xl-4 col-sm-12 my-2 text-center"" >
      <button class=""btn btn-primary"" >Create Game Session</button>
    </div>
  </div>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        private Mock<IUserProfileClientService> InitializeProfileService()
        {
            var currentprofile = new UserProfile
            {
                Id = "test-profile-1",
                Name = "Test1",
                Server = "s1",
                UserId = "test-user-1"
            };

            var mockProfileService = new Mock<IUserProfileClientService>();
            mockProfileService.Setup(x => x.GetCurrentProfileAsync(It.IsAny<Task<AuthenticationState>?>()))
                              .ReturnsAsync(currentprofile);

            return mockProfileService;
        }

        private Mock<IArcadeMetadataService> InitializeMetadataService()
        {
            var metadata = new GameMetadata
            {
                Id = "arcade-game-tictactoe",
                Name = "Tic-Tac-Toe",
                Description = "The classic pen and paper game of Xs and Os. Get three of your pieces in a row, column, or diagonally to win. Configure it to play multiple rounds of this fast paced game with your friends.",
                NumPlayers = new Common.Core.ValueRange
                {
                    Min = 1,
                    Max = 2
                },
                Duration = new Common.Core.ValueRange
                {
                    Min = 1,
                    Max = 1
                },
                Complexity = 1,
                Tags = new List<string> { "Classic", "Easy", "Pencil & Paper" },
                SortOrder = 100,
                Locations = new GameLocationMetadata
                {
                    GameUrl = "/games/tic-tac-toe",
                    IconUrl = "/images/games/tic-tac-toe/icon.png",
                    LargeLogoUrl = "/images/games/tic-tac-toe/large-logo.png",
                    BannerImageUrl = "/images/games/tic-tac-toe/banner-background.png"
                }
            };

            var mockMetadataService = new Mock<IArcadeMetadataService>();
            mockMetadataService.Setup(x => x.GetGameMetadataByIdAsync(It.IsAny<string>()))
                               .ReturnsAsync(metadata);

            return mockMetadataService;
        }
    }
}
