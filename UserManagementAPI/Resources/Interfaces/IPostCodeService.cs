#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IPostCodeService
    {
        Task<DefaultAPIResponse> GetAddressesAsync(SingleParam payLoad);
        Task<DefaultAPIResponse> getPostCodeCongestionChargeAsync(SingleParam payLoad);
    }
}
