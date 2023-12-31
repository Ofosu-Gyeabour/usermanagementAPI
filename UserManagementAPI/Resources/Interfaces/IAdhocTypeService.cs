﻿#nullable disable

using UserManagementAPI.POCOs;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Interfaces
{
    public interface IAdhocTypeService
    {
        Task<DefaultAPIResponse> GetAdHocTypesAsync();
        Task<DefaultAPIResponse> CreateAdhocTypeAsync(AdhocTypeLookup payLoad);

        Task<UploadAPIResponse> UploadAdhocTypeDataAsync(IEnumerable<AdhocTypeLookup> payLoad);

        Task<DefaultAPIResponse> GetAdhoc(SingleParam param);

        Task<DefaultAPIResponse> ComputeOrderSummary(OrderSummaryParameter param);
    }
}
