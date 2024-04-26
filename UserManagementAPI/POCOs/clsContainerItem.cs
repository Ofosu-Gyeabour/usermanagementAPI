#nullable disable
using System.Runtime.InteropServices;
using UserManagementAPI.Enums;
using UserManagementAPI.Models;
using UserManagementAPI.Procs;

namespace UserManagementAPI.POCOs
{
    public class clsContainerItem
    {
        public int id { get; set; }
        public int containerId { get; set; }
        public int shippingOrderId { get; set; }
        public string orderNo { get; set; }
        public string itembcode { get; set; }
        public DateTime orderDate { get; set; }
        public string consignee { get; set; }
        public decimal weight { get; set; }
        public decimal cube { get; set; }
        public string mark { get; set; }
        public int qty { get; set; }
        public string itemDescrib { get; set; }
        public string method { get; set; }
        public string shipper { get; set; }

        public async Task<IEnumerable<clsContainerItem>> getContainerItems()
        {
            //todo: gets container items using the container Id
            List<clsContainerItem> containerItems = null;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var dta = await config.TContainerItems.Where(c => c.ContainerId == id).ToListAsync();
                        if (dta != null)
                        {
                            containerItems = dta
                                                .Select(a => new clsContainerItem()
                                                {
                                                    id = a.Id,
                                                    containerId = (int) a.ContainerId,
                                                    shippingOrderId = (int) a.ShippingOrderId,
                                                    orderNo = a.OrderNo,
                                                    itembcode = a.ItemBcode,
                                                    orderDate = (DateTime) a.OrderDate,
                                                    consignee = a.Consignee,
                                                    weight = (decimal) a.Weight,
                                                    cube = (decimal) a.Cube,
                                                    mark = a.Mark,
                                                    qty = (int) a.Qty,
                                                    itemDescrib = a.ItmDescrib,
                                                    method = a.Method,
                                                    shipper = a.Shipper
                                                }).ToList();
                        }
                    }
                    catch(Exception cErr)
                    {
                        throw cErr;
                    }
                }

                return containerItems;
            }
            catch(Exception x)
            {
                return containerItems;
            }
        }

        public async Task<bool> AddContainerItemAsync(int containerID, LoadedItem item)
        {
            //TODO: adds an item to a designated container
            bool bln = false;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    var transaction = await config.Database.BeginTransactionAsync();

                    try
                    {
                        //tContainerItem
                        TContainerItem containerItem = new TContainerItem()
                        {
                            ContainerId = containerID,
                            ShippingOrderId = item.recordid,
                            OrderNo = item.bolNo,
                            OrderDate = item.OrderCreationDate,
                            ItemBcode = item.itembcode,
                            Consignee = item.Consignee.Length > 1 ? item.Consignee : item.ConsigneeName,
                            Weight = item.itemWeight,
                            Cube = item.itemVolume,
                            Mark = item.marks,
                            Qty = item.qty,
                            ItmDescrib = item.itemDescription,
                            Method = item.method,
                            Shipper = item.Shipper.Length > 1 ? item.Shipper: item.ShipperName
                        };

                        await config.AddAsync(containerItem);
                        await config.SaveChangesAsync();

                        //tshippingorderitem
                        var obj = await config.TShippingOrderItems.Where(c => c.ItemBcode == item.itembcode).FirstOrDefaultAsync();
                        if (obj != null)
                        {
                            //update status
                            obj.ItemStatusId = (int)ItemStatusEnum.SCANNED_TO_CONTAINER;

                            await config.SaveChangesAsync();
                        }

                        await transaction.CommitAsync();
                        bln = true;
                    }
                    catch(Exception configX)
                    {
                        await transaction.RollbackAsync();
                        throw configX;
                    }
                }

                return bln;
            }
            catch(Exception x)
            {
                return bln;
            }
        }
    
        public async Task<bool> RemoveContainerItemAsync()
        {
            //removes container item from the data store
            bool bln = false;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    var trans = await config.Database.BeginTransactionAsync();

                    try
                    {
                        var containerObj = await config.TContainerItems.Where(x=>x.ShippingOrderId == shippingOrderId).Where(y => y.ItemBcode == itembcode).FirstOrDefaultAsync();
                        if (containerObj != null)
                        {
                            config.TContainerItems.Remove(containerObj);
                            await config.SaveChangesAsync();
                        }

                        //update tshippingorderitems
                        var shippingOrderItemObj = await config.TShippingOrderItems.Where(x => x.ShippingorderId == shippingOrderId).Where(y => y.ItemBcode == itembcode).FirstOrDefaultAsync();
                        if (shippingOrderItemObj != null)
                        {
                            shippingOrderItemObj.ItemStatusId = (int)ItemStatusEnum.SCANNED_TO_WAREHOUSE;

                            await config.SaveChangesAsync();

                            await trans.CommitAsync();
                            bln = true;
                        }
                    }
                    catch(Exception configX)
                    {
                        await trans.RollbackAsync();
                        throw configX;
                    }
                }

                return bln;
            }
            catch(Exception x)
            {
                return bln;
            }
        }
    
    }

    public class clsContainerDocValue
    {
        public int DocValueId { get; set; }
        public int? LookupId { get; set; } = 0;
        public string? doctype { get; set; } = string.Empty;
        public string? tcontainerdocvalue { get; set; } = string.Empty;

        public int? containerId { get; set; } = 0;

        public async Task<IEnumerable<GenericLookup>> fetchContainerDocLookup()
        {
            List<GenericLookup> clookup = null;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var dta = await config.TLoadContainerDocLookups.ToListAsync();
                        if (dta != null)
                        {
                            clookup = dta.Select(a => new GenericLookup()
                            {
                                id = a.Id,
                                idValue = a.DocType
                            }).ToList();
                        }
                    }
                    catch(Exception configX)
                    {
                        throw configX;
                    }
                }

                return clookup;
            }
            catch(Exception x)
            {
                return clookup;
            }
        }

        public async Task<bool> insertDefaultValuesAsync()
        {
            //todo: inserts blank values for the docs of a container
            bool bln = false;

            try
            {
                var cd = await fetchContainerDocLookup();

                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        if (cd != null)
                        {
                            foreach (var doc in cd)
                            {
                                TLoadContainerDocValue cdValue = new TLoadContainerDocValue()
                                {
                                    ContainerId = containerId,
                                    TcontainerDocLookupId = doc.id,
                                    TcontainerDocValue = string.Empty
                                };

                                await config.TLoadContainerDocValues.AddAsync(cdValue);
                                await config.SaveChangesAsync();
                            }
                        }
                    }
                    catch(Exception cErr)
                    {
                        throw cErr;
                    }
                }

                return bln = true;
            }
            catch(Exception x)
            {
                return bln;
            }
        }
    }
}
