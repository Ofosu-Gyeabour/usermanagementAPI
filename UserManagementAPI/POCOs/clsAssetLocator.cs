#nullable disable
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.POCOs
{
    public class clsAssetLocator
    {

        public int Id { get; set; }
        public string itembcode { get; set; }
        public clsWarehouse oWarehouse { get; set; }
        public int warehousesectionId { get; set; }
        public string warehousesectionName { get; set; }
        public DateTime lastLocationDate { get; set; }
        
        public string orderNo { get; set; }
        public string packagingitem { get; set; }
        public string pluralName { get; set; }
        public int qty { get; set; }
        public string itemstatusdescrib { get; set; }
        public string mainDescrib { get; set; }

        public async Task<IEnumerable<clsAssetLocator>> getWarehouseAssetsAsync()
        {
            //todo: gets all assets belonging to the warehouse
            List<clsAssetLocator> assets = null;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var q = (from ast in config.TAssetLocators
                                 join twh in config.TwhouseSections on ast.WarehouseSectionId equals twh.Id
                                 select new
                                 {
                                     id = ast.Id,
                                     bcode = ast.Itembcode,
                                     whouseId = ast.WarehouseSectionId,
                                     warehouse = twh.WhouseSection,
                                     associatedBcode = twh.AssocBarcode,
                                     lastLocationDate = ast.LastLocationUpdate
                                 });

                        var ql = await q.ToListAsync().ConfigureAwait(false);

                        assets = ql.Select(a => new clsAssetLocator()
                        {
                            Id = a.id,
                            itembcode = a.bcode,
                            lastLocationDate = (DateTime) a.lastLocationDate,
                            oWarehouse = new clsWarehouse()
                            {
                                Id = (int) a.whouseId,
                                warehouseSection = a.warehouse 
                            },
                            warehousesectionId = (int) a.whouseId,
                            warehousesectionName = a.warehouse 
                        }).ToList();

                        return assets;
                    }
                    catch(Exception configE)
                    {
                        throw configE;
                    }
                }
            }
            catch(Exception x)
            {
                return assets;
            }
        }

        public async Task<bool> updateWarehouseAssetAsync()
        {
            //todo: updates warehouse asset in the locator list
            //todo: if warehouse asset is not listed, insert into the current locator list

            bool bln = false;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    var trans = config.Database.BeginTransaction();
                    
                    try
                    {
                        var objTest = await config.TAssetLocators.Where(x => x.Itembcode == itembcode).FirstOrDefaultAsync();

                        if (objTest != null)
                        {
                            //update asset locator and last update date
                            objTest.WarehouseSectionId = oWarehouse.Id;
                            objTest.LastLocationUpdate = lastLocationDate;

                            await config.SaveChangesAsync();
                        }
                        else
                        {
                            //asset does not exist. create one
                            TAssetLocator assetLocator = new TAssetLocator() { 
                                Itembcode = itembcode,
                                WarehouseSectionId = oWarehouse.Id,
                                LastLocationUpdate = lastLocationDate
                            };

                            await config.AddAsync(assetLocator);
                            await config.SaveChangesAsync();
                        }

                        //transaction
                        await trans.CommitAsync();
                        bln = true;
                    }
                    catch(Exception ce)
                    {
                        await trans.RollbackAsync();
                        throw ce;
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

    public class clsAssetParticular
    {
        public string orderNo { get; set; }
        public string packagingitem { get; set; }
        public string pluralName { get; set; }
        public int qty { get; set; }
        public string itemstatusdescrib { get; set; }
    }

}
