using ConfigurationBuilder;
using OpenWeatherMap;
using System.Net.Http;
using System.Threading.Tasks;
using API.Configuration;

namespace API
{
    public class ApiClient : IApiClient
    {
        private readonly OpenWeatherMapClient client;

        public ApiClient(string apiKey = null, HttpMessageHandler handler = null)
        {
            client = new OpenWeatherMapClient(GetApiKey(apiKey), handler);
        }

        protected string GetApiKey(string apiKey)
            => apiKey ?? new ConfigurationBuilder<ClientConfiguration>()
                   .FromResource("API.Configuration.ProductionConfig.json")
                   .AsJsonFormat()
                   .Build().ApiKey;

        public Task<CurrentWeatherResponse> GetWeatherByCityNameAsync(string cityName)
        {
            return client?.CurrentWeather?.GetByName(cityName, MetricSystem.Metric);
        }
    }
}
