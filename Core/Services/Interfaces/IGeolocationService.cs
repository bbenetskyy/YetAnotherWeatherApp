using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core.Services.Interfaces
{
    public interface IGeolocationService
    {
        Task<Location> GetLocationAsync(GeolocationRequest request);
    }
}
