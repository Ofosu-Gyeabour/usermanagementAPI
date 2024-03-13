#nullable disable

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IXeroService
    {
        Task<XeroAPIResponse> CreateContactAsync(clsXeroContact payLoad);
        Task<XeroAPIResponse> createXeroInvoiceAsync(clsXeroInvoice payLoad);

        #region SDK

        Task<XeroAPIResponse> CreateContactSDKAsync(clsXeroContact payLoad);

        #endregion
    }
}
