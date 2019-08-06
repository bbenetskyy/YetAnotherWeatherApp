using Core.Models;

namespace Core.Services.Interfaces
{
    public interface IAlertService
    {
        void Show(string message, AlertType type);
    }
}