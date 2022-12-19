//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Client.Services;
using Blazor.Arcade.Client.Types;
using Blazor.Arcade.Client.UnitTests.Mocks;
using Blazor.Arcade.Client.UnitTests.Services;
using Blazor.Arcade.Common.Core.Client;
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
            mockConfig.Setup(p => p["ArcadeServiceUrl"])
                      .Returns("https://test/.com");
            var mockHostEnv = new Mock<IWebAssemblyHostEnvironment>();

            // act
            Program.ConfigureServices(services, mockConfig.Object, mockHostEnv.Object);

            // assert
            Assert.IsTrue(services.Count > 35);
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(ITypedHttpClient)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(HttpClient)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(IAuthorizationService)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(CustomAuthorizationMessageHandler)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(IArcadeMetadataService)));
            Assert.IsTrue(services.Any(p => p.ServiceType == typeof(IUserProfileClientService)));
        }

        [TestMethod]
        public void ConfigureHttpClients_NullServiceUrl()
        {
            // arrange
            var services = new ServiceCollection();
            var config = new ArcadeConfiguration
            {
                BaseDefaultUrl = "https://test.com/",
                ServiceUrl = null
            };

            // act
            Program.ConfigureHttpClients(services, config);

            // assert
            Assert.IsTrue(services.Count >= 21);
        }

        [TestMethod]
        public void ConfigureHttpClients_ServiceUrl()
        {
            // arrange
            var services = new ServiceCollection();
            var config = new ArcadeConfiguration
            {
                BaseDefaultUrl = "https://test.com/",
                ServiceUrl = "https://test.com/api/v1"
            };

            // act
            Program.ConfigureHttpClients(services, config);

            // assert
            Assert.IsTrue(services.Count >= 21);
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
            Assert.IsNotNull(provider.GetService<ITypedHttpClient>());
            Assert.IsNotNull(provider.GetService<IArcadeMetadataService>());
            Assert.IsNotNull(provider.GetService<IUserProfileClientService>());
        }
    }
}
