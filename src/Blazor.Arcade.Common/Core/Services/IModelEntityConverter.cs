//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Services
{
    public interface IModelEntityConverter<T, TEntity>
        where T : new()
        where TEntity : CosmosStoreEntity, new()
    {
        public TEntity ConvertToEntity(T model);

        public T ConvertToModel(TEntity entity);
    }
}
