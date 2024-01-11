#nullable disable

using UserManagementAPI.Response;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IConsolidatorService
    {
        Task<DefaultAPIResponse> AuthenticatorConsolidatorAsync(UserInfo payLoad);

        Task<DefaultAPIResponse> CreateConsolidatorUserAccountAsync(consolUserRecord payLoad);

        Task<DefaultAPIResponse> ResetUserAccountAsync(consolUserRecord payLoad);

        Task<DefaultAPIResponse> ListConsolidatorUsersAsync(consolUserRecord payLoad);

        Task<DefaultAPIResponse> GetCustomersOfClientAsync(consolUserRecord payLoad, string flag = @"*");

        Task<DefaultAPIResponse> CreateIndividualCustomerAsync(IndividualConsolidatorClient payLoad);
        Task<DefaultAPIResponse> CreateCorporateCustomerAsync(CorporateConsolidatorClient payLoad);

        Task<DefaultAPIResponse> CreateConsolidatorOrderAsync(clsConsolidatorOrder payLoad);

    }
}
