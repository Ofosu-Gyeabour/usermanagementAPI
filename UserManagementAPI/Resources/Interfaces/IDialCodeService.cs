#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IDialCodeService
    {
        Task<DefaultAPIResponse> GetDialCodesAsync();
        Task<DefaultAPIResponse> CreateDialCodeAsync(DialCodeLookup payLoad);
    }
}
