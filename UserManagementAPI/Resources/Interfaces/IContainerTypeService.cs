#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IContainerTypeService
    {
        Task<DefaultAPIResponse> GetContainerTypesAsync();
        Task<DefaultAPIResponse> CreateContainerTypeAsync(ContainerTypeLookup payLoad);

    }
}
