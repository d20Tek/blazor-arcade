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
            return await ServiceOperationAsync<IList<T>>(
                nameof(GetEntitiesAsync),
                async () =>
                {
                    var response = await _client.GetAsync(_serviceUrl);
                    response.EnsureSuccessStatusCode();

                    var results = await response.Content.ReadFromJsonAsync<List<T>>();
                    return results.ListOrDefault();
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
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }

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
                    using var content = JsonContent.Create<T>(entity, options: _options);

                    var response = await _client.PostAsync(_serviceUrl, content);
                    if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        throw new EntityAlreadyExistsException("Id", entity.ToString().ValueOrDefault());
                    }

                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<T>();
                    return result.ObjectOrDefault();
                });
        }

        public async Task<T> UpdateEntityAsync(string id, T entity)
        {
            return await ServiceOperationAsync<T>(
                nameof(UpdateEntityAsync),
                async () =>
                {
                    string fullUrl = $"{_serviceUrl}/{id}";
                    using var content = JsonContent.Create<T>(entity, options: _options);

                    var response = await _client.PutAsync(fullUrl, content);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new EntityNotFoundException("Id", id);
                    }

                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadFromJsonAsync<T>();
                    return result.ObjectOrDefault();
                });
        }

        public async Task DeleteEntityAsync(string id)
        {
            await ServiceOperationAsync<bool>(
                nameof(DeleteEntityAsync),
                async () =>
                {
                    string fullUrl = $"{_serviceUrl}/{id}";

                    var response = await _client.DeleteAsync(fullUrl);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new EntityNotFoundException("Id", id);
                    }

                    response.EnsureSuccessStatusCode();
                    return true;
                });
        }
    }
}
