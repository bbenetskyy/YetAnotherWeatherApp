using OpenWeatherMap.Standard;
using System.Threading.Tasks;

namespace API
{
    public interface IApiClient
    {
        Task<WeatherData> GetWeatherByCityNameAsync(string cityName);
    }
    public class ApiClient : IApiClient
    {
        private readonly string apiKey = "adb608d1b4b3a21dc16c85c499bf535f";
        private readonly Forecast forecast = new Forecast();

        public Task<WeatherData> GetWeatherByCityNameAsync(string cityName)
        {
            return forecast.GetWeatherDataByCityNameAsync(apiKey, cityName);
        }
    }
}
