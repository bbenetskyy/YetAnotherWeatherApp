using OpenWeatherMap;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IAlertService
    {
        Task<CurrentWeatherResponse> GetWeatherAsync(string cityName, string errorMessage);
        bool IsInternetConnection();
    }
}
