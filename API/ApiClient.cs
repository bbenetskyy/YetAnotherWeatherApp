using OpenWeatherMap;
using System.Net.Http;
using System.Threading.Tasks;

namespace API
{
    public class ApiClient : IApiClient
    {
        private readonly OpenWeatherMapClient client;

        public ApiClient(string apiKey = null, HttpMessageHandler handler = null)
        {
            client = new OpenWeatherMapClient(apiKey ?? "adb608d1b4b3a21dc16c85c499bf535f",
                handler);
        }

        public Task<CurrentWeatherResponse> GetWeatherByCityNameAsync(string cityName)
        {
            return client?.CurrentWeather?.GetByName(cityName, MetricSystem.Metric);
        }
    }
}
