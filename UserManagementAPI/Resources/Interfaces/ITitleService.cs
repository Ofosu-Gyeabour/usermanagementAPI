#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface ITitleService
    {
        Task<DefaultAPIResponse> GetTitlesAsync();
        Task<DefaultAPIResponse> CreateTitleAsync(TitleLookup payLoad);

    }
}
