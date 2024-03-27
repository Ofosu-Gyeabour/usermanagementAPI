#nullable disable
using Newtonsoft.Json;
using System.Diagnostics;
using UserManagementAPI.POCOs;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;

namespace UserManagementAPI.Resources.Implementations
{
    public class ScannerService : IScannerService
    {
        public ScannerService()
        {
            
        }

        public async Task<DownloadAPIResponse> GenerateQRCodeAsync(string payLoad)
        {
            //TODO: generate qr code data
            DownloadAPIResponse rsp = null;

            try
            {
                clsCollectionAndDelivery cnd = new clsCollectionAndDelivery();
                var cndData = await cnd.getCollectionRecordAsync(payLoad);
                cndData.orderItems = await cnd.getCollectionOrderItemsAsync(cndData.orderId);
                string st = JsonConvert.SerializeObject(cndData);

                clsQRCode q = new clsQRCode();
                var bitmapBytes = await q.returnQRCode(st);

                rsp = new DownloadAPIResponse() { 
                    status = bitmapBytes.Length > 0 ? true: false,
                    message = bitmapBytes.Length > 0 ? @"success": @"failed",
                    data =  bitmapBytes
                };

                return rsp;
            }
            catch(Exception ex)
            {
                return rsp = new DownloadAPIResponse() { 
                    status = false,
                    message = $"error: {ex.Message}"
                };
            }
        }

        public async Task<DownloadAPIResponse> GenerateBarCodeAsync(string payLoad)
        {
            //TODO: generate bar code data
            DownloadAPIResponse rsp = null;

            try
            {
                clsCollectionAndDelivery cnd = new clsCollectionAndDelivery();
                var cndData = payLoad.Substring(0, 3).Trim() == @"PCO" ? await cnd.getDeliveryRecordAsync(payLoad) : await cnd.getCollectionRecordAsync(payLoad);
                cndData.orderItems = payLoad.Substring(0, 3) == @"PCO" ? await cnd.getDeliveryOrderItemsAsync(cndData.orderId) : await cnd.getCollectionOrderItemsAsync(cndData.orderId);
                string st = cndData.orderItems.ToString();

                clsBarCode b = new clsBarCode();
                var pngBytes = await b.returnBarCodeAsync(cndData.orderItems,cndData.name);

                rsp = new DownloadAPIResponse()
                {
                    status = pngBytes.Length > 0 ? true: false,
                    message = pngBytes.Length > 0 ? @"success": @"failed",
                    data = pngBytes
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DownloadAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DownloadAPIResponse> GenerateOperationalBarCodeAsync(string payLoad)
        {
            //TODO: generates operational barcode. barcode initiates operation on software
            DownloadAPIResponse rsp = null;

            try
            {
                clsBarCode b = new clsBarCode();
                var pngBytes = await b.returnBarCodeAsync(payLoad, payLoad);

                rsp = new DownloadAPIResponse()
                {
                    status = pngBytes.Length > 0 ? true : false,
                    message = pngBytes.Length > 0 ? @"success" : @"failed",
                    data = pngBytes
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DownloadAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> AddBarCodeGeneratorAsync(clsBarCodeGenerator payLoad)
        {
            DefaultAPIResponse rsp = null;

            try
            {
                clsBarCodeGenerator obj = new clsBarCodeGenerator()
                {
                    barcodeId = payLoad.barcodeId,
                    generatedBarCode = payLoad.generatedBarCode,
                    generatedDate = payLoad.generatedDate
                };

                bool bn = await obj.AddAsync();

                return rsp = new DefaultAPIResponse()
                {
                    status = bn,
                    message = bn ? @"success": @"failed",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<PaginationAPIResponse> ListBarCodeGeneratorsAsync(int page, int pageSize)
        {
            //TODO: list bar code generators
            PaginationAPIResponse rsp = null;

            try
            {
                clsBarCodeGenerator obj = new clsBarCodeGenerator();
                var blist = await obj.ListAsync();

                int totalCount = blist.ToList().Count();
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                rsp = new PaginationAPIResponse() {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? $"Page {page} of {totalPages}": @"failed",
                    total = totalCount,
                    data = blist.ToList()
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList()
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new PaginationAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
