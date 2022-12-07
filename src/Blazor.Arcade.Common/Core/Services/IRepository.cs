//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Blazor.Arcade.Common.Core.Services
{
    public interface IRepository<T>
        where T : new()
    {
        Task<T> GetItemAsync(string itemId, string? partitionId = null);

        Task<IList<T>> GetPartitionItemsAsync(string partitionId);

        Task<IList<T>> GetItemsAsync(List<string> itemIds, string? partitionId = null);

        Task<T> CreateItemAsync(T item);

        Task<T> UpdateItemAsync(T item);

        Task DeleteItemAsync(string itemId, string? partitionId = null);
    }
}
