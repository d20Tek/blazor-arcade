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
            FixupListEntities(entity.PartitionId, entity, false);

            return entity;
        }

        public T Remove<T>(string entityId)
            where T : CosmosStoreEntity, new()
        {
            var key = FormatKey<T>(entityId);
            var entity = (T)_entityCache.Remove(key);
            FixupListEntities(entity.PartitionId, entity, true);

            return entity;
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

        private void FixupListEntities<T>(string listKey, T entity, bool isDeleted)
            where T : CosmosStoreEntity, new()
        {
            if (ContainsList<T>(listKey))
            {
                var cachedPartition = GetList<T>(listKey);

                if (isDeleted)
                {
                    cachedPartition = cachedPartition.Where(
                        cached => cached.Id != entity.Id).ToList();
                }
                else if (cachedPartition.Any(cached => cached.Id == entity.Id))
                {
                    cachedPartition = cachedPartition.Select(
                        cached => cached.Id == entity.Id ? entity : cached).ToList();
                }
                else
                {
                    cachedPartition.Add(entity);
                }

                SetList<T>(listKey, cachedPartition);
            }
        }
    }
}
