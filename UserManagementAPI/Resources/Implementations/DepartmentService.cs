using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
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
    
        public async Task<DefaultAPIResponse> CreateDepartmentAsync(DepartmentLookup payLoad)
        {
            //creates department record in data store
            DefaultAPIResponse response = null;

            try
            {
                var dObj = await _context.TDepartments.Where(d => d.DepartmentName == payLoad.nameOfdepartment.Trim()).FirstOrDefaultAsync();
                if (dObj == null)
                {
                    var companyObj = await _context.Tcompanies.Where(c => c.Company == payLoad.oCompany.nameOfcompany.Trim()).FirstOrDefaultAsync();
                    if (companyObj != null)
                    {
                        TDepartment department = new TDepartment()
                        {
                            DepartmentName = payLoad.nameOfdepartment.Trim().ToUpper(),
                            CompanyId = companyObj.CompanyId,
                            Describ = payLoad.departmentDescription
                        };

                        await _context.AddAsync(department);
                        await _context.SaveChangesAsync();

                        response = new DefaultAPIResponse()
                        {
                            status = true,
                            message = $"Department {payLoad.nameOfdepartment.Trim().ToUpper()} added successfully to data store",
                            data = payLoad
                        };
                    }
                    else { response = new DefaultAPIResponse() { status = false, message = @"Id of company cannot be zero (0)" }; }
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"Department already exist in datastore"}; }

                return response;
            }
            catch(Exception ex)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {ex.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> UpdateDepartmentAsync(DepartmentLookup payLoad)
        {
            //updates a department record
            DefaultAPIResponse response = null;
            string previous_department_name = string.Empty;

            try
            {
                //get department using Id
                var departmentObj = await _context.TDepartments.Where(d => d.Id == payLoad.id).FirstOrDefaultAsync();
                if (departmentObj != null)
                {
                    previous_department_name = departmentObj.DepartmentName;

                    departmentObj.DepartmentName = payLoad.nameOfdepartment.Trim().ToUpper();
                    departmentObj.Describ = payLoad.departmentDescription.Trim();

                    await _context.SaveChangesAsync();

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Department {previous_department_name} updated to {payLoad.nameOfdepartment.Trim().ToUpper()} in the datastore",
                        data = payLoad
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"Department cannot have an Id of zero(0)" }; }

                return response;
            }
            catch(Exception ex)
            {
                response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {ex.Message}"
                };

                return response;
            }
        }

    }
}
