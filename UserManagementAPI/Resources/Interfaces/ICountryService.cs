#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface ICountryService
    {
        Task<DefaultAPIResponse> GetCountriesAsync();
        Task<DefaultAPIResponse> CreateCountryAsync(CountryLookup payLoad);
        Task<DefaultAPIResponse> UpdateCountryAsync(CountryLookup payLoad);

        Task<UploadAPIResponse> UploadCountryAsync(IEnumerable<CountryLookup> payLoad);
    }
}
