﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.Net.Http.Json;
using static Blazor.Arcade.Client.Pages.FetchData;

namespace Blazor.Arcade.Client.Services
{
    public class ArcadeClientService
    {
        public HttpClient _httpClient;

        public ArcadeClientService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<WeatherForecast[]> GetWeatherForecastsAsync()
        {
            var response = await _httpClient.GetAsync("/WeatherForecast");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<WeatherForecast[]>();
        }
    }
}