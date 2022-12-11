//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Services
{
    public interface ICacheService
    {
        bool Contains<T>(string entityId)
            where T : CosmosStoreEntity, new();

        T Get<T>(string entityId)
            where T : CosmosStoreEntity, new();

        T Set<T>(string entityId, T entity)
            where T : CosmosStoreEntity, new();

        T Remove<T>(string entityId)
            where T : CosmosStoreEntity, new();

        bool ContainsList<T>(string keyId)
            where T : CosmosStoreEntity, new();

        IList<T> GetList<T>(string keyId)
            where T : CosmosStoreEntity, new();

        IList<T> SetList<T>(string keyId, IList<T> entities)
            where T : CosmosStoreEntity, new();

        IList<T> RemoveList<T>(string keyId)
            where T : CosmosStoreEntity, new();
    }
}
