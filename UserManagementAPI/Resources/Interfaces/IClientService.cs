#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IClientService
    {
        Task<DefaultAPIResponse> GetClientInformationAsync();

        Task<DefaultAPIResponse> GetCorporateClientAsync();
        Task<DefaultAPIResponse> GetIndividualClientAsync();

        Task<PagedResponseAPI> PaginationTestAsync(int pageNumber, int pageSize);

        Task<DefaultAPIResponse> GetGenericCustomerListAsync(SearchParam param);

        Task<DefaultAPIResponse> GetClientRecordAsync(SearchParam param);
    }
}
