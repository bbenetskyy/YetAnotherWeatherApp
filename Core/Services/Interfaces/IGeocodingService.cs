using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core.Services.Interfaces
{
    public interface IGeocodingService
    {
        Task<IEnumerable<Placemark>> GetPlacemarksAsync(double latitude, double longitude);
    }
}
