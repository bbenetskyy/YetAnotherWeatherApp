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

        public ApiClient(HttpMessageHandler handler = null)
        {
            client = new OpenWeatherMapClient(GetApiKey(), handler);
        }

        protected string GetApiKey()
            => new ConfigurationBuilder<ClientConfiguration>()
                   .FromResource("API.Configuration.ProductionConfig.json")
                   .AsJsonFormat()
                   .Build().ApiKey;

        public Task<CurrentWeatherResponse> GetWeatherByCityNameAsync(string cityName)
        {
            return client?.CurrentWeather?.GetByName(cityName, MetricSystem.Metric);
        }
    }
}
