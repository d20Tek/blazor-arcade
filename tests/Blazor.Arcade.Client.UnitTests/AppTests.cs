//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Blazored.LocalStorage;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests
{
    [TestClass]
    public class AppTests
    {
        private IArcadeMetadataService _metadataService = new Mock<IArcadeMetadataService>().Object;
        private IChatHubClient _chatHub = new Mock<IChatHubClient>().Object;
        private IMessageBoxService _msgbox = new Mock<IMessageBoxService>().Object;
        private ILocalStorageService _storage = new Mock<ILocalStorageService>().Object;

        [TestMethod]
        public void Render_App()
        {
            // arrange
            var ctx = InitializeTestContext(true);

            // act
            var comp = ctx.RenderComponent<App>();

            // assert
            Assert.IsTrue(comp.Markup.Contains("Test Profile"));
            Assert.IsTrue(comp.Markup.Contains("Welcome to Blazor Arcade"));
            Assert.IsTrue(comp.Markup.Contains("Test Game"));
        }

        [TestMethod]
        public void Render_App_Anonymous()
        {
            // arrange
            var ctx = InitializeTestContext(false);
            ctx.AddTestAuthorization();

            // act
            var comp = ctx.RenderComponent<App>();

            // assert
            Assert.IsTrue(comp.Markup.Contains("Log in"));
            Assert.IsTrue(comp.Markup.Contains("Welcome to Blazor Arcade"));
        }

        [TestMethod]
        public void Render_App_WithGivenName()
        {
            // arrange
            var ctx = InitializeTestContext(false);

            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"), new Claim("given_name", "Foo"));

            // act
            var comp = ctx.RenderComponent<App>();

            // assert
            Assert.IsTrue(comp.Markup.Contains("Test Profile"));
            Assert.IsTrue(comp.Markup.Contains("Welcome to Blazor Arcade"));
        }

        [TestMethod]
        public void Render_App_ToggleNavMenu()
        {
            // arrange
            var ctx = InitializeTestContext(true);

            // act
            var comp = ctx.RenderComponent<App>();
            comp.Find(".navbar-toggler").Click();

            // assert
            Assert.IsTrue(comp.Markup.Contains("Test Profile"));
            Assert.IsTrue(comp.Markup.Contains("Welcome to Blazor Arcade"));
        }

        private b.TestContext InitializeTestContext(bool useAuth)
        {
            var listGames = new List<GameMetadata>
            {
                new GameMetadata { Id = "Test1", Name = "Test Game"}
            };

            var mockMetadata = new Mock<IArcadeMetadataService>();
            mockMetadata.Setup(p => p.GetGamesMetadataAsync())
                        .ReturnsAsync(listGames);

            var mockProfile = new Mock<IUserProfileClientService>();
            mockProfile.Setup(x => x.HasCurrentProfileAsync(It.IsAny<Task<AuthenticationState>?>()))
                                    .ReturnsAsync(true);
            mockProfile.Setup(x => x.GetCurrentProfileAsync(It.IsAny<Task<AuthenticationState>?>()))
                                    .ReturnsAsync(new UserAccount { Id = "user-profile-1", Name = "Test Profile" });

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(mockMetadata.Object);
            ctx.Services.AddSingleton<IChatHubClient>(_chatHub);
            ctx.Services.AddSingleton<IUserProfileClientService>(mockProfile.Object);
            ctx.Services.AddSingleton<IMessageBoxService>(_msgbox);
            ctx.Services.AddSingleton<ILocalStorageService>(_storage);

            if (useAuth == true)
            {
                ctx.AddTestAuthorization()
                   .SetAuthorized("Test User")
                   .SetClaims(new Claim("oid", "test-user-id"));
            }

            return ctx;
        }
    }
}
