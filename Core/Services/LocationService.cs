using Core.Resources;
using Core.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using Xamarin.Essentials;

namespace Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly IGeolocationService geolocation;
        private readonly IGeocodingService geocoding;
        private readonly IAlertService alertService;

        public LocationService(
            IGeolocationService geolocation,
            IGeocodingService geocoding,
            IAlertService alertService)
        {
            this.geolocation = geolocation;
            this.geocoding = geocoding;
            this.alertService = alertService;
        }

        public async Task<string> GetLocationCityNameAsync()
        {
            try
            {
                var location = await geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest));
                if (location == null)
                {
                    ShowAlert();
                    return null;
                }

                var place = (await geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude))?.FirstOrDefault();
                if (string.IsNullOrEmpty(place?.Locality))
                    ShowAlert();
                return place?.Locality;
            }
            catch (Exception)
            {
                ShowAlert();
                return null;
            }
        }

        private void ShowAlert()
        {
            alertService.Show(AppResources.CanNotGetCityName, AlertType.Warning);
        }
    }
}