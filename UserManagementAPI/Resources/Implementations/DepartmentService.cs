using Microsoft.EntityFrameworkCore;
using UserManagementAPI.POCOs;
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
            List<departmentLookup> dta = null;

            try
            {
                departments = await _context.TDepartments.Include(c => c.Company).ToListAsync();

                if (departments.Count() > 0)
                {
                    dta = new List<departmentLookup>();
                    foreach(var d in departments)
                    {
                        var obj = new departmentLookup()
                        {
                            id = d.Id,
                            name = d.DepartmentName,
                            describ = d.Describ,
                            companyName = d.Company.Company.ToString()
                        };

                        dta.Add(obj);
                    }
                }
                return new DefaultAPIResponse() {
                    status = true,
                    message = @"successful",
                    data = dta
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
