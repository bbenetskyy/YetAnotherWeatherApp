using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Interfaces
{
    public interface IConnectivityService
    {
        bool IsConnected { get; }
    }
}
