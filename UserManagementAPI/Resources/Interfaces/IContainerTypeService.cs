#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Procs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IContainerTypeService
    {
        Task<DefaultAPIResponse> GetContainerTypesAsync();
        Task<DefaultAPIResponse> CreateContainerTypeAsync(ContainerTypeLookup payLoad);
        Task<UploadAPIResponse> UploadContainerTypeDataAsync(IEnumerable<ContainerTypeLookup> payLoad);

        Task<DefaultAPIResponse> SaveContainerRecordAsync(clsShippingContainer payLoad);
        Task<DefaultAPIResponse> GetContainerLoadingStatusAsync();

        Task<PaginationAPIResponse> GetContainersToLoadListAsync(int page, int pageSize);
        Task<PaginationAPIResponse> GetUnloadedContainerItemsListAsync(int page, int pageSize);

        Task<DefaultAPIResponse> InitiateContainerLoadingAsync(string containerAttribute);

        Task<DefaultAPIResponse> LoadContainerItemAsync(LoadedItem payLoad);
        Task<DefaultAPIResponse> GetContainerItemsAsync(string containerAttribute);
        Task<DefaultAPIResponse> RemoveContainerItemAsync(GenericLookup containerPayLoad);
        Task<DefaultAPIResponse> GetContainerDocLookup();
        Task<DefaultAPIResponse> GetContainerDocLookup(int containerId);
    }
}
