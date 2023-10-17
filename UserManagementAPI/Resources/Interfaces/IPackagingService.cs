﻿#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IPackagingService
    {
        Task<DefaultAPIResponse> GetPackageItemListAsync();
        Task<DefaultAPIResponse> GetPackagePriceListAsync();

        Task<DefaultAPIResponse> CreatePackageItemAsync(PackageItemLookup payLoad);
        Task<DefaultAPIResponse> CreatePackagingPriceAsync(PackagepriceLookup payLoad);

        Task<UploadAPIResponse> UploadPackageItemAsync(IEnumerable<PackageItemLookup> payLoad);
        Task<UploadAPIResponse> UploadPackagingPriceAsync(IEnumerable<PackagepriceLookup> payLoad);
    }
}
