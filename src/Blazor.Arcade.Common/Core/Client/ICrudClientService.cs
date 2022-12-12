//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Client
{
    public interface ICrudClientService<T>
        where T : class, new()
    {
        public Task<IList<T>> GetEntitiesAsync();

        public Task<T?> GetEntityAsync(string id);

        public Task<T> CreateEntityAsync(T entity);

        public Task<T> UpdateEntityAsync(string id, T entity);

        public Task DeleteEntityAsync(string id);
    }
}
