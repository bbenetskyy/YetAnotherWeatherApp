using Core.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core.Services
{
    public class GeocodingService : IGeocodingService
    {
        public Task<IEnumerable<Placemark>> GetPlacemarksAsync(double latitude, double longitude)
        {
            return Xamarin.Essentials.Geocoding.GetPlacemarksAsync(latitude, longitude);
        }
    }
}