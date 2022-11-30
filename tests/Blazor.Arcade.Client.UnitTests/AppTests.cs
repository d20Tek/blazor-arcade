//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Common.Models;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Blazor.Arcade.Client.UnitTests
{
    [TestClass]
    public class AppTests
    {
        private IArcadeMetadataService _metadataService = new Mock<IArcadeMetadataService>().Object;

        [TestMethod]
        public void Render_App()
        {
            // arrange
            var listGames = new List<GameMetadata>
            {
                new GameMetadata { Id = "Test1", Name = "Test Game"}
            };

            var mockMetadata = new Mock<IArcadeMetadataService>();
            mockMetadata.Setup(p => p.GetGamesMetadataAsync())
                        .ReturnsAsync(listGames);

            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(mockMetadata.Object);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<App>();

            // assert
            Assert.IsTrue(comp.Markup.Contains("Hi, Test User!"));
            Assert.IsTrue(comp.Markup.Contains("Welcome to Blazor Arcade"));
            Assert.IsTrue(comp.Markup.Contains("Test Game"));
        }

        [TestMethod]
        public void Render_App_Anonymous()
        {
            // arrange
            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(_metadataService);
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
            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(_metadataService);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"), new Claim("given_name", "Foo"));

            // act
            var comp = ctx.RenderComponent<App>();

            // assert
            Assert.IsTrue(comp.Markup.Contains("Hi, Foo!"));
            Assert.IsTrue(comp.Markup.Contains("Welcome to Blazor Arcade"));
        }

        [TestMethod]
        public void Render_App_ToggleNavMenu()
        {
            // arrange
            var ctx = new b.TestContext();
            ctx.Services.AddSingleton<IArcadeMetadataService>(_metadataService);
            ctx.AddTestAuthorization()
               .SetAuthorized("Test User")
               .SetClaims(new Claim("oid", "test-user-id"));

            // act
            var comp = ctx.RenderComponent<App>();
            comp.Find(".navbar-toggler").Click();

            // assert
            Assert.IsTrue(comp.Markup.Contains("Hi, Test User!"));
            Assert.IsTrue(comp.Markup.Contains("Welcome to Blazor Arcade"));
        }
    }
}
