#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IShippingPortService
    {
        Task<DefaultAPIResponse> GetShippingPortsAsync();
        Task<DefaultAPIResponse> CreateShippingPortAsync(ShippingPortLookup payLoad);
    }
}
