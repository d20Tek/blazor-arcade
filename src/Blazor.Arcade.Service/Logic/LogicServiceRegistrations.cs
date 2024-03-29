﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Service.Logic
{
    internal static class LogicServiceRegistrations
    {
        public static IServiceCollection AddLogicServices(this IServiceCollection services)
        {
            services.AddSingleton<IUserProfileActionManager, UserProfileActionManager>();
            services.AddSingleton<IGameSessionActionManager, GameSessionActionManager>();

            return services;
        }
    }
}
