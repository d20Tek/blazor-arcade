//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Azure.Cosmos;
using Blazor.Arcade.Common.Core.Services;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace Blazor.Arcade.Service.Repositories
{
    internal abstract class CosmosRepository<T, TEntity> : IRepository<T>
        where T : new()
        where TEntity : CosmosStoreEntity, new()
    {
        private const int _defaultRetryAmount = 3;
        private readonly TimeSpan _operationTimeout = TimeSpan.FromSeconds(4);

        private readonly CosmosClient _cosmosClient;
        private readonly string _dbId;
        private readonly string _containerId;
        private readonly string _partitionDef;
        private readonly string _typeName;
        private readonly IModelEntityConverter<T, TEntity> _converter;
        private readonly ICacheService? _cache;
        private readonly ILogger _logger;

        public CosmosRepository(
            CosmosClient cosmosClient,
            string databaseId,
            string containerId,
            string partitionDef,
            IModelEntityConverter<T, TEntity> converter,
            ICacheService? cache,
            ILogger logger)
        {
            _typeName= typeof(T).Name;

            _cosmosClient= cosmosClient;
            _dbId = databaseId;
            _containerId= containerId;
            _partitionDef= partitionDef;
            _converter = converter;
            _cache = cache;
            _logger = logger;
        }

        public async Task<T> GetItemAsync(string itemId, string? partitionId = null)
        {
            var entity = await CosmosOperationAsync(nameof(GetItemAsync), async (c) =>
            {
                try
                {
                    if (_cache != null && _cache.Contains<TEntity>(itemId))
                    {
                        return _cache.Get<TEntity>(itemId);
                    }

                    var container = await VerifyContainerInitialized();
                    partitionId = VerifyPartitionId(itemId, partitionId);
                    var response = await container.ReadItemAsync<TEntity>(
                        itemId,
                        new PartitionKey(partitionId),
                        cancellationToken: c);

                    _cache?.Set(itemId, response.Value);
                    return response.Value;
                }
                catch (CosmosException ex)
                {
                    if (ex.Status == (int)HttpStatusCode.NotFound)
                    {
                        throw new EntityNotFoundException("Id", itemId, ex);
                    }

                    throw;
                }
            });

            return _converter.ConvertToModel(entity);
        }

        public async Task<IList<T>> GetPartitionItemsAsync(string partitionId)
        {
            var list =  await CosmosOperationAsync(nameof(GetPartitionItemsAsync), async (c) =>
            {
                if (_cache != null && _cache.ContainsList<TEntity>(partitionId))
                {
                    return _cache.GetList<TEntity>(partitionId);
                }

                var container = await VerifyContainerInitialized();

                var queryDefinition = new QueryDefinition("select * from c");

                var iterator = container.GetItemQueryIterator<TEntity>(
                    queryDefinition,
                    requestOptions: new QueryRequestOptions
                    {
                        PartitionKey = new PartitionKey(partitionId)
                    },
                    cancellationToken: c);

                var entities = new List<TEntity>();
                await foreach (var e in iterator)
                {
                    entities.Add(e);
                }

                _cache?.SetList(partitionId, entities);
                return entities;
            });

            var items = list.Select(x => _converter.ConvertToModel(x));
            return items.ToList();
        }

        public async Task<IList<T>> GetItemsAsync(List<string> itemIds, string? partitionId = null)
        {
            var items = await Task.WhenAll(
                itemIds.Select(async id => await GetItemAsync(id, partitionId))
                );

            return items.ToList();
        }

        public async Task<T> CreateItemAsync(T item)
        {
            return await CosmosOperationAsync(nameof(CreateItemAsync), async (c) =>
            {
                var entity = _converter.ConvertToEntity(item);

                try
                {
                    var container = await VerifyContainerInitialized();

                    // Create an item in the container representing this entity.
                    var response = await container.CreateItemAsync<TEntity>(
                            entity,
                            new PartitionKey(entity.PartitionId),
                            cancellationToken: c);

                    _cache?.Set(entity.Id, response.Value);
                    return _converter.ConvertToModel(response.Value);
                }
                catch (CosmosException ex)
                {
                    if (ex.Status == (int)HttpStatusCode.Conflict)
                    {
                        throw new EntityAlreadyExistsException("Id", entity.Id, ex);
                    }

                    throw;
                }
            });
        }

        public async Task<T> UpdateItemAsync(T item)
        {
            return await CosmosOperationAsync(nameof(CreateItemAsync), async (c) =>
            {
                var entity = _converter.ConvertToEntity(item);

                try
                {
                    var container = await VerifyContainerInitialized();
                    var response = await container.ReplaceItemAsync<TEntity>(
                            entity,
                            entity.Id,
                            new PartitionKey(entity.PartitionId),
                            cancellationToken: c);

                    _cache?.Set(entity.Id, response.Value);
                    return _converter.ConvertToModel(response.Value);
                }
                catch (CosmosException ex)
                {
                    if (ex.Status == (int)HttpStatusCode.NotFound)
                    {
                        throw new EntityNotFoundException("Id", entity.Id, ex);
                    }

                    throw;
                }
            });
        }

        public async Task DeleteItemAsync(string itemId, string? partitionId = null)
        {
            await CosmosOperationAsync(nameof(DeleteItemAsync), async (c) =>
            {
                try
                {
                    var container = await VerifyContainerInitialized();
                    partitionId = VerifyPartitionId(itemId, partitionId);

                    var response = await container.DeleteItemAsync<TEntity>(
                            itemId,
                            new PartitionKey(partitionId),
                            cancellationToken: c);

                    _cache?.Remove<TEntity>(itemId);
                }
                catch (CosmosException ex)
                {
                    if (ex.Status == (int)HttpStatusCode.NotFound)
                    {
                        throw new EntityNotFoundException("Id", itemId, ex);
                    }

                    throw;
                }
            });
        }

        private static string VerifyPartitionId(string itemId, string? partitionId)
        {
            partitionId ??= itemId;
            return partitionId;
        }

        [ExcludeFromCodeCoverage]
        private async Task<TResult> CosmosOperationAsync<TResult>(
            string methodName,
            Func<CancellationToken, Task<TResult>> operation,
            int retries = _defaultRetryAmount)
        {
            var serviceName = string.Format("CosmosContainer-{0}; Method-{1}", _typeName, methodName);

            try
            {
                while (retries > 0)
                {
                    var operationCount = 1 + _defaultRetryAmount - retries;
                    var operationTimeoutBackoff = _operationTimeout * operationCount;
                    var cancellationToken = new CancellationTokenSource(operationTimeoutBackoff).Token;

                    _logger.LogTrace($"Begin Cosmos Call: '{serviceName}'");
                    var result = await operation(cancellationToken);
                    _logger.LogTrace($"End Cosmos Call: '{serviceName}'");

                    return result;
                }

                _logger.LogError($"Failed CosmosOperation request '{serviceName}'.");
                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                if (ex is CosmosException || ex is SocketException)
                {
                    _logger.LogWarning($"Retrying Cosmos Operation: '{serviceName}' with error '{ex.Message}'");
                    return await CosmosOperationAsync<TResult>(methodName, operation, --retries);
                }

                throw;
            }
        }

        [ExcludeFromCodeCoverage]
        private async Task CosmosOperationAsync(
            string methodName,
            Func<CancellationToken, Task> operation,
            int retries = _defaultRetryAmount)
        {
            var serviceName = string.Format("CosmosContainer-{0}; Method-{1}", _typeName, methodName);

            try
            {
                while (retries > 0)
                {
                    var operationCount = 1 + _defaultRetryAmount - retries;
                    var operationTimeoutBackoff = _operationTimeout * operationCount;
                    var cancellationToken = new CancellationTokenSource(operationTimeoutBackoff).Token;

                    _logger.LogTrace($"Begin Cosmos Call: '{serviceName}'");
                    await operation(cancellationToken);
                    _logger.LogTrace($"End Cosmos Call: '{serviceName}'");
                    return;
                }

                _logger.LogError($"Failed CosmosOperation request '{serviceName}'.");
                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                if (ex is CosmosException || ex is SocketException)
                {
                    _logger.LogWarning($"Retrying Cosmos Operation: '{serviceName}' with error '{ex.Message}'");
                    await CosmosOperationAsync(methodName, operation, --retries);
                }

                throw;
            }
        }

        protected virtual async Task<CosmosContainer> VerifyContainerInitialized()
        {
            // Create the database if it doesn't exist.
            var dbResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_dbId);

            // Create the container if it doesn't exist.
            await dbResponse.Database
                            .CreateContainerIfNotExistsAsync(_containerId, _partitionDef);

            return _cosmosClient.GetContainer(_dbId, _containerId);
        }
    }
}
