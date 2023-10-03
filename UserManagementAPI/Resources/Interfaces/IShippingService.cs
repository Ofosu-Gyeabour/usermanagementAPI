#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IShippingService
    {
        Task<DefaultAPIResponse> GetShippingLineListAsync();
        Task<DefaultAPIResponse> CreateShippingLineAsync(ShippingLineLookup payLoad);

        Task<DefaultAPIResponse> CreateStockControlAsync(StockControlLookup payLoad);
        DefaultAPIResponse GetStockControlListAsync();

        DefaultAPIResponse GetShippingVesselListAsync();
        Task<DefaultAPIResponse> CreateShippingVesselAsync(VesselLookup payLoad);

        Task<DefaultAPIResponse> GetShippingMethodListAsync();
        Task<DefaultAPIResponse> CreateShippingMethodAsync(ShippingMethodLookup payLoad);

        Task<DefaultAPIResponse> GetShipperCategoryListAsync();
        Task<DefaultAPIResponse> CreateShipperCategoryAsync(ShipperCategoryLookup payLoad);

        Task<DefaultAPIResponse> GetDeliveryMethodListAsync();
        Task<DefaultAPIResponse> CreateDeliveryMethodAsync(DeliveryMethodLookup payLoad);

        DefaultAPIResponse GetDeliveryZoneListAsync();
        Task<DefaultAPIResponse> CreateDeliveryZoneAsync(DeliveryZoneLookup payLoad);

        Task<DefaultAPIResponse> GetHSCodeListAsync();
        Task<DefaultAPIResponse> CreateHSCodAsync(HSCodeLookup payLoad);

        Task<DefaultAPIResponse> GetInsuranceTypeListAsync();
        Task<DefaultAPIResponse> CreateInsuranceTypeAsync(InsuranceTypeLookup payLoad);

        DefaultAPIResponse GetInsuranceListAsync();
        Task<DefaultAPIResponse> CreateInsuranceAsync(InsuranceLookup payLoad);

        DefaultAPIResponse GetSailingScheduleListAsync();
        Task<DefaultAPIResponse> CreateSailingScheduleAsync(SailingScheduleLookup payLoad);

        Task<UploadAPIResponse> UploadSailingScheduleAsync(IEnumerable<SailingScheduleLookup> payLoad);

    }
}
