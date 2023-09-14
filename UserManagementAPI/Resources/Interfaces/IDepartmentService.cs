using Microsoft.EntityFrameworkCore;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IDepartmentService
    {
        Task<DefaultAPIResponse> GetDepartmentsAsync();
        //Task<DefaultAPIResponse> GetDepartmentAsync(int _id);
        Task<DefaultAPIResponse> CreateDepartmentAsync(DepartmentLookup payLoad);
        Task<DefaultAPIResponse> UpdateDepartmentAsync(DepartmentLookup payLoad);

    }
}
