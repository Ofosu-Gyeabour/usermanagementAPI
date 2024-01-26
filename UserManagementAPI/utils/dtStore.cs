#nullable disable

using UserManagementAPI.Data;
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

            salesResults = await config.PSalesOrders.FromSqlRaw("exec proc_getsales_orders {0},{1}, {2}", customerID, df, dt).ToListAsync();
            return salesResults;
        }

    }
}
