using OpenWeatherMap;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<CurrentWeatherResponse> GetWeatherAsync(string cityName);
    }
}
