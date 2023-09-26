#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Response;
using UserManagementAPI.Models;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IModuleService
    {
        Task<DefaultAPIResponse> GetModulesAsync();
        Task<DefaultAPIResponse> GetModulesInUseAsync();
    }
}
