//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Client
{
    public class CrudClientService<T> : ICrudClientService<T>
    {
        public CrudClientService()
        {
           
        }
        public Task<T> GetEntitiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T?> GetEntityAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> CreateEntityAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateEntityAsync(string id, T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEntityAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
