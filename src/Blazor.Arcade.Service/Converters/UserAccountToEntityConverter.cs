//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Entities;

namespace Blazor.Arcade.Service.Converters
{
    internal class UserAccountToEntityConverter : IModelEntityConverter<UserProfile, UserAccountEntity>
    {
        public UserAccountEntity ConvertToEntity(UserProfile model)
        {
            return new UserAccountEntity
            {
                AccountId = model.Id,
                ServerId = model.Server,
                DisplayName = model.Name,
                AvatarUri = model.Avatar,
                Gender= model.Gender,
                Exp = model.Exp,
                UserId = model.UserId
            };
        }

        public UserProfile ConvertToModel(UserAccountEntity entity)
        {
            return new UserProfile
            {
                Id = entity.AccountId,
                Server = entity.ServerId,
                Name = entity.DisplayName,
                Avatar = entity.AvatarUri,
                Gender= entity.Gender,
                Exp = entity.Exp,
                UserId = entity.UserId
            };
        }
    }
}
