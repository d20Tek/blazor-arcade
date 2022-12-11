//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure.Cosmos;
using Blazor.Arcade.Service.Hubs;
using Blazor.Arcade.Service.Logic;
using Blazor.Arcade.Service.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.Arcade.Service
{
    public class Program
    {
        private const string _configSignalRConnection = "Azure.SignalR.ConnectionString";
        private const string _configCosmosDbConnection = "CosmosDb.ConnectionString";

        [ExcludeFromCodeCoverage]
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);

            var app = builder.Build();
            ConfigureApp(app).Run();
        }

        internal static WebApplicationBuilder ConfigureServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(
                            "https://white-ground-04941e21e.2.azurestaticapps.net",
                            "https://localhost:7238")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddRepositoryServices();
            builder.Services.AddLogicServices();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR()
                            .AddAzureSignalR(builder.Configuration[_configSignalRConnection]);
            builder.Services.AddCosmosClient(builder.Configuration[_configCosmosDbConnection]);

            return builder;
        }

        internal static WebApplication ConfigureApp(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/api/v1/chat");
            });


            return app;
        }
    }
}