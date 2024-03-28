#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Diagnostics;
using UserManagementAPI.utils;
using UserManagementAPI.Models;

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
        
        public async Task<DefaultAPIResponse> GetPackagingItemAndPriceListAsync(CompanyLookup payLoad)
        {
            //TODO: companyId = 1,18,27,28 and 30 uses packaging items from 1
            DefaultAPIResponse response = null;
            int cid = payLoad.id;
            List<PackagepriceLookup> packagePrices = null;

            try
            {
                if ((cid == 1) || (cid == 18) || (cid == 27) || (cid == 28) || (cid == 30))
                {
                    cid = 1;
                }

                var Query = (from tpp in config.TPackagingPrices
                             join tpi in config.Tpackagings on tpp.PackagingItemId equals tpi.Id
                             join c in config.Tcompanies on tpp.CompanyId equals c.CompanyId
                             where tpp.CompanyId == cid

                             select new
                             {
                                 id = tpp.Id,
                                 packagingitemId = tpp.PackagingItemId,
                                 packagingitemName = tpi.Packagingitem,
                                 description = tpi.Itemdescription,
                                 unitPrice = tpp.UnitPrice,
                                 wholesaleprice = tpp.WholesalePrice,
                                 retailPrice = tpp.RetailPrice,
                                 companyId = tpp.CompanyId,
                                 companyName = c.Company,
                                 nomcode = tpi.Itemcode
                             });

                var QueryList = await Query.ToListAsync().ConfigureAwait(false);

                packagePrices = QueryList
                                    .Select(x => new PackagepriceLookup()
                                    {
                                        id = x.id,
                                        oPackageItem = new PackageItemLookup()
                                        {
                                            id = (int) x.packagingitemId,
                                            name = x.packagingitemName
                                        },
                                        unitPrice = (decimal) x.unitPrice,
                                        wholesalePrice = (decimal) x.wholesaleprice,
                                        retailPrice = (decimal) x.retailPrice,
                                        oCompany = new CompanyLookup()
                                        {
                                            id = (int)x.companyId,
                                            nameOfcompany = x.companyName
                                        },
                                        nomCode = x.nomcode
                                    }).ToList();

                return response = new DefaultAPIResponse()
                {
                    status = true,
                    message = $"{packagePrices.Count()} records fetched from the data store",
                    data = packagePrices
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

        public async Task<UploadAPIResponse> UploadPackageItemAsync(IEnumerable<PackageItemLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<PackageItemLookup> successList = new List<PackageItemLookup>();
            List<PackageItemLookup> errorList = new List<PackageItemLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        var Query = (from pck in config.TPackagingItems
                                     where pck.PackagingItem == record.name.Trim()
                                     && pck.PackagingDescription == record.description.Trim()

                                     select new
                                     {
                                         id = pck.Id,
                                         pItem = pck.PackagingItem,
                                         describ = pck.PackagingDescription
                                     });

                        if (Query.Count() == 0)
                        {
                            TPackagingItem obj = new TPackagingItem() { 
                                PackagingItem = record.name.ToUpper().Trim(),
                                PackagingDescription = record.description.ToUpper().Trim()
                            };

                            await config.AddAsync(obj);
                            await config.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Packaging Item '{record.name.Trim()}' already exist in the data store");
                        }
                    }
                    catch(Exception exc)
                    {
                        failed += 1;
                        errorList.Add(record);
                        errors.Add($"error: '{exc.Message}'");
                    }
                }

                return response = new UploadAPIResponse()
                {
                    status = true,
                    successCount = success,
                    errorCount = failed,
                    data = successList,
                    errorList = errorList,
                    errorMessageList = errors,
                    message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}"
                };
            }
            catch(Exception x)
            {
                return response = new UploadAPIResponse() { 
                    status = false,
                    message = $"error: '{x.Message}'",
                    errorList = errorList
                };
            }
        }
    
        public async Task<UploadAPIResponse> UploadPackagingPriceAsync(IEnumerable<PackagepriceLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<PackagepriceLookup> successList = new List<PackagepriceLookup>();
            List<PackagepriceLookup> errorList = new List<PackagepriceLookup>();
            List<string> errors = new List<string>();

            Tcompany comp = null;
            TPackagingItem pck = null;

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        using (var cfg = new swContext())
                        {
                            comp = await cfg.Tcompanies.Where(c => c.Company == record.oCompany.nameOfcompany.Trim()).FirstOrDefaultAsync();
                            pck = await cfg.TPackagingItems.Where(p => p.PackagingItem == record.oPackageItem.name.Trim()).FirstOrDefaultAsync();
                        }

                        if ((comp != null) && (pck != null))
                        {
                            //check if record exist
                            var Query = (from pp in config.TPackagingPrices
                                         join pitem in config.TPackagingItems on pp.PackagingItemId equals pitem.Id
                                         join cmp in config.Tcompanies on pp.CompanyId equals cmp.CompanyId
                                         where pp.PackagingItemId == pck.Id &&
                                         pp.CompanyId == comp.CompanyId &&
                                         pp.UnitPrice == record.unitPrice &&
                                         pp.WholesalePrice == record.wholesalePrice &&
                                         pp.RetailPrice == record.retailPrice

                                         select new
                                         {
                                             id = pp.Id,
                                             rPrice = pp.RetailPrice
                                         });

                            if (Query.Count() == 0)
                            {
                                TPackagingPrice obj = new TPackagingPrice()
                                {
                                    PackagingItemId = pck.Id,
                                    UnitPrice = record.unitPrice,
                                    WholesalePrice = record.wholesalePrice,
                                    RetailPrice = record.retailPrice,
                                    CompanyId = comp.CompanyId
                                };

                                await config.AddAsync(obj);
                                await config.SaveChangesAsync();

                                success += 1;
                                successList.Add(record);
                            }
                            else
                            {
                                failed += 1;
                                errorList.Add(record);
                                errors.Add($"Packaging price record already exist with data points");
                            }
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"error: either company '{record.oCompany.nameOfcompany}' or packaging item '{record.oPackageItem.name}' cannot be found in the data store");
                        }
                    }
                    catch(Exception exc)
                    {
                        failed += 1;
                        errorList.Add(record);
                        errors.Add($"error: '{exc.Message}'");
                    }
                }

                return response = new UploadAPIResponse()
                {
                    status = true,
                    successCount = success,
                    errorCount = failed,
                    data = successList,
                    errorList = errorList,
                    errorMessageList = errors,
                    message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}"
                };
            }
            catch(Exception x)
            {
                return response = new UploadAPIResponse()
                {
                    status = false,
                    message = $"error: '{x.Message}'",
                    errorList = errorList
                };
            }
        }

        public async Task<DefaultAPIResponse> GetPackagePriceRecord(int companyId, int itemID)
        {
            //TODO: gets a packaging price record from stock...in a selected company with an itemID
            DefaultAPIResponse response = null;
            PackagepriceLookup pprice = null;

            try
            {
                Helper helper = new Helper();
                var presults = await helper.getPackagePriceRecordAsync(companyId, itemID);

                return response = new DefaultAPIResponse() { 
                    status = presults != null ? true: false,
                    message = presults != null ? $"Packaging item data fetched for {presults.oPackageItem.name} with respect of {presults.oCompany.nameOfcompany} company": @"No data",
                    data = presults
                };
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

        public async Task<DefaultAPIResponse> AddPackagingStockAsync(PackageStockLookup payLoad)
        {
            //TODO: add a packaging stock to the data store
            DefaultAPIResponse rsp = null;

            try
            {
                Helper helper = new Helper();
                var bln = await helper.savePackagingStockAsync(payLoad);

                return rsp = new DefaultAPIResponse() { 
                    status = bln,
                    message = bln == true ? $"{payLoad.oPackageItem.name} saved successfully": @"An error occured. Please see Administrator",
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

        public async Task<PaginationAPIResponse> ListPackagingStockAsync(int page, int pageSize)
        {
            //TODO: list packaging stock
            PaginationAPIResponse rsp = null;

            try
            {
                Helper helper = new Helper();
                var packageStockList = await helper.ListPackagingStockAsync();

                int totalCount = packageStockList.ToList().Count();
                int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                rsp = new PaginationAPIResponse()
                {
                    status = totalCount > 0 ? true : false,
                    message = totalCount > 0 ? @"success" : @"failed",
                    total = totalCount,
                    data = packageStockList
                                  .ToList()
                                  .Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToList()
                };

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new PaginationAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}"
                };
            }
        }

        public async Task<DefaultAPIResponse> GetPackagingStockAsync()
        {
            DefaultAPIResponse rsp = null;

            try
            {
                Helper helper = new Helper();
                var packageStockList = await helper.ListPackagingStockAsync();
                int totalCount = packageStockList.ToList().Count();

                return rsp = new DefaultAPIResponse() { 
                    status = totalCount > 0 ? true: false,
                    message = totalCount > 0 ? @"success": @"failed",
                    data = packageStockList.ToList()
                };
            }
            catch(Exception x)
            {
                return rsp = new DefaultAPIResponse() { 
                    status = false,
                    message  = $"error: {x.Message}"
                };
            }
        }

    }
}
