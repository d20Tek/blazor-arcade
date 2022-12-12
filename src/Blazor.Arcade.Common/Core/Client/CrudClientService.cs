//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Blazor.Arcade.Common.Core.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blazor.Arcade.Common.Core.Client
{
    public class CrudClientService<T> : ClientServiceBase, ICrudClientService<T>
        where T: class, new()
    {
        private readonly string _serviceUrl;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
        };

        public CrudClientService(string serviceUrl, ITypedHttpClient typedClient, ILogger<CrudClientService<T>> logger)
            : base(logger)
        {
            serviceUrl.ThrowIfEmpty(nameof(serviceUrl));
            _serviceUrl = serviceUrl;
            _client = typedClient.HttpClient;
        }

        public async Task<IList<T>> GetEntitiesAsync()
        {
            return await ServiceOperationAsync<List<T>>(
                nameof(GetEntitiesAsync),
                async () =>
                {
                    var response = await _client.GetAsync(_serviceUrl);
                    response.EnsureSuccessStatusCode();

                    var results = await response.Content.ReadFromJsonAsync<List<T>>();
                    return results ?? new List<T>();
                });
        }

        public async Task<T?> GetEntityAsync(string id)
        {
            return await ServiceOperationAsync<T?>(
                nameof(GetEntityAsync),
                async () =>
                {
                    string fullUrl = $"{_serviceUrl}/{id}";
                    var response = await _client.GetAsync(fullUrl);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadFromJsonAsync<T>();
                });
        }

        public async Task<T> CreateEntityAsync(T entity)
        {
            return await ServiceOperationAsync<T>(
                nameof(CreateEntityAsync),
                async () =>
                {
                    var response = await _client.GetAsync(_serviceUrl);
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        throw new EntityAlreadyExistsException("Id", entity.ToString() ?? string.Empty);
                    }

                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<T>();
                    return result ?? new T();
                });
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
