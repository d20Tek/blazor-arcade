﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.UnitTests.Mocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Blazor.Arcade.Client.UnitTests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void ConfigureServices()
        {
            // arrange
            var services = new ServiceCollection();
            var mockConfig = new Mock<IConfiguration>();
            var mockHostEnv = new Mock<IWebAssemblyHostEnvironment>();

            // act
            Program.ConfigureServices(services, mockConfig.Object, mockHostEnv.Object);

            // assert
            Assert.IsTrue(services.Count > 35);
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(IArcadeService)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(HttpClient)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(IAuthorizationService)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(CustomAuthorizationMessageHandler)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(IArcadeMetadataService)));
        }

        [TestMethod]
        public void GetConfiguredServices()
        {
            // arrange
            // setup mock services for config and host environment
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(p => p.GetSection(It.IsAny<string>()))
                      .Returns(new Mock<IConfigurationSection>().Object);
            mockConfig.Setup(p => p["ArcadeServiceUrl"])
                      .Returns("https://test.com/api");
            var mockHostEnv = new Mock<IWebAssemblyHostEnvironment>();
            mockHostEnv.Setup(p => p.BaseAddress).Returns("https://test.com");

            // setup service collection with some required base services
            var services = new ServiceCollection();
            services.AddSingleton<NavigationManager>(new MockNavigationManager());
            services.AddSingleton<IConfiguration>(mockConfig.Object);
            services.AddSingleton<IJSRuntime>(new Mock<IJSRuntime>().Object);

            // act
            Program.ConfigureServices(services, mockConfig.Object, mockHostEnv.Object);
            var provider = services.BuildServiceProvider();

            // assert
            Assert.IsNotNull(provider);
            Assert.IsNotNull(provider.GetService<HttpClient>());
            Assert.IsNotNull(provider.GetService<IArcadeService>());
            Assert.IsNotNull(provider.GetService<IArcadeMetadataService>());
        }
    }
}