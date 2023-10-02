#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface ISealService
    {
        Task<DefaultAPIResponse> GetSealTypeListAsync();
        Task<DefaultAPIResponse> GetSealPriceListAsync();

        Task<DefaultAPIResponse> CreateSealTypeAsync(SealTypeLookup payLoad);
        Task<DefaultAPIResponse> CreateSealPriceAsync(SealPriceLookup payLoad);

    }
}
