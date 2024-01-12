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

        Task<DefaultAPIResponse> getOrderSummaryKeysAsync(OrderTypeLookup payLoad);

        Task<DefaultAPIResponse> updateAccountKeysAsync(OrderStat payLoad);

        Task<DefaultAPIResponse> getShippingItemsAsync();
        Task<DefaultAPIResponse> getShippingOrderItemsAsync();
        Task<DefaultAPIResponse> getConsigneesAsync(consigneeParam payLoad);

        Task<DefaultAPIResponse> getTotalParishesAsync();
        Task<DefaultAPIResponse> getZoneFromParishAsync(clsParish payLoad);
        Task<DefaultAPIResponse> calculateFreightCostAsync(clsFreightInput payLoad);
        Task<DefaultAPIResponse> getRateListAsync();

        Task<DefaultAPIResponse> createRecordAsync(clsShippingOrder payLoad);

    }
}
