//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Services
{
    public interface IReadRepository<T>
    {
        public Task<IList<T>> GetAll();

        public Task<T?> GetById(string id);
    }
}
