using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface ILocationService
    {
        Task<string> GetLocationCityNameAsync();
    }
}
