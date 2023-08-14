using UserManagementAPI.Response;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IUserService
    {
        Task<DefaultAPIResponse> GetUsersAsync();

        Task<DefaultAPIResponse> GetUserAsync(UserInfo userCredential);
        Task<DefaultAPIResponse> GetMD5EncryptedPasswordAsync(SingleParam singleParam);
    }
}
