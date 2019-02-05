using InteractiveAlert;
using MvvmCross;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Core.Services
{
    public interface ILocationService
    {
        Task<string> GetLocationCityNameAsync();
    }

    public class LocationService : ILocationService
    {
        public async Task<string> GetLocationCityNameAsync()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest));
                if (location == null) return null;
                var place = (await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude)).FirstOrDefault();
                return place?.Locality;
            }
            catch (Exception)
            {
                var interactiveAlerts = Mvx.IoCProvider.Resolve<IInteractiveAlerts>();
                var alertConfig = new InteractiveAlertConfig
                {
                    OkButton = new InteractiveActionButton(),
                    Title = "Warning",
                    Message = "Can't get your City name",
                    Style = InteractiveAlertStyle.Warning,
                    IsCancellable = true
                };
                interactiveAlerts.ShowAlert(alertConfig);
            }
            return null;
        }
    }
}
