#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface ITitleService
    {
        Task<DefaultAPIResponse> GetTitlesAsync();
        Task<PaginationAPIResponse> ListTitlesAsync(int page, int pageSize);
        Task<DefaultAPIResponse> CreateTitleAsync(TitleLookup payLoad);
        Task<UploadAPIResponse> UploadTitleAsync(IEnumerable<TitleLookup> payLoad);
    }
}
