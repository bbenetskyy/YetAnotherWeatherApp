using OpenWeatherMap.Standard;
using System.Threading.Tasks;

namespace API
{
    public interface IApiClient
    {
        string ApiKey { get; set; }
        Task<WeatherData> GetWeatherByCityNameAsync(string cityName);
    }
}