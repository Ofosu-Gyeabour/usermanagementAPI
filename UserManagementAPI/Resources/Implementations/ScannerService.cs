#nullable disable
using IronBarCode;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO.Compression;
using UserManagementAPI.POCOs;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Response;

using IronBarCode;
using SixLabors.ImageSharp.Memory;
using System.IO.Compression;
using System.Drawing;
using System.Security.Cryptography;

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
                var cr = await cnd.getCollectionOrderItemsAsync(cndData.orderId);
                cndData.orderItems = cr.descriptionOfitems;
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

        public async Task<DownloadAPIResponse> GenerateZippedBarCodeAsync(string payLoad, string barcodes, string itemDescriptions)
        {
            DownloadAPIResponse rsp = null;

            try
            {
                var zipName = Path.Combine(utils.ConfigObject.ZIPPED_FOLDER, $"ZippedBarCodes-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip");
                
                var bb = new clsBarCode();

                var dta = await bb.returnZippedBarCodesAsync(barcodes, payLoad, itemDescriptions, zipName);

                rsp = new DownloadAPIResponse() { 
                    status = true,
                    message = @"success",
                    data = dta,
                    mimeType = @"application/zip",
                    filename = zipName
                };

                return rsp;
            }
            catch (Exception ex)
            {
                return rsp = new DownloadAPIResponse()
                {
                    status = false,
                    message = $"error: {ex.Message}"
                };
            }
        }
        public async Task<DownloadAPIResponse> GenerateBarCodeAsync(string payLoad, string barcodes, string itemDescriptions)
        {
            //TODO: generate bar code data
            DownloadAPIResponse rsp = null;
            
            try
            {
                clsCollectionAndDelivery cnd = new clsCollectionAndDelivery();
                var cndData = payLoad.Substring(0, 3).Trim() == @"PCO" ? await cnd.getDeliveryRecordAsync(payLoad) : await cnd.getCollectionRecordAsync(payLoad);

                clsBarCode b = new clsBarCode();
                var pngBytes = await b.returnBarCodeAsync(barcodes,payLoad,itemDescriptions);

                var d = b.ImageFromByteArray(pngBytes,payLoad);

                rsp = new DownloadAPIResponse()
                {
                    status = pngBytes.Length > 0 ? true: false,
                    message = pngBytes.Length > 0 ? $"{payLoad}.png": @"failed",
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
                var pngBytes = await b.returnBarCodeAsync(payLoad, payLoad,string.Empty);

                //var pngBytes = await b.test(payLoad, payLoad, string.Empty);

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
                    generatedDate = payLoad.generatedDate,
                    Location = payLoad.Location
                };

                //bool bn = await obj.AddAsync();

                bool bn = obj.Location.Length > 0 ? await obj.AddOpBarCodeAndLocationAsync() : await obj.AddAsync();

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

        public async Task<PaginationAPIResponse> ListWarehouseSectionsAsync(int page, int pageSize)
        {
            //TODO: list all the warehouse sections in the data store
            PaginationAPIResponse rsp = null;

            try
            {
                clsWarehouse obj = new clsWarehouse();
                var warehouseSections = await obj.ListSectionsAsync();

                int totalCount = warehouseSections.ToList().Count();
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                rsp = new PaginationAPIResponse()
                {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? $"Page {page} of {totalPages}" : @"failed",
                    total = totalCount,
                    data = warehouseSections.ToList()
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
