#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IPackagingService
    {
        Task<DefaultAPIResponse> GetPackageItemListAsync();
        Task<DefaultAPIResponse> GetPackagingItemAndPriceListAsync(CompanyLookup payLoad);
        Task<DefaultAPIResponse> GetPackagePriceListAsync();
        //Task<DefaultAPIResponse> GetCompanyPackagingPriceListAsync(CompanyLookup payLoad);

        Task<DefaultAPIResponse> CreatePackageItemAsync(PackageItemLookup payLoad);
        Task<DefaultAPIResponse> CreatePackagingPriceAsync(PackagepriceLookup payLoad);

        Task<UploadAPIResponse> UploadPackageItemAsync(IEnumerable<PackageItemLookup> payLoad);
        Task<UploadAPIResponse> UploadPackagingPriceAsync(IEnumerable<PackagepriceLookup> payLoad);

        Task<DefaultAPIResponse> GetPackagePriceRecord(int companyId, int itemID);
    }
}
