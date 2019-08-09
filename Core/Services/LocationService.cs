using Core.Resources;
using Core.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using MvvmCross.Logging;
using Xamarin.Essentials;

namespace Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly TimeSpan timeout = TimeSpan.FromSeconds(5);
        private readonly IMvxLog logger;
        private readonly IGeolocationService geolocation;
        private readonly IGeocodingService geocoding;

        public LocationService(
            IMvxLog logger,
            IGeolocationService geolocation,
            IGeocodingService geocoding)
        {
            this.logger = logger;
            this.geolocation = geolocation;
            this.geocoding = geocoding;
        }

        public async Task<string> GetLocationCityNameAsync()
        {
            try
            {
                var location = await geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest, timeout));
                var place = (await geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude))?.First();
                return place.Locality;
            }
            catch (Exception ex)
            {
                logger.Log(MvxLogLevel.Warn, () => ex.Message, ex);
                throw new LocationException(AppResources.CanNotGetCityName, ex);
            }
        }
    }
}