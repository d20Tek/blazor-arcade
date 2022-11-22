//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Arcade.Service.UnitTests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void ConfigureServices()
        {
            // arrange
            var builder = WebApplication.CreateBuilder(Array.Empty<string>());

            // act
            var result = Program.ConfigureServices(builder);

            // assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Services.Count > 210);
            Assert.IsTrue(result.Services.Any(p => p.ServiceType == typeof(IWebHostEnvironment)));
            Assert.IsTrue(result.Services.Any(p => p.ServiceType == typeof(IAuthenticationService)));
            Assert.IsTrue(result.Services.Any(p => p.ServiceType == typeof(JwtBearerHandler)));
            Assert.IsTrue(result.Services.Any(p => p.ServiceType == typeof(ICorsService)));
            Assert.IsTrue(result.Services.Any(p => p.ServiceType == typeof(IControllerFactory)));
        }

        [TestMethod]
        public void ConfigureApp()
        {
            // arrange
            var builder = WebApplication.CreateBuilder(Array.Empty<string>());
            Program.ConfigureServices(builder);

            var app = builder.Build();

            // act
            var result = Program.ConfigureApp(app);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Services.GetService<ICorsService>());
        }

        [TestMethod]
        public void ConfigureApp_DevEnvironment()
        {
            // arrange
            var options = new WebApplicationOptions
            {
                ApplicationName = "testhost",
                Args = Array.Empty<string>(),
                EnvironmentName = "Development",
            };

            var builder = WebApplication.CreateBuilder(options);
            Program.ConfigureServices(builder);

            var app = builder.Build();

            // act
            var result = Program.ConfigureApp(app);

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Services.GetService<ICorsService>());
        }
    }
}
