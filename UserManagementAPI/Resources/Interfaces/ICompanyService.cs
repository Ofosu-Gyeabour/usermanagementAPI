using Microsoft.EntityFrameworkCore;
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface ICompanyService
    {
        Task<DefaultAPIResponse> GetCompanyTypesAsync();
        Task<DefaultAPIResponse> CreateCompanyTypeAsync(CompanyTypeLookup payLoad);
        Task<DefaultAPIResponse> UpdateCompanyTypeAsync(CompanyTypeLookup payLoad);
    }
}
