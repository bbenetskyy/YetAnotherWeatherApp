using Core.Services.Interfaces;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core.Services
{
    public class GeolocationService : IGeolocationService
    {
        public Task<Location> GetLocationAsync(GeolocationRequest request)
        {
            return Geolocation.GetLocationAsync(request);
        }
    }
}
