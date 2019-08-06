using Core.Models;
using Core.Resources;
using Core.Services.Interfaces;
using InteractiveAlert;
using MvvmCross;

namespace Core.Services
{
    public class AlertService : IAlertService
    {
        public void Show(string message, AlertType type)
        {
            var interactiveAlerts = Mvx.IoCProvider.Resolve<IInteractiveAlerts>();
            var alertConfig = new InteractiveAlertConfig
            {
                OkButton = new InteractiveActionButton(),
                Title = type == AlertType.Warning ? AppResources.Warning : AppResources.Error,
                Message = message,
                Style = type == AlertType.Warning ? InteractiveAlertStyle.Warning : InteractiveAlertStyle.Error,
                IsCancellable = true
            };
            interactiveAlerts.ShowAlert(alertConfig);
        }
    }
}