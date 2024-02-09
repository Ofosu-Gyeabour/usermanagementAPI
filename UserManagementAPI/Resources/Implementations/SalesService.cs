#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using UserManagementAPI.utils;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using UserManagementAPI.Procs;

namespace UserManagementAPI.Resources.Implementations
{
    public class SalesService : ISalesService
    {
        private swContext config;

        public SalesService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetPaymentMethodAsync()
        {
            //gets the payment methods
            DefaultAPIResponse rsp = null;
            List<PaymentMethod> results = null;

            try
            {
                var q = (from tpm in config.TPaymentMethods
                         select new
                         {
                             id = tpm.Id,
                             method = tpm.Method,
                             isAccounts = tpm.IsAccnt
                         });

                var queryList = await q.ToListAsync().ConfigureAwait(false);
                results = queryList.Select(x => new PaymentMethod()
                {
                    id = x.id,
                    method = x.method,
                    isAccount = x.isAccounts
                }).ToList();

                return rsp = new DefaultAPIResponse() {
                    status = true,
                    message = @"success",
                    data = results
                };
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreateSalesAsync(Sale adhocPayLoad)
        {
            DefaultAPIResponse response = null;
            int recordId = 0;

            try
            {
                Helper helper = new Helper();
                var identityValue = await helper.createOrderNumber(@"SALES");

                using var trans = await config.Database.BeginTransactionAsync();
                var adhoc = new TAdhoc() { 
                    //Id = int.Parse(identityValue.stringValue),
                    AdhocTypeId = adhocPayLoad.oAdhocType.id,
                    CompanyId = adhocPayLoad.oCompany.id,
                    IsInvoiced = adhocPayLoad.isInvoiced,
                    InvoiceDate = adhocPayLoad.invoiceDate,
                    CreatedBy = adhocPayLoad.createdBy,
                    ClientId = adhocPayLoad.clientId,
                    AdhocDescrib = adhocPayLoad.adhocDescrib,
                    AdhocDate = adhocPayLoad.adhocDate,
                    LastModifedBy = adhocPayLoad.lastModifiedBy,
                    IsuploadtoSage = adhocPayLoad.isuploadtoSage,
                    Vat = adhocPayLoad.vat,
                    PaymentTermsId = adhocPayLoad.oPaymentTerm.id,
                    OrderNo = string.Format("{0}{1}", @"SI", (10000 + identityValue.stringValue))
                };
                
                await config.AddAsync(adhoc);
                await config.SaveChangesAsync();

                recordId = adhoc.Id;

                int count = 0;
                int payCount = 0;
                //save adhoc items
                foreach(var item in adhocPayLoad.saleItems)
                {
                    var adType = await helper.getAdhocType(item.oAdhocType.name);

                    var obj = new TAdhocItem() { 
                        AdhocId = recordId,
                        Qty = item.quantity,
                        Describ = item.description.Trim().ToUpper(),
                        NCode = adType.Nomcode,
                        NCodeDescrib = adType.AdhocName.Trim().ToUpper(),
                        UnitPrice = item.unitPrice,
                        TotalPrice = item.totalPrice,
                        AddedBy = item.addedBy,
                        AddedDate = item.addedDate
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    count += 1;
                }

                //save adhoc payments
                foreach(var pay in adhocPayLoad.salePayments)
                {
                    var admethodType = await helper.getPaymentMethod(pay.payMethod.method);

                    var objPay = new TAdhocPayment() { 
                        AdhocId = recordId,
                        PayDate = pay.paymentDate,
                        PayAmt = pay.paymentAmt,
                        PayMethodId = admethodType.Id,
                        OutstandingAmt = pay.outstandingAmt
                    };

                    await config.AddAsync(objPay);
                    await config.SaveChangesAsync();

                    payCount += 1;
                }

                await trans.CommitAsync();

                response = new DefaultAPIResponse() { 
                    status = true,
                    message = $"Sales order created with Order No: {adhoc.OrderNo}",
                    data = new { adhocPayLoad}
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreatePackageAsync(Package payLoad)
        {
            //TODO: creates a packaging order
            DefaultAPIResponse response = null;

            try
            {
                Helper helper = new Helper();
                var opResponse = await helper.createPackagingOrderRecordAsync(payLoad);

                payLoad.orderNumber = opResponse;

                return response = new DefaultAPIResponse() { 
                    status = opResponse.Length > 0 ? true: false,
                    message = opResponse.Length > 0? $"Packaging order created successfully with ID: {opResponse}": @"An error occured. Please contact Administrator",
                    data = payLoad
                };
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
    
        public async Task<DefaultAPIResponse> GetSalesRecordAsync(string saleOrderNo)
        {
            //TODO: gets sales record data
            DefaultAPIResponse response = null;

            try
            {
                dtStore dt = new dtStore();
                pSalesOrderData dta = new pSalesOrderData();

                //get sale order record
                dta.saleRecord =  dt.GetSaleRecord(saleOrderNo);
                int adId = (int)dta.saleRecord.id;

                //get sale items
                var r = await dt.GetSaleItems(adId);
                dta.saleItems = r.ToList();

                //get sale payments
                var p = await dt.GetSaleOrderPayments(adId);
                dta.salePayments = p.ToList();

                response = new DefaultAPIResponse()
                {
                    status = dta != null ? true: false,
                    message = dta != null ? @"success": @"failed",
                    data = dta
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetPackageRecordAsync(string packageOrderNo)
        {
            //TODO: fetch packaging order record
            DefaultAPIResponse rsp = null;
            pPackageOrderData dta = new pPackageOrderData();

            try
            {
                dtStore dt = new dtStore();
                Helper helper = new Helper();

                dta.pPackageRecord = dt.GetPackageOrder(packageOrderNo);
                int pID = (int)dta.pPackageRecord.Id;
                int clientID = (int)dta.pPackageRecord.clientId;

                var pitems = await dt.GetPackageOrderItems(pID);
                dta.pPackageItems = pitems.ToList();

                var pcharges = await dt.GetPackageOrderCharges(pID);
                dta.pPackageCharges = pcharges.ToList();

                var ppayments = await dt.GetPackageOrderPayments(pID);
                dta.pPackageOrderPayments = ppayments.ToList();

                var packageClient = await helper.getClientUsingParamAsync(new SearchParam() { stringValue = clientID.ToString() });

                dta.pPackager = packageClient;

                rsp = new DefaultAPIResponse() {
                    status = dta != null ? true : false,
                    message = dta != null ? @"success" : @"failed",
                    data = dta
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

    }
}
