#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IDataService
    {
        Task<DefaultAPIResponse> getCustomerShippingOrder(int customerID, int page, int pageSize, DateTime df, DateTime dt);
    }
}
