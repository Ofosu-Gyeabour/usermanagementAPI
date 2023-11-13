#nullable disable
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IPaymentTermService
    {
        Task<DefaultAPIResponse> Get();
    }
}
