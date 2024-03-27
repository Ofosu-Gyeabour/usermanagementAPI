#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IScannerService
    {
        Task<DownloadAPIResponse> GenerateQRCodeAsync(string payLoad);
        Task<DownloadAPIResponse> GenerateBarCodeAsync(string payLoad, string barcodes, string itemDescriptions);

        Task<DownloadAPIResponse> GenerateOperationalBarCodeAsync(string payLoad);
        Task<DefaultAPIResponse> AddBarCodeGeneratorAsync(clsBarCodeGenerator payLoad);

        Task<PaginationAPIResponse> ListBarCodeGeneratorsAsync(int page, int pageSize);

        Task<DownloadAPIResponse> GenerateZippedBarCodeAsync(string payLoad, string barcodes, string itemDescriptions);

        Task<PaginationAPIResponse> ListWarehouseSectionsAsync(int page, int pageSize);

    }
}
