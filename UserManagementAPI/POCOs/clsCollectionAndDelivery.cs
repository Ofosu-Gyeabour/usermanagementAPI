#nullable disable
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UserManagementAPI.Data;
using UserManagementAPI.Enums;
using UserManagementAPI.utils;

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
        public string bcodeItems { get; set; }  //bar code for all items in the order

        public DateTime orderDate { get; set; }
        public string driverName { get; set; }
        public string orderNote { get; set; }   //make changes to the schema and add it

        #endregion



        #region Methods
        public async Task<ItemBarCodeRecord> getCollectionOrderItemsAsync(int id)
        {
            //gets the collection order items and converts the entire thing to a string
            ItemBarCodeRecord collRec = new ItemBarCodeRecord();
            string results = string.Empty;
            string bcodes = string.Empty;

            try
            {
                dtStore dt = new dtStore();
                var dataSet = await dt.GetOrderItems(id);

                if (dataSet.ToList().Count() > 0)
                {
                    foreach(var ds in dataSet.ToList())
                    {
                        if (ds.qty == 1)
                        {
                            results += results.Length == 0 ? ds.itemdescription : $",{ds.itemdescription}";
                            bcodes += bcodes.Length == 0 ? $"{id}o{ds.itemid.ToString()}o{ds.qty.ToString()}" : $"|{id}o{ds.itemid.ToString()}o{ds.qty.ToString()}";
                        }
                        else if (ds.qty > 1)
                        {
                            results += results.Length == 0 ? $"{ds.qty} {ds.pluralName}" : $",{ds.qty} {ds.pluralName}";
                            bcodes += bcodes.Length == 0 ? $"{id}o{ds.itemid.ToString()}o{ds.qty.ToString()}" : $"|{id}o{ds.itemid.ToString()}o{ds.qty.ToString()}";
                        }
                    }
                }

                collRec.descriptionOfitems = results;
                collRec.barcodeOfitems = bcodes;

                return collRec;
            }
            catch(Exception x)
            {
                return collRec;
            }
        }

        public record ItemBarCodeRecord()
        {
            public string descriptionOfitems { get; set; }
            public string barcodeOfitems { get; set; }
        }
        public async Task<ItemBarCodeRecord> getDeliveryOrderItemsAsync(int id)
        {
            //gets the collection order items and converts the entire thing to a string
            ItemBarCodeRecord item = new ItemBarCodeRecord();

            string results = string.Empty;
            string bcodes = string.Empty;

            try
            {
                dtStore dt = new dtStore();
                var dataSet = await dt.GetDeliveryOrderItemsAsync(id);

                if (dataSet.ToList().Count() > 0)
                {
                    foreach (var ds in dataSet.ToList())
                    {
                        if (ds.qty == 1)
                        {
                            results += results.Length == 0 ? $"{ds.qty} {ds.itemName}" : $",{ds.qty} {ds.itemName}";
                            bcodes += bcodes.Length == 0 ? $"{id}o{ds.itemid.ToString()}o{ds.qty.ToString()}" : $"|{id}o{ds.itemid.ToString()}o{ds.qty.ToString()}";
                        }
                        else if (ds.qty > 1)
                        {
                            results += results.Length == 0 ? $"{ds.qty} {ds.pluralName}" : $",{ds.qty} {ds.pluralName}";
                            bcodes += bcodes.Length == 0 ? $"{id}o{ds.itemid.ToString()}o{ds.qty.ToString()}" : $"|{id}o{ds.itemid.ToString()}o{ds.qty.ToString()}";
                        }
                    }
                }

                item.descriptionOfitems = results;
                item.barcodeOfitems = bcodes;

                return item;
            }
            catch (Exception x)
            {
                return item;
            }
        }

        public async Task<clsCollectionAndDelivery> getCollectionRecordAsync(string orderNo)
        {
            //TODO: gets the collection record via the orderNo
            clsCollectionAndDelivery data = null;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var q = (from tsh in config.TShippings
                                     //join soi in config.TShippingOrderItems on tsh.Id equals soi.ShippingorderId
                                 join cmp in config.Tcompanies on tsh.CompanyId equals cmp.CompanyId
                                 join cl in config.TClients on tsh.ConsignorId equals cl.Id    //changed from customerId to consignorId
                                 join clad in config.TClientAddresses on cl.Id equals clad.ClientId
                                 where tsh.BolNo == orderNo

                                 select new
                                 {
                                     id = tsh.Id,
                                     company = cmp.Company,
                                     orderNo = tsh.BolNo,
                                     name = cl.ClientTypeId == 1 ? $"{cl.Firstname} {cl.Surname}" : $"{cl.ClientBusinessName}",
                                     mobile = cl.MobileNo != null ? cl.MobileNo.ToString() : string.Empty,
                                     address = $"{clad.ClientAddr1},{clad.ClientAddr2},{clad.ClientAddr3}",
                                     email = cl.ClientEmailAddr != null ? cl.ClientEmailAddr.ToString() : string.Empty,
                                     oDate = tsh.Driverdeliverydate,
                                     driver = tsh.DriveruserName,
                                     driverNote = tsh.Drivernote != null ? tsh.Drivernote.ToString() : string.Empty
                                 });

                        var qData = await q.ToListAsync().ConfigureAwait(false);
                        data = qData
                                     .Select(a => new clsCollectionAndDelivery()
                                     {
                                         orderId = a.id,
                                         companyName = a.company,
                                         orderNo = a.orderNo,
                                         type = @"COLLECTION",
                                         name = a.name,
                                         mobile = a.mobile,
                                         address = a.address,
                                         email = a.email,
                                         orderDate = (DateTime)a.oDate,
                                         driverName = a.driver,
                                         orderNote = a.driverNote    //introduce column to the schema
                                     }).FirstOrDefault();

                        return data;
                    }
                    catch(Exception configErr)
                    {
                        throw configErr;
                    }
                }
            }
            catch(Exception x)
            {
                return data;
            }
        }

        public async Task<IEnumerable<clsCollectionAndDelivery>> getCollectionAsync(DateTime df, DateTime dt)
        {
            //TODO: gets collections from the data store for the date range
            List<clsCollectionAndDelivery> results = new List<clsCollectionAndDelivery>();

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var q = (from tsh in config.TShippings
                                 //join soi in config.TShippingOrderItems on tsh.Id equals soi.ShippingorderId
                                 join cmp in config.Tcompanies on tsh.CompanyId equals cmp.CompanyId
                                 join cl in config.TClients on tsh.ConsignorId equals cl.Id    //changed from customerId to consignorId
                                 join clad in config.TClientAddresses on cl.Id equals clad.ClientId
                                 where tsh.Driverdeliverydate >= df && tsh.Driverdeliverydate <= dt
                                 && tsh.OrderStatusId == (int)OrderStatusEnum.INPUTTED

                                 select new
                                 {
                                     id = tsh.Id,
                                     company = cmp.Company,
                                     orderNo = tsh.BolNo,
                                     name = cl.ClientTypeId == 1 ? $"{cl.Firstname} {cl.Surname}" : $"{cl.ClientBusinessName}",
                                     mobile = cl.MobileNo != null ? cl.MobileNo.ToString() : string.Empty,
                                     address = $"{clad.ClientAddr1},{clad.ClientAddr2},{clad.ClientAddr3}",
                                     email = cl.ClientEmailAddr != null ? cl.ClientEmailAddr.ToString() : string.Empty,
                                     oDate = tsh.Driverdeliverydate,
                                     driver = tsh.DriveruserName,
                                     driverNote = tsh.Drivernote != null ? tsh.Drivernote.ToString(): string.Empty
                                 });

                        var qData = await q.ToListAsync().ConfigureAwait(false);

                        results = qData
                                     .Select(a => new clsCollectionAndDelivery()
                                     {
                                         orderId = a.id,
                                         companyName = a.company,
                                         orderNo = a.orderNo,
                                         type = @"COLLECTION",
                                         name = a.name,
                                         mobile = a.mobile,
                                         address = a.address,
                                         email = a.email,
                                         orderDate = (DateTime)a.oDate,
                                         driverName = a.driver,
                                         orderNote = a.driverNote    //introduce column to the schema
                                     }).ToList();

                        return results;
                    }
                    catch(Exception configErr)
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
        
        public async Task<clsCollectionAndDelivery> getDeliveryRecordAsync(string orderNo)
        {
            //TODO: gets a single record using the order number
            clsCollectionAndDelivery data = new clsCollectionAndDelivery();

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var q = (from po in config.TpackagingOrders
                                     //join poi in config.TpackagingOrderItems on po.Id equals poi.PackageOrderId
                                 join cmp in config.Tcompanies on po.CompanyId equals cmp.CompanyId
                                 join cl in config.TClients on po.ClientId equals cl.Id
                                 where po.OrderNo == orderNo

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
                                     driver = po.DriverName,    //make it the email address
                                     delivOrderNote = po.DeliveryNote != null ? po.DeliveryNote.Trim() : string.Empty
                                 });

                        var qData = await q.ToListAsync().ConfigureAwait(false);
                        data = qData
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

                                         orderDate = (DateTime)a.oDate,
                                         driverName = a.driver,
                                         orderNote = a.delivOrderNote    //introduce column to the schema
                                     }).FirstOrDefault();

                        return data;
                    }
                    catch(Exception configExc)
                    {
                        throw configExc;
                    }
                }
            }
            catch(Exception x)
            {
                return data;
            }
        }
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
                                 //join poi in config.TpackagingOrderItems on po.Id equals poi.PackageOrderId
                                 join cmp in config.Tcompanies on po.CompanyId equals cmp.CompanyId
                                 join cl in config.TClients on po.ClientId equals cl.Id
                                 where po.DeliveryDate >= df && po.DeliveryDate <= dt
                                 && po.StatusId == (int)OrderStatusEnum.INPUTTED

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
                                     driver = po.DriverName,    //make it the email address
                                     delivOrderNote = po.DeliveryNote != null ? po.DeliveryNote.Trim(): string.Empty
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
                                         orderNote = a.delivOrderNote    //introduce column to the schema
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
