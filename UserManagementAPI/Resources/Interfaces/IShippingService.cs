#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IShippingService
    {
        #region Shipping-Lines
        Task<DefaultAPIResponse> GetShippingLineListAsync();
        Task<DefaultAPIResponse> CreateShippingLineAsync(ShippingLineLookup payLoad);

        Task<UploadAPIResponse> UploadShippingLineAsync(IEnumerable<ShippingLineLookup> payLoad);
        #endregion

        Task<DefaultAPIResponse> CreateStockControlAsync(StockControlLookup payLoad);
        DefaultAPIResponse GetStockControlListAsync();

        #region Shipping-Vessel
        DefaultAPIResponse GetShippingVesselListAsync();
        Task<DefaultAPIResponse> CreateShippingVesselAsync(VesselLookup payLoad);

        Task<UploadAPIResponse> UploadShippingVesselAsync(IEnumerable<VesselLookup> payLoad);

        #endregion

        #region Shipping-Method
        Task<DefaultAPIResponse> GetShippingMethodListAsync();
        Task<DefaultAPIResponse> CreateShippingMethodAsync(ShippingMethodLookup payLoad);
        Task<UploadAPIResponse> UploadShippingMethodDataAsync(IEnumerable<ShippingMethodLookup> payLoad);

        #endregion

        #region Shipper-Category
        Task<DefaultAPIResponse> GetShipperCategoryListAsync();
        Task<DefaultAPIResponse> CreateShipperCategoryAsync(ShipperCategoryLookup payLoad);
        Task<UploadAPIResponse> UploadShipperCategoryAsync(IEnumerable<ShipperCategoryLookup> payLoad);
        #endregion

        #region Delivery-Method
        Task<DefaultAPIResponse> GetDeliveryMethodListAsync();
        Task<DefaultAPIResponse> CreateDeliveryMethodAsync(DeliveryMethodLookup payLoad);

        Task<UploadAPIResponse> UploadDeliveryMethodAsync(IEnumerable<DeliveryMethodLookup> payLoad);
        #endregion

        #region Delivery - Zone
        DefaultAPIResponse GetDeliveryZoneListAsync();
        Task<DefaultAPIResponse> CreateDeliveryZoneAsync(DeliveryZoneLookup payLoad);
        Task<UploadAPIResponse> UploadDeliveryZoneAsync(IEnumerable<DeliveryZoneLookup> payLoad);

        #endregion

        #region HS-Codes
        Task<DefaultAPIResponse> GetHSCodeListAsync();
        Task<DefaultAPIResponse> CreateHSCodAsync(HSCodeLookup payLoad);
        Task<UploadAPIResponse> UploadHSCodesAsync(IEnumerable<HSCodeLookup> payLoad);
        #endregion
        /*
        #region Insurance-Type
        Task<DefaultAPIResponse> GetInsuranceTypeListAsync();
        Task<DefaultAPIResponse> CreateInsuranceTypeAsync(InsuranceTypeLookup payLoad);
        Task<UploadAPIResponse> UploadInsuranceTypeAsync(IEnumerable<InsuranceTypeLookup> payLoad);
        #endregion
        */
        
        #region Insurance
        DefaultAPIResponse GetInsuranceListAsync();
        Task<DefaultAPIResponse> CreateInsuranceAsync(InsuranceLookup payLoad);
        Task<UploadAPIResponse> UploadInsuranceAsync(IEnumerable<InsuranceLookup> payLoad);
        #endregion

        #region Sailing-Schedule
        DefaultAPIResponse GetSailingScheduleListAsync();
        Task<DefaultAPIResponse> CreateSailingScheduleAsync(SailingScheduleLookup payLoad);

        Task<UploadAPIResponse> UploadSailingScheduleAsync(IEnumerable<SailingScheduleLookup> payLoad);

        #endregion

    }
}
