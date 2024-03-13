#nullable disable
namespace UserManagementAPI.POCOs
{
    public class clsCollectionAndDelivery
    {
        public clsCollectionAndDelivery()
        {
            
        }

        #region properties

        public int orderId { get; set; }
        public string orderNo { get; set; }
        public string companyName { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public List<PackageItem> packageItems { get; set; }
        public string orderItems { get; set; }  //description of all items in the order
        public DateTime orderDate { get; set; }
        public string driverName { get; set; }
        public string orderNote { get; set; }   //make changes to the schema and add it

        #endregion

        #region Methods

        public async Task<IEnumerable<clsCollectionAndDelivery>> getDeliveryAsync(DateTime df, DateTime dt)
        {
            //TODO: gets deliveries from the data store for a date range
            List<clsCollectionAndDelivery> results = new List<clsCollectionAndDelivery>();

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var q = (from po in config.TpackagingOrders
                                 join poi in config.TpackagingOrderItems on po.Id equals poi.PackageOrderId
                                 join cmp in config.Tcompanies on po.CompanyId equals cmp.CompanyId
                                 join cl in config.TClients on po.ClientId equals cl.Id
                                 where po.DeliveryDate >= df && po.DeliveryDate <= dt
                                 select new
                                 {
                                     id = po.Id,
                                     company = cmp.Company,
                                     orderNo = po.OrderNo,
                                     name = cl.ClientTypeId == 1 ? $"{cl.Firstname} {cl.Surname}" : $"{cl.ClientBusinessName}",
                                     mobile = cl.MobileNo != null ? cl.MobileNo.ToString() : string.Empty,
                                     address = $"{po.Addr1},{po.Addr2},{po.Addr3}",
                                     email = cl.ClientEmailAddr != null ? cl.ClientEmailAddr.ToString() : string.Empty,
                                     oDate = po.DeliveryDate,
                                     driver = po.DriverName    //make it the email address
                                 });

                        var qData = await q.ToListAsync().ConfigureAwait(false);
                        results = qData
                                     .Select(a => new clsCollectionAndDelivery()
                                     {
                                         orderId = a.id,
                                         companyName = a.company,
                                         orderNo = a.orderNo,
                                         type = @"DELIVERY",
                                         name = a.name,
                                         mobile = a.mobile,
                                         address = a.address,
                                         email = a.email,

                                         orderDate = (DateTime) a.oDate,
                                         driverName = a.driver,
                                         orderNote = @""    //introduce column to the schema
                                     }).ToList();

                        return results;
                    }
                    catch (Exception configErr)
                    {
                        throw configErr;
                    }
                }
            }
            catch(Exception ex)
            {
                return results;
            }
        }

        #endregion

    }
}
