#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IAdhocTypeService
    {
        Task<DefaultAPIResponse> GetAdHocTypesAsync();
        Task<DefaultAPIResponse> CreateAdhocTypeAsync(AdhocTypeLookup payLoad);
    }
}
