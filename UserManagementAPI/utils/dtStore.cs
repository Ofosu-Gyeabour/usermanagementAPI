#nullable disable

using System.Runtime.InteropServices;
using UserManagementAPI.Procs;

namespace UserManagementAPI.utils
{
    public record View360
    {
        public IEnumerable<pShippingOrder> procShippings { get; set; }
        public IEnumerable<pPackagingOrder> procPackagings { get; set; }
        public IEnumerable<pSalesOrder> procSales { get; set; }

        public int customerId { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }

        public async Task<View360> get360View()
        {
            //TODO: gets all the data points for the 360 view
            dtStore dts = new dtStore();
            View360 resultView = new View360();

            try
            {
                resultView.procShippings = await dts.GetShippingOrders(this.customerId, this.dateFrom.ToString("yyyy-MM-dd"), this.dateTo.ToString("yyyy-MM-dd"));
                resultView.procPackagings = await dts.GetPackagingOrders(this.customerId, this.dateFrom.ToString("yyyy-MM-dd"), this.dateTo.ToString("yyyy-MM-dd"));
                resultView.procSales = await dts.GetSalesOrders(this.customerId, this.dateFrom.ToString("yyyy-MM-dd"), this.dateTo.ToString("yyyy-MM-dd"));

                return resultView;
            }
            catch(Exception x)
            {
                return resultView;
            }
        }
    }
    public record shippingOrderRec
    {
        public int customerID { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }

        public DateTime? df { get; set; } 
        public DateTime? dt { get; set; }
    }
    public class dtStore
    {
        private swContext config;

        public dtStore()
        {
            config = new swContext();
        }

        public record shippingOrderRec
        {
            public int customerID { get; set; }
            public int page { get; set; }
            public int pageSize { get; set; }
        }
        public async Task<IEnumerable<pShippingOrder>> GetShippingOrders(int customerId, string df, string dt)
        {
            //TODO: gets a list of shipping orders from the data store using stored procedure
            List<pShippingOrder> result = null;

            result = await config.PShippingOrders.FromSqlRaw("exec proc_getshipping_orders {0}, {1}, {2};",customerId, df, dt).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<pPackagingOrder>> GetPackagingOrders(int customerID, string df, string dt)
        {
            //TODO: gets a list of packaging orders from the data store using stored procedure
            List<pPackagingOrder> results = null;

            results = await config.PPackagingOrders.FromSqlRaw("exec proc_getpackaging_orders {0}, {1}, {2}", customerID, df, dt).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<pSalesOrder>> GetSalesOrders(int customerID, string df, string dt)
        {
            //TODO: gets a list of sales orders from the data store using stored procedure
            List<pSalesOrder> salesResults = null;
            try
            {
                salesResults = await config.PSalesOrders.FromSqlRaw("exec proc_getsales_orders {0},{1}, {2}", customerID, df, dt).ToListAsync();
                return salesResults;
            }
            catch(Exception x)
            {
                return salesResults;
            }          
        }

        #region adhoc or sale records

        public psaleRecord GetSaleRecord(string salesOrderNo)
        {
            //TODO: gets sales order or adhoc item record
            psaleRecord saleOrderResult = null;
            try
            {
                var xx =  config.Set<psaleRecord>().FromSqlRaw("dbo.proc_fetchsaleorder @salesorder = {0}", salesOrderNo).AsEnumerable().FirstOrDefault();
                //var o =  config.PsaleRecords.FromSqlRaw("exec dbo.proc_fetchsaleorder {0}", salesOrderNo).AsEnumerable().FirstOrDefault();
                saleOrderResult = xx;
                return saleOrderResult;
            }
            catch(Exception x)
            {
                return saleOrderResult;
            }
        }

        public async Task<IEnumerable<pSalePayment>> GetSaleOrderPayments(int salesOrderID)
        {
            //TODO: gets the list of items for the sale or adhoc order
            List<pSalePayment> salepayments = null;

            try
            {
                salepayments = await config.PSalePayments.FromSqlRaw("exec proc_getsale_payments {0}", salesOrderID).ToListAsync();
                return salepayments;
            }
            catch(Exception x)
            {
                return salepayments;
            }
        }

        public async Task<IEnumerable<pSaleItem>> GetSaleItems(int salesOrderID)
        {
            //TODO: gets sale or adhoc items for the sale or adhoc order
            List<pSaleItem> salesitem = null;

            salesitem = await config.PSaleItems.FromSqlRaw("exec proc_fetch_saleitems {0}", salesOrderID).ToListAsync();
            return salesitem;
        }

        #endregion

        #region package order

        public pPackageOrder GetPackageOrder(string packageOrderNo)
        {
            //TODO: gets package order number from the data store using stored procedure
            pPackageOrder obj = null;

            try
            {
                obj = config.PPackageOrderRecord.FromSqlRaw("exec dbo.proc_fetchpackageOrder {0}", packageOrderNo).AsEnumerable().FirstOrDefault();
                return obj;
            }
            catch(Exception x)
            {
                return obj;
            }
        }

        public async Task<IEnumerable<pPackageOrderItem>> GetPackageOrderItems(int packageOrderID)
        {
            //TODO: uses the package order id to fetch package items from the data store using stored procedure
            List<pPackageOrderItem> results = null;

            try
            {
                results = await config.PPackageOrderItems.FromSqlRaw("exec dbo.proc_fetchpackageOrder_items {0}", packageOrderID).ToListAsync();
                return results;
            }
            catch(Exception x)
            {
                return results;
            }
        }

        public async Task<IEnumerable<pPackageOrderCharge>> GetPackageOrderCharges(int packageOrderID)
        {
            //TODO: gets the charges associated with a package using LINQ
            List<pPackageOrderCharge> results = null;

            try
            {
                var q = (from ch in config.TpackagingOrderCharges
                         where ch.PackageOrderId == packageOrderID
                         select new
                         {
                             id = ch.Id,
                             chargeId = ch.ChargeId,
                             chargeDescription = ch.ChargeDescription,
                             amt = ch.ChargeAmt
                         });

                var qList = await q.ToListAsync().ConfigureAwait(false);

                results = qList
                             .Select(a => new pPackageOrderCharge()
                             {
                                 Id = a.id,
                                 chargeId = a.chargeId,
                                 chargeDescription = a.chargeDescription,
                                 chargeAmt = a.amt
                             }).ToList();

                return results;
            }
            catch(Exception x)
            {
                return results;
            }
        }

        public async Task<IEnumerable<pPackageOrderPayment>> GetPackageOrderPayments(int packageOrderID)
        {
            //TODO: gets the package order payments for a package order record
            List<pPackageOrderPayment> results = null;

            try
            {
                var pQry = (from tp in config.TpackagingOrderPayments
                            join pm in config.TPaymentMethods on tp.PayMethodId equals pm.Id
                            where tp.PackageOrderId == packageOrderID
                            select new
                            {
                                id = tp.Id,
                                paymentDate = tp.PayDate,
                                paymentAmount = tp.PayAmt,
                                paymentMethodId = tp.PayMethodId,
                                paymentMethod = pm.Method,
                                paymentReceiptNo = tp.PayReceiptNo,
                                paymentOutstanding = tp.OutstandingAmt
                            });

                var pList = await pQry.ToListAsync().ConfigureAwait(false);

                results = pList
                            .Select(a => new pPackageOrderPayment()
                            {
                                Id = a.id,
                                payDate = a.paymentDate,
                                payAmt = a.paymentAmount,
                                payMethodId = a.paymentMethodId,
                                payMethod = a.paymentMethod,
                                receiptNo = a.paymentReceiptNo,
                                outstandingAmt = a.paymentOutstanding
                            }).ToList();

                return results;
            }
            catch(Exception x)
            {
                return results;
            }
        }
        #endregion


    }
}
