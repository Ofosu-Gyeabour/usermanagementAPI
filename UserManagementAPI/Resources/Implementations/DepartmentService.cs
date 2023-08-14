using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        swContext _context;

        public DepartmentService()
        {
            _context = new swContext();
        }
        public async Task<DefaultAPIResponse> GetDepartmentsAsync()
        {
            DefaultAPIResponse result;
            List<TDepartment> departments = null;

            try
            {
                departments = await _context.TDepartments.ToListAsync();

                return new DefaultAPIResponse() {
                    status = true,
                    message = @"successful",
                    data = (object)departments
                };
            }
            catch(Exception ex)
            {
                result = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"{ex.Message}"
                };

                return result;
            }
        }
    }
}
