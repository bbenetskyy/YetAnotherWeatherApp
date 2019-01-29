using OpenWeatherMap.Standard;
using System.Threading.Tasks;

namespace API
{
    public class ApiClient : IApiClient
    {
        private readonly Forecast forecast;
        public string ApiKey { get; set; } = "adb608d1b4b3a21dc16c85c499bf535f";

        public ApiClient(IRestService service = null)
        {
            forecast = service == null
                ? new Forecast()
                : new Forecast(service);
        }

        public Task<WeatherData> GetWeatherByCityNameAsync(string cityName)
        {
            return forecast.GetWeatherDataByCityNameAsync(apiKey, cityName);
        }
    }
}
