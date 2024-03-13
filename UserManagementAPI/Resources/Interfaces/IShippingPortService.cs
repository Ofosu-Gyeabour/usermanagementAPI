#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IShippingPortService
    {
        DefaultAPIResponse GetShippingPortsAsync();
        Task<DefaultAPIResponse> CreateShippingPortAsync(ShippingPortLookup payLoad);
        Task<UploadAPIResponse> UploadShippingPortAsync(IEnumerable<ShippingPortLookup> payLoad);

        Task<DefaultAPIResponse> GetCountryShippingPortAsync(CountryLookup payLoad);

        Task<DefaultAPIResponse> GetShippingPortRecordAsync(SingleParam payLoad);

        Task<PaginationAPIResponse> GetShippingPortPageAsync(int page, int pageSize);

    }
}
