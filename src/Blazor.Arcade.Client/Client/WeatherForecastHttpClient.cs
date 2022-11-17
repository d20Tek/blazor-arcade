using System.Net.Http.Json;
using static Blazor.Arcade.Client.Pages.FetchData;

namespace Blazor.Arcade.Client.Client
{
    public class WeatherForecastHttpClient
    {
        public HttpClient _httpClient;

        public WeatherForecastHttpClient(HttpClient client)
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
