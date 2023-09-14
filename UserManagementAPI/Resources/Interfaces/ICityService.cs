#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface ICityService
    {
        Task<DefaultAPIResponse> CreateCityAsync(CityLookup payLoad);
        Task<DefaultAPIResponse> UpdateCityAsync(CityLookup payLoad);
        Task<DefaultAPIResponse> UploadCityDataAsync(IEnumerable<CityLookup> payLoad);

        Task<DefaultAPIResponse> UpdateCountryOfCityAsync(CityLookup payLoad);
    }
}
