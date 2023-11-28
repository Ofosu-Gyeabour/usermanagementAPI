#nullable disable
using UserManagementAPI.Resources;
using UserManagementAPI.Response;
using UserManagementAPI.POCOs;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IUtilityService
    {
        Task<DefaultAPIResponse> createChargeAsync(ChargeEngineLookup payLoad);

        Task<DefaultAPIResponse> getChargeEngineAsync(OrderTypeLookup payLoad);

        Task<DefaultAPIResponse> getAllChargesAsync();

        Task<DefaultAPIResponse> amendChargeListAsync(ChargeEngineLookup payLoad);

        Task<DefaultAPIResponse> getOrderTypeAsync();

        Task<DefaultAPIResponse> getChargeEngineLinesAsync(OrderTypeLookup payLoad);

        Task<DefaultAPIResponse> getSalesTypeAsync();

        Task<DefaultAPIResponse> getDeliveryTimeAsync();

        Task<DefaultAPIResponse> addChargeOrTaxAsync(ChargeLookup payLoad);
        Task<DefaultAPIResponse> getChargeOrTaxListAsync();
    }
}
