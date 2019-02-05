using OpenWeatherMap;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IAlertService
    {
        Task<CurrentWeatherResponse> GetWeather(string cityName, string errorMessage);
    }
}
