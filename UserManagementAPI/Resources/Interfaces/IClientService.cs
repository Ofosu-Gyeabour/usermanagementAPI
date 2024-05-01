#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.website;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IClientService
    {
        Task<DefaultAPIResponse> GetClientInformationAsync(int page, int pageSize);

        Task<DefaultAPIResponse> GetCorporateClientAsync();
        Task<DefaultAPIResponse> GetIndividualClientAsync();

        Task<PagedResponseAPI> PaginationTestAsync(int pageNumber, int pageSize);

        Task<DefaultAPIResponse> GetGenericCustomerListAsync(SearchParam param);
        Task<GenericCustomerLookup> GetGenericCustomer(string acctParam);
        Task<DefaultAPIResponse> GetClientRecordAsync(SearchParam param);

        Task<DefaultAPIResponse> SaveCorporateClientRecordAsync(CorporateCustomerLookup payLoad);
        Task<DefaultAPIResponse> SaveIndividualClientRecordAsync(IndividualCustomerLookup payLoad);
        Task<DefaultAPIResponse> SaveOnlineCustomerAsync(clsCustomer payLoad);
        Task<DefaultAPIResponse> VerifyOnlineCustomerAsync(string user, string verificationCode);
        Task<DefaultAPIResponse> UpdateClientInformationAsync(IndividualCustomerLookup payLoad);
        Task<DefaultAPIResponse> UpdateClientAddressAsync(IndividualCustomerLookup payLoad);

    }
}
