using System.Threading.Tasks;
using OpenWeatherMap.Standard;

namespace API
{
    public interface IApiClient
    {
        Task<WeatherData> GetWeatherByCityNameAsync(string cityName);
    }
}