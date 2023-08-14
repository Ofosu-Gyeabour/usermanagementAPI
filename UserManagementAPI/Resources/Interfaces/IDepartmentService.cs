using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IDepartmentService
    {
        Task<DefaultAPIResponse> GetDepartmentsAsync();
        //Task<DefaultAPIResponse> GetDepartmentAsync(int _id);
    }
}
