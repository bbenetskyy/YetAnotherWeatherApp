using Core.Services.Interfaces;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core.Services
{
    public class Geolocation : IGeolocationService
    {
        public Task<Location> GetLocationAsync(GeolocationRequest request)
        {
            return Xamarin.Essentials.Geolocation.GetLocationAsync(request);
        }
    }
}
