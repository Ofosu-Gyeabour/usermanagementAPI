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
    }
}
