#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using UserManagementAPI.Data;
using System.Runtime.InteropServices;

namespace UserManagementAPI.Resources.Implementations
{
    public class PackagingService : IPackagingService
    {
        swContext config;

        public PackagingService()
        {
            config = new swContext();
        }

        public async Task<DefaultAPIResponse> GetPackageItemListAsync()
        {
            //gets packaging items from the data store
            DefaultAPIResponse rs = null;
            List<PackageItemLookup> packageItems = null;

            try
            {
                var dta = await config.TPackagingItems.ToListAsync();
                if (dta != null)
                {
                    packageItems = new List<PackageItemLookup>();
                    foreach(var d in dta)
                    {
                        var packageItem = new PackageItemLookup() { 
                            id = d.Id,
                            name = d.PackagingItem,
                            description = d.PackagingDescription
                        };

                        packageItems.Add(packageItem);
                    }

                    rs = new DefaultAPIResponse() { 
                        status = true, 
                        message = @"success",
                        data = packageItems
                    };
                }
                else { rs = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return rs;
            }
            catch(Exception x)
            {
                return rs = new DefaultAPIResponse() { 
                    status = false, 
                    message = $"error: {x.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> GetPackagePriceListAsync()
        {
            DefaultAPIResponse rs = null;
            List<PackagepriceLookup> packagePriceList = null;

            try
            {
                var query = (from tpp in config.TPackagingPrices
                             join c in config.Tcompanies on tpp.CompanyId equals c.CompanyId
                             join tpi in config.TPackagingItems on tpp.PackagingItemId equals tpi.Id
                             select new
                             {
                                 id = tpp.Id,
                                 unitPrice = tpp.UnitPrice,
                                 wholeSalePrice = tpp.WholesalePrice,
                                 retailPrice = tpp.RetailPrice,
                                 companyId = c.CompanyId,
                                 nameOfcompany = c.Company,
                                 pItemId = tpi.Id,
                                 pItemName = tpi.PackagingItem,
                                 pItemDescrib = tpi.PackagingDescription
                             });

                if (query != null)
                {
                    packagePriceList = new List<PackagepriceLookup>();
                    foreach(var q in query)
                    {
                        var obj = new PackagepriceLookup() { 
                            id = q.id,
                            oPackageItem = new PackageItemLookup()
                            {
                                id = q.pItemId,
                                name = q.pItemName,
                                description = q.pItemDescrib
                            },
                            unitPrice = (decimal) q.unitPrice,
                            wholesalePrice = (decimal) q.wholeSalePrice,
                            retailPrice = (decimal) q.retailPrice,
                            oCompany = new CompanyLookup()
                            {
                                id = q.companyId,
                                nameOfcompany = q.nameOfcompany
                            }
                        };

                        packagePriceList.Add(obj);
                    }

                    rs = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = packagePriceList
                    };
                }
                else { rs = new DefaultAPIResponse() { status = false, message = @"No data" }; }

                return rs;
            }
            catch(Exception x)
            {
                return rs = new DefaultAPIResponse()
                {
                    status = false, 
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> CreatePackageItemAsync(PackageItemLookup payLoad)
        {
            //creates a new packaging item resource
            DefaultAPIResponse rs = null;

            try
            {
                var dt = await config.TPackagingItems.Where(tpi => tpi.PackagingItem == payLoad.name.Trim()).FirstOrDefaultAsync();
                if (dt == null)
                {
                    TPackagingItem packagingItem = new TPackagingItem() { 
                        PackagingItem = payLoad.name,
                        PackagingDescription = payLoad.description
                    };

                    await config.AddAsync(packagingItem);
                    await config.SaveChangesAsync();

                    rs = new DefaultAPIResponse()
                    {
                        status = true, 
                        message = $"Packaging Item '{payLoad.name}' added to data store",
                        data = payLoad
                    };
                }
                else { rs = new DefaultAPIResponse() { status = false, message = $"Package item '{payLoad.name}' alread exist in data store" }; }

                return rs;
            }
            catch(Exception x)
            {
                return rs = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }
        public async Task<DefaultAPIResponse> CreatePackagingPriceAsync(PackagepriceLookup payLoad)
        {
            //creates a packaging price resource
            DefaultAPIResponse rs = null;

            try
            {
                var dt = await config.TPackagingPrices.Where(pp => pp.PackagingItemId == payLoad.oPackageItem.id).Where(x => x.CompanyId == payLoad.oCompany.id).FirstOrDefaultAsync();
                if (dt == null)
                {
                    TPackagingPrice tpp = new TPackagingPrice()
                    {
                        UnitPrice = payLoad.unitPrice,
                        WholesalePrice = payLoad.wholesalePrice,
                        RetailPrice = payLoad.retailPrice,
                        CompanyId = payLoad.oCompany.id,
                        PackagingItemId = payLoad.oPackageItem.id
                    };

                    await config.AddAsync(tpp);
                    await config.SaveChangesAsync();

                    rs = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Payment package '{payLoad.oPackageItem.name}' created for company '{payLoad.oCompany.nameOfcompany}' in the data store",
                        data = payLoad
                    };
                }
                else { rs = new DefaultAPIResponse() { status = false, message = $"Packaging Price '{payLoad.oPackageItem.name}' exist for company '{payLoad.oCompany.nameOfcompany}' in the data stores" }; }

                return rs;
            }
            catch(Exception x)
            {
                return rs = new DefaultAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }


    }
}
