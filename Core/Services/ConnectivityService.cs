using Core.Services.Interfaces;
using Xamarin.Essentials;

namespace Core.Services
{
    public class ConnectivityService : IConnectivityService
    {
        public bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
    }
}