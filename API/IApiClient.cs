using OpenWeatherMap;
using System.Threading.Tasks;

namespace API
{
    public interface IApiClient
    {
        Task<CurrentWeatherResponse> GetWeatherByCityNameAsync(string cityName);
    }
}