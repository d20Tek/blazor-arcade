//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.UnitTests.Mocks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;

namespace Blazor.Arcade.Client.UnitTests
{
    [TestClass]
    public class CustomAuthorizationMessageHandlerTests
    {
        [TestMethod]
        public void Create()
        {
            // arrange
            var navManager = new MockNavigationManager();
            var provider = new Mock<IAccessTokenProvider>().Object;
            var config = new Mock<IConfiguration>();
            config.Setup(p => p["ArcadeServiceUrl"]).Returns("https://test.com/api");

            // act
            var handler = new CustomAuthorizationMessageHandler(
                provider,
                navManager,
                config.Object);

            // assert
            Assert.IsNotNull(handler);
        }
    }
}
