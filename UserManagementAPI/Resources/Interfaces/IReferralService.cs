#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IReferralService
    {
        Task<DefaultAPIResponse> getReferralsAsync();
        Task<DefaultAPIResponse> CreateReferralAsync(ReferralLookup payLoad);
        Task<DefaultAPIResponse> UpdateReferralAsync(ReferralLookup payLoad);

        Task<UploadAPIResponse> UploadRefferalDataAsync(IEnumerable<ReferralLookup> payLoad);
    }
}
