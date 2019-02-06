using Core.Resources;
using Core.Services.Interfaces;
using InteractiveAlert;
using MvvmCross;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core.Services
{
    public class LocationService : ILocationService
    {
        private readonly IGeolocationService geolocation;
        private readonly IGeocodingService geocoding;

        public LocationService(
            IGeolocationService geolocation,
            IGeocodingService geocoding)
        {
            this.geolocation = geolocation;
            this.geocoding = geocoding;
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
            var interactiveAlerts = Mvx.IoCProvider.Resolve<IInteractiveAlerts>();
            var alertConfig = new InteractiveAlertConfig
            {
                OkButton = new InteractiveActionButton(),
                Title = AppResources.Warning,
                Message = AppResources.CanNotGetCityName,
                Style = InteractiveAlertStyle.Warning,
                IsCancellable = true
            };
            interactiveAlerts.ShowAlert(alertConfig);
        }
    }
}