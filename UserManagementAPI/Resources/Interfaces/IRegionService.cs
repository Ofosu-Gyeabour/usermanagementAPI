#nullable disable
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IRegionService
    {
        Task<DefaultAPIResponse> GetRegionAsync();
        
        Task<DefaultAPIResponse> CreateRegionAsync(RegionLookup payLoad);
    }
}
