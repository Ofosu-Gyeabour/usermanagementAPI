#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IAirportService
    {
        Task<DefaultAPIResponse> GetAirportsAsync();
        Task<DefaultAPIResponse> CreateAirportAsync(AirportLookup payLoad);
    }
}
