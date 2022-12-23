//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Entities;

namespace Blazor.Arcade.Service.Converters
{
    internal class GameSessionToEntityConverter : IModelEntityConverter<GameSession, GameSessionEntity>
    {
        public GameSessionEntity ConvertToEntity(GameSession model)
        {
            return new GameSessionEntity
            {
                SessionId = model.Id,
                Name = model.Name,
                ServerId = model.ServerId,
                MetadataId = model.MetadataId,
                HostId = model.HostId,
                Phase = model.Phase,
                GameState= model.GameState,
            };
        }

        public GameSession ConvertToModel(GameSessionEntity entity)
        {
            return new GameSession
            {
                Id= entity.Id,
                Name= entity.Name,
                ServerId= entity.ServerId,
                MetadataId= entity.MetadataId,
                HostId= entity.HostId,
                Phase = entity.Phase,
                GameState= entity.GameState,
            };
        }
    }
}
