using UserManagementAPI.Response;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IUserService
    {
        Task<DefaultAPIResponse> GetUsersAsync();
        Task<UserAPIResponse> GetUserAsync(UserInfo userCredential);
        Task<DefaultAPIResponse> GetMD5EncryptedPasswordAsync(SingleParam singleParam);

        Task<DefaultAPIResponse> SetLoggedFlagAsync(UserInfo _usr);

        Task<DefaultAPIResponse> SetLoggedOutFlagAsync(UserInfo _usr);

        Task<DefaultAPIResponse> GetUserProfileAsync(UserInfo _usr);
        Task<DefaultAPIResponse> AmendUserProfileAsync(UserProfile payLoad);

        Task<DefaultAPIResponse> CreateUserAsync(userRecord payLoad);
    }
}
