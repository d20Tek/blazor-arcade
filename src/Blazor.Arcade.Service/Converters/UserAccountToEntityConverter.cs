//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Blazor.Arcade.Common.Models;
using Blazor.Arcade.Service.Entities;

namespace Blazor.Arcade.Service.Converters
{
    internal class UserAccountToEntityConverter : IModelEntityConverter<UserAccount, UserAccountEntity>
    {
        public UserAccountEntity? ConvertToEntity(UserAccount? model)
        {
            if (model == default)
            {
                return default;
            }

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

        public UserAccount? ConvertToModel(UserAccountEntity? entity)
        {
            if (entity == default)
            {
                return default;
            }

            return new UserAccount
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
