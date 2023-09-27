#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IBranchService
    {
        Task<DefaultAPIResponse> GetBranchesAsync();
        Task<DefaultAPIResponse> GetBranchAsync();
        Task<DefaultAPIResponse> CreateBranchAsync(BranchLookup payLoad);
    }
}
