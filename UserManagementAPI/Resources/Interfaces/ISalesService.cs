#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface ISalesService
    {
        Task<DefaultAPIResponse> GetPaymentMethodAsync();

        Task<DefaultAPIResponse> CreateSalesAsync(Sale adhocPayLoad);

        Task<DefaultAPIResponse> CreatePackageAsync(Package payLoad);

    }
}
