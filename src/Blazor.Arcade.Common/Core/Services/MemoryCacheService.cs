//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Runtime.Caching;

namespace Blazor.Arcade.Common.Core.Services
{
    public class MemoryCacheService : ICacheService
    {
        private const int _defaultSlidingExpiration = 5;

        private readonly CacheItemPolicy _cachePolicy;
        private readonly ObjectCache _entityCache;

        public MemoryCacheService()
        {
            _cachePolicy = new CacheItemPolicy
            {
                SlidingExpiration = new TimeSpan(0, _defaultSlidingExpiration, 0)
            };

            _entityCache = MemoryCache.Default;
        }

        public bool Contains<T>(string entityId)
            where T : CosmosStoreEntity, new()
        {
            var key = FormatKey<T>(entityId);
            return _entityCache.Contains(key);
        }

        public T Get<T>(string entityId)
            where T : CosmosStoreEntity, new()
        {
            var key = FormatKey<T>(entityId);
            return (T)_entityCache.Get(key);
        }

        public T Set<T>(string entityId, T entity)
            where T : CosmosStoreEntity, new()
        {
            var key = FormatKey<T>(entityId);
            _entityCache.Set(key, entity, _cachePolicy);

            return entity;
        }

        public T Remove<T>(string entityId)
            where T : CosmosStoreEntity, new()
        {
            var key = FormatKey<T>(entityId);
            return (T)_entityCache.Remove(key);
        }

        public bool ContainsList<T>(string keyId)
            where T : CosmosStoreEntity, new()
        {
            return _entityCache.Contains(keyId);
        }

        public IList<T> GetList<T>(string keyId)
            where T : CosmosStoreEntity, new()
        {
            return (IList<T>)_entityCache.Get(keyId);
        }

        public IList<T> SetList<T>(string keyId, IList<T> entities)
            where T : CosmosStoreEntity, new()
        {
            _entityCache.Set(keyId, entities, _cachePolicy);
            return entities;
        }

        public IList<T> RemoveList<T>(string keyId)
            where T : CosmosStoreEntity, new()
        {
            return (IList<T>)_entityCache.Remove(keyId);
        }

        private string FormatKey<T>(string entityId)
            where T : CosmosStoreEntity, new()
        {
            return string.Format("{0}_{1}", typeof(T).Name, entityId);
        }
    }
}
