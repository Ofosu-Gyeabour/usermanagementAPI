#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Response;
using UserManagementAPI.Models;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IProfileService
    {
        Task<DefaultAPIResponse> SaveProfileAsync(SystemProfile ProfilePayLoad);
        Task<DefaultAPIResponse> AmendProfileAsync(SystemProfile ProfilePayLoad);
        Task<DefaultAPIResponse> GetProfilesAsync(int _companyId);
        Task<DefaultAPIResponse> GetProfileModulesAsync(SingleParam payLoad);
        //Task<DefaultAPIResponse> DeleteProfileAsync();
    }
}
