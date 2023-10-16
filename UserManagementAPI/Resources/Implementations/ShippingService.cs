#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics;

namespace UserManagementAPI.Resources.Implementations
{
    public class ShippingService : IShippingService
    {
        swContext config;
        DefaultAPIResponse response;

        public ShippingService()
        {
            config = new swContext();
        }

        #region Shipping-Line
        public async Task<DefaultAPIResponse> GetShippingLineListAsync()
        {
            //gets shipping line resources
            List<ShippingLineLookup> shippingLines = null;

            try
            {
                var dta = await config.TShippingLines.ToListAsync();
                if (dta != null)
                {
                    shippingLines = new List<ShippingLineLookup>();
                    foreach(var d in dta)
                    {
                        var obj = new ShippingLineLookup() { id = d.Id, shippingLine = d.ShippingLine };
                        shippingLines.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true, 
                        message = @"success",
                        data = shippingLines
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data"}; }

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
        public async Task<DefaultAPIResponse> CreateShippingLineAsync(ShippingLineLookup payLoad)
        {
            //creates a new shipping line resource in the data store
            try
            {
                var record = await config.TShippingLines.Where(sh => sh.ShippingLine == payLoad.shippingLine).FirstOrDefaultAsync();
                if (record == null)
                {
                    TShippingLine obj = new TShippingLine() { ShippingLine = payLoad.shippingLine };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Shipping Line '{payLoad.shippingLine}' added to the data store successfully!!!",
                        data = payLoad
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = $"Shipping Line '{payLoad.shippingLine}' already exist in the data store" }; }

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

        public async Task<UploadAPIResponse> UploadShippingLineAsync(IEnumerable<ShippingLineLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<ShippingLineLookup> successList = new List<ShippingLineLookup>();
            List<ShippingLineLookup> errorList = new List<ShippingLineLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach (var record in payLoad)
                {
                    try
                    {
                        var query = (from tsl in config.TShippingLines
                                     where tsl.ShippingLine == record.shippingLine.Trim()
                                     select new
                                     {
                                         id = tsl.Id,
                                         shippingLine =tsl.ShippingLine
                                     });

                        if (query.Count() == 0)
                        {
                            TShippingLine obj = new TShippingLine()
                            {
                                ShippingLine = record.shippingLine.ToUpper().Trim()
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
                            errors.Add($"Shipping Line '{record.shippingLine}' already exist in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errorList.Add(record);
                        errors.Add($"error: {innerExc.Message}");
                    }
                }

                response = new UploadAPIResponse()
                {
                    status = true,
                    successCount = success,
                    errorCount = failed,
                    data = successList,
                    errorList = errorList,
                    errorMessageList = errors,
                    message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}"
                };

                return response;
            }
            catch(Exception x)
            {
                return response = new UploadAPIResponse() { 
                    status = false,
                    message =$"error: {x.Message}",
                    errorList = errorList
                };
            }
        }

        #endregion

        #region Stock-control

        public async Task<DefaultAPIResponse> CreateStockControlAsync(StockControlLookup payLoad)
        {
            //create a stock control resource
            try
            {
                var record = await config.TPackagingStocks.Where(p => p.TpackagingItemId == payLoad.oPackageItem.id).Where(x => x.CompanyId == payLoad.oCompany.id).FirstOrDefaultAsync();
                if (record == null)
                {
                    //create new record
                    TPackagingStock stock = new TPackagingStock() { 
                        TpackagingItemId = payLoad.oPackageItem.id,
                        InStock = payLoad.currentStock,
                        FloorThreshold = payLoad.minimumStockAllowed,
                        CeilingThreshold = payLoad.maximumStockAllowed,
                        CompanyId = payLoad.oCompany.id
                    };

                    await config.AddAsync(stock);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Stock for '{payLoad.oPackageItem.name}' created in the data store successfully!!!",
                        data = payLoad
                    };
                }
                else
                {
                    //update old record
                    record.InStock = payLoad.currentStock;
                    record.FloorThreshold = payLoad.minimumStockAllowed;
                    record.CeilingThreshold = payLoad.maximumStockAllowed;

                    await config.SaveChangesAsync();
                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Stock record for '{payLoad.oPackageItem.name}' updated in the data store for company '{payLoad.oCompany.nameOfcompany}'",
                        data = payLoad
                    };
                }

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
        public DefaultAPIResponse GetStockControlListAsync()
        {
            List<StockControlLookup> stockList = null;

            try
            {
                var query = (from st in config.TPackagingStocks
                             join pc in config.TPackagingItems on st.TpackagingItemId equals pc.Id
                             join c in config.Tcompanies on st.CompanyId equals c.CompanyId

                             select new
                             {
                                 Id = st.Id,
                                 packagingItemId = st.TpackagingItemId,
                                 packageItem = pc.PackagingItem,
                                 inStock = st.InStock,
                                 floorThreshold = st.FloorThreshold,
                                 ceilingThreshold = st.CeilingThreshold,
                                 companyId = c.CompanyId,
                                 nameOfcompany = c.Company
                             });

                if (query != null)
                {
                    stockList = new List<StockControlLookup>();
                    foreach(var q in query)
                    {
                        var obj = new StockControlLookup()
                        {
                            id = q.Id,
                            currentStock = (int) q.inStock,
                            minimumStockAllowed = (int)q.floorThreshold,
                            maximumStockAllowed = (int) q.ceilingThreshold,
                            oPackageItem = new PackageItemLookup()
                            {
                                id = (int) q.packagingItemId,
                                name = q.packageItem
                            },
                            oCompany = new CompanyLookup()
                            {
                                id = q.companyId,
                                nameOfcompany = q.nameOfcompany
                            }
                        };

                        stockList.Add(obj);
                    }

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = @"success",
                        data = stockList
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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

        #endregion

        #region shipping-vessel
        public DefaultAPIResponse GetShippingVesselListAsync()
        {
            //gets a list of shipping vessel resources from the data store
            List<VesselLookup> vesselList = null;

            try
            {
                var query = (from v in config.TVessels
                             join spl in config.TShippingLines on v.ShippingLineId equals spl.Id

                             select new
                             {
                                 id = v.Id,
                                 nameOfshippingLine = spl.ShippingLine,
                                 IdOfshippingLine = spl.Id,
                                 vesselName = v.VesselName,
                                 vesselFlag = v.VesselFlag
                             });

                if (query != null)
                {
                    vesselList = new List<VesselLookup>();
                    foreach(var q in query)
                    {
                        var obj = new VesselLookup()
                        {
                            id = q.id,
                            nameOfvessel = q.vesselName,
                            flagOfvessel = q.vesselFlag,
                            oShippingLine = new ShippingLineLookup()
                            {
                                id = q.IdOfshippingLine,
                                shippingLine = q.nameOfshippingLine
                            }
                        };

                        vesselList.Add(obj);
                    }

                    response = new DefaultAPIResponse() { 
                        status = true, 
                        message = @"success",
                        data = vesselList
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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
        public async Task<DefaultAPIResponse> CreateShippingVesselAsync(VesselLookup payLoad)
        {
            try
            {
                var record = await config.TVessels.Where(v => v.ShippingLineId == payLoad.oShippingLine.id).Where(x => x.VesselName == payLoad.nameOfvessel).FirstOrDefaultAsync();
                if (record == null)
                {
                    TVessel obj = new TVessel()
                    {
                        ShippingLineId = payLoad.oShippingLine.id,
                        VesselName = payLoad.nameOfvessel,
                        VesselFlag = payLoad.flagOfvessel
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Vessel '{payLoad.nameOfvessel}' added successfully to shipping Line: '{payLoad.oShippingLine.shippingLine}' in the data store",
                        data = payLoad
                    };
                }
                else
                {
                    record.VesselFlag = payLoad.flagOfvessel;
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse() {
                        status = true,
                        message = $"Vessel '{payLoad.nameOfvessel}' already existing: Vessel Flag '{payLoad.flagOfvessel}' updated in data store",
                        data = payLoad
                    };
                }

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

        public async Task<UploadAPIResponse> UploadShippingVesselAsync(IEnumerable<VesselLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<VesselLookup> successList = new List<VesselLookup>();
            List<VesselLookup> errorList = new List<VesselLookup>();
            List<string> errors = new List<string>();
            TShippingLine tt = null;

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        using (var cfg = new swContext())
                        {
                            tt = await cfg.TShippingLines.Where(x => x.ShippingLine == record.oShippingLine.shippingLine.Trim()).FirstOrDefaultAsync();
                        }

                        var query = (from tv in config.TVessels
                                        join tsl in config.TShippingLines on tv.ShippingLineId equals tsl.Id
                                        where tsl.ShippingLine == record.oShippingLine.shippingLine &&
                                        tv.VesselName == record.nameOfvessel.Trim() &&
                                        tv.VesselFlag == record.flagOfvessel

                                        select new
                                        {
                                            id = tv.Id,
                                            shippingLine = tsl.ShippingLine,
                                            shippingLineId = tsl.Id,
                                            vessel = tv.VesselName,
                                            flag = tv.VesselFlag
                                        });

                        if (query.Count() == 0)
                        {
                            TVessel vesselObj = new TVessel() { 
                                ShippingLineId = tt.Id,
                                VesselName = record.nameOfvessel.ToUpper().Trim(),
                                VesselFlag = record.flagOfvessel.ToUpper().Trim()
                            };

                            await config.AddAsync(vesselObj);
                            await config.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Vessel with name '{record.nameOfvessel}' already exist for shipping line and flag");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errors.Add($"error: {innerExc.Message}");
                        errorList.Add(record);
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
                    message =$"error: {x.Message}",
                    errorList = errorList
                };
            }
        }

        #endregion

        #region shipping - methods

        public async Task<DefaultAPIResponse> GetShippingMethodListAsync()
        {
            List<ShippingMethodLookup> methods = null;

            try
            {
                var itemList = await config.TShippingMethods.ToListAsync();
                if (itemList != null)
                {
                    methods = new List<ShippingMethodLookup>();
                    foreach(var item in itemList)
                    {
                        var obj = new ShippingMethodLookup()
                        {
                            id = item.Id,
                            shippingMethod = item.Method,
                            shippingRoute = item.Route
                        };

                        methods.Add(obj);
                    }

                    response = new DefaultAPIResponse() { 
                        status = true, 
                        message = @"success",
                        data = methods
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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
        public async Task<DefaultAPIResponse> CreateShippingMethodAsync(ShippingMethodLookup payLoad)
        {
            try
            {
                var record = await config.TShippingMethods.Where(sm => sm.Method == payLoad.shippingMethod).FirstOrDefaultAsync();
                if (record == null)
                {
                    TShippingMethod obj = new TShippingMethod() { 
                        Method = payLoad.shippingMethod,
                        Route = payLoad.shippingRoute
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Shipping method '{payLoad.shippingMethod}' added to the data store successfully!!!",
                        data = payLoad
                    };
                }
                else
                {
                    //update route
                    record.Route = payLoad.shippingRoute;
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Shipping method '{payLoad.shippingMethod}' already exist: updated route with '{payLoad.shippingRoute}' in the data store",
                        data = payLoad
                    };
                }

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
        public async Task<UploadAPIResponse> UploadShippingMethodDataAsync(IEnumerable<ShippingMethodLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<ShippingMethodLookup> successList = new List<ShippingMethodLookup>();
            List<ShippingMethodLookup> errorList = new List<ShippingMethodLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        var query = (from tsm in config.TShippingMethods
                                     where tsm.Method == record.shippingMethod.Trim() &&
                                     tsm.Route == record.shippingRoute.Trim()

                                     select new
                                     {
                                         id = tsm.Id,
                                         method = tsm.Method,
                                         route = tsm.Route
                                     });

                        if (query.Count() == 0)
                        {
                            TShippingMethod shippingMethodObj = new TShippingMethod()
                            {
                                Method = record.shippingMethod.ToUpper().Trim(),
                                Route = record.shippingRoute.ToUpper().Trim()
                            };

                            await config.AddAsync(shippingMethodObj);
                            await config.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Shipping method '{record.shippingMethod}' already exist for the route '{record.shippingRoute}' in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errorList.Add(record);
                        errors.Add($"error: '{innerExc.Message}'");
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
                return new UploadAPIResponse()
                {
                    status = false,
                    message = $"error:{x.Message}",
                    errorList = errorList
                };
            }
        }

        #endregion

        #region shipper - categorization

        public async Task<UploadAPIResponse> UploadShipperCategoryAsync(IEnumerable<ShipperCategoryLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<ShipperCategoryLookup> successList = new List<ShipperCategoryLookup>();
            List<ShipperCategoryLookup> errorList = new List<ShipperCategoryLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        var Q = (from sCat in config.TShipperCategories
                                 where sCat.Description == record.description.Trim()

                                 select new
                                 {
                                     id = sCat.Id,
                                     describ = sCat.Description
                                 });

                        if (Q.Count() == 0)
                        {
                            TShipperCategory obj = new TShipperCategory() { Description = record.description.ToUpper().Trim() };
                            await config.AddAsync(obj);
                            await config.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Shipping category '{record.description}' already exist in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errorList.Add(record);
                        errors.Add($"error: '{innerExc.Message}'");
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
                    message = $"error: {x.Message}",
                    errorList = errorList
                };
            }
        }
        public async Task<DefaultAPIResponse> GetShipperCategoryListAsync()
        {
            List<ShipperCategoryLookup> s_categories = null;

            try
            {
                var records = await config.TShipperCategories.ToListAsync();
                if (records != null)
                {
                    s_categories = new List<ShipperCategoryLookup>();
                    foreach (var record in records)
                    {
                        var obj = new ShipperCategoryLookup() { id = record.Id, description = record.Description };
                        s_categories.Add(obj);
                    }

                    response = new DefaultAPIResponse() {
                        status = true,
                        message = @"success",
                        data = s_categories
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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
        public async Task<DefaultAPIResponse> CreateShipperCategoryAsync(ShipperCategoryLookup payLoad)
        {
            try
            {
                var record = await config.TShipperCategories.Where(x => x.Description == payLoad.description).FirstOrDefaultAsync();
                if (record == null)
                {
                    TShipperCategory obj = new TShipperCategory() { Description = payLoad.description };
                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true, 
                        message = $"Shipper category '{payLoad.description}' added to the data store successfully!!!",
                        data = payLoad
                    };
                }
                else
                {
                    response = new DefaultAPIResponse() { 
                        status = false,
                        message = $"Shipper category '{payLoad.description}' already exist in the data store",
                        data = payLoad
                    };
                }

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


        #endregion

        #region Delivery - methods

        public async Task<UploadAPIResponse> UploadDeliveryMethodAsync(IEnumerable<DeliveryMethodLookup> payLoad)
        {
            UploadAPIResponse response = null;
            int success = 0;
            int failed = 0;
            List<DeliveryMethodLookup> successList = new List<DeliveryMethodLookup>();
            List<DeliveryMethodLookup> errorList = new List<DeliveryMethodLookup>();
            List<string> errors = new List<string>();

            try
            {
                foreach(var record in payLoad)
                {
                    try
                    {
                        var Q = (from dm in config.TDeliveryMethods
                                 where dm.Method == record.method.Trim() 
                                 //&& dm.Description == record.methodDescription.Trim()

                                 select new
                                 {
                                     id = dm.Id,
                                     method = dm.Method,
                                     describ = dm.Description
                                 });

                        if (Q.Count() == 0)
                        {
                            TDeliveryMethod tdm = new TDeliveryMethod() { 
                                Method = record.method.ToUpper().Trim(),
                                Description = record.methodDescription.ToUpper().Trim()
                            };

                            await config.AddAsync(tdm);
                            await config.SaveChangesAsync();

                            success += 1;
                            successList.Add(record);
                        }
                        else
                        {
                            failed += 1;
                            errorList.Add(record);
                            errors.Add($"Delivery method '{record.method}' already exist in the data store");
                        }
                    }
                    catch(Exception innerExc)
                    {
                        failed += 1;
                        errorList.Add(record);
                        errors.Add($"error: '{innerExc.Message}'");
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
        public async Task<DefaultAPIResponse> GetDeliveryMethodListAsync()
        {
            //get delivery method resources
            List<DeliveryMethodLookup> methods = null;

            try
            {
                var records = await config.TDeliveryMethods.ToListAsync();
                if (records != null)
                {
                    methods = new List<DeliveryMethodLookup>();
                    foreach(var record in records)
                    {
                        var obj = new DeliveryMethodLookup()
                        {
                            id = record.Id,
                            method = record.Method,
                            methodDescription = record.Description
                        };
                        methods.Add(obj);
                    }

                    response = new DefaultAPIResponse() { 
                        status = true, 
                        message = @"success",
                        data = methods
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data"}; }
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
        public async Task<DefaultAPIResponse> CreateDeliveryMethodAsync(DeliveryMethodLookup payLoad)
        {
            try
            {
                var record = await config.TDeliveryMethods.Where(x => x.Method == payLoad.method).FirstOrDefaultAsync();
                if (record == null)
                {
                    TDeliveryMethod obj = new TDeliveryMethod()
                    {
                        Method = payLoad.method,
                        Description = payLoad.methodDescription
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = $"Delivery method '{payLoad.method}' added to the data store successfully!!!",
                        data = payLoad
                    };
                }
                else
                {
                    record.Description = payLoad.methodDescription;
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Delivery method '{payLoad.method}' already exist in the data store: updated description successfully!!!",
                        data = payLoad
                    };
                }

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


        #endregion

        #region Delivery-Zone

        public DefaultAPIResponse GetDeliveryZoneListAsync()
        {
            List<DeliveryZoneLookup> dzones = null;
            try
            {
                var query = (from dz in config.TDeliveryZones
                             join dm in config.TDeliveryMethods on dz.DeliverymethodId equals dm.Id
                             join c in config.TCountryLookups on dz.CountryId equals c.CountryId

                             select new
                             {
                                 Id = dz.Id,
                                 nameOfzone = dz.Zone,
                                 method = dm.Method,
                                 methodId = dm.Id,
                                 countryId = c.CountryId,
                                 nameOfcountry = c.CountryName
                             });

                if (query != null)
                {
                    dzones = new List<DeliveryZoneLookup>();
                    foreach(var q in query)
                    {
                        var zone = new DeliveryZoneLookup()
                        {
                            id = q.Id,
                            zoneName = q.nameOfzone,
                            oDeliveryMethod = new DeliveryMethodLookup()
                            {
                                id = q.methodId,
                                method = q.method
                            },
                            oCountry = new CountryLookup()
                            {
                                id = q.countryId,
                                nameOfcountry = q.nameOfcountry
                            }
                        };

                        dzones.Add(zone);
                    }

                    response = new DefaultAPIResponse() { 
                        status = true,
                        message = @"success",
                        data = dzones
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }
                
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
        public async Task<DefaultAPIResponse> CreateDeliveryZoneAsync(DeliveryZoneLookup payLoad)
        {
            try
            {
                var record = await config.TDeliveryZones.Where(x => x.Zone == payLoad.zoneName).Where(x => x.DeliverymethodId == payLoad.oDeliveryMethod.id).Where(x => x.CountryId == payLoad.oCountry.id).FirstOrDefaultAsync();
                if (record == null)
                {
                    TDeliveryZone zoneObj = new TDeliveryZone()
                    {
                        Zone = payLoad.zoneName,
                        DeliverymethodId = payLoad.oDeliveryMethod.id,
                        CountryId = payLoad.oCountry.id
                    };

                    await config.AddAsync(zoneObj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Delivery zone '{payLoad.zoneName}' added successfully to the data store",
                        data = payLoad
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = $"Delivery zone '{payLoad.zoneName}' already exist for country '{payLoad.oCountry.nameOfcountry}'" }; }

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


        #endregion

        #region HS-Codes

        public async Task<DefaultAPIResponse> GetHSCodeListAsync()
        {
            //get HS code resource 
            List<HSCodeLookup> hscodes = null;
            try
            {
                var records = await config.Thscodes.ToListAsync();
                if (records != null)
                {
                    hscodes = new List<HSCodeLookup>();
                    foreach(var record in records)
                    {
                        var obj = new HSCodeLookup()
                        {
                            id = record.Id,
                            code = record.Hscode,
                            description = record.Description
                        };

                        hscodes.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = hscodes
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }
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
        public async Task<DefaultAPIResponse> CreateHSCodAsync(HSCodeLookup payLoad)
        {
            //create an HS code resource
            try
            {
                var record = await config.Thscodes.Where(hs => hs.Hscode == payLoad.code).FirstOrDefaultAsync();
                if (record == null)
                {
                    Thscode obj = new Thscode()
                    {
                        Hscode = payLoad.code,
                        Description = payLoad.description
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"HS Code '{payLoad.code}' added successfully to the data store",
                        data = payLoad
                    };
                }
                else
                {
                    record.Description = payLoad.description;
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = false,
                        message = $"HS Code '{payLoad.code}' already exist in the data store. Update effected",
                        data = payLoad
                    };
                }

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

        #endregion

        #region Insurance type

        public async Task<DefaultAPIResponse> GetInsuranceTypeListAsync()
        {
            List<InsuranceTypeLookup> results = null;

            try
            {
                var ins_types = await config.TInsuranceTypes.ToListAsync();
                if (ins_types != null)
                {
                    results = new List<InsuranceTypeLookup>();
                    foreach(var ins in ins_types)
                    {
                        var obj = new InsuranceTypeLookup()
                        {
                            id = ins.Id,
                            insuranceType = ins.InsuranceType
                        };

                        results.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = results
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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
        public async Task<DefaultAPIResponse> CreateInsuranceTypeAsync(InsuranceTypeLookup payLoad)
        {
            //create an insurance type resource
            try
            {
                var record = await config.TInsuranceTypes.Where(x => x.InsuranceType == payLoad.insuranceType).FirstOrDefaultAsync();
                if (record == null)
                {
                    TInsuranceType obj = new TInsuranceType()
                    {
                        InsuranceType = payLoad.insuranceType
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Insurance type '{payLoad.insuranceType}' added to the data store",
                        data = payLoad
                    };
                }
                else
                {
                    response = new DefaultAPIResponse()
                    {
                        status = false,
                        message = $"Insurance type '{payLoad.insuranceType}' already exist in the data store"
                    };
                }

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

        #endregion

        #region Insurance

        public DefaultAPIResponse GetInsuranceListAsync()
        {
            List<InsuranceLookup> insurance_list = null;
            try
            {
                var query = (from i in config.TInsurances
                             join ityp in config.TInsuranceTypes on i.InsuranceTypeId equals ityp.Id

                             select new
                             {
                                 Id = i.Id,
                                 description = i.Description,
                                 unitPrice = i.UnitPrice,
                                 insuranceTypeId = ityp.Id,
                                 insuranceType = ityp.InsuranceType
                             });

                if (query != null)
                {
                    insurance_list = new List<InsuranceLookup>();
                    foreach(var q in query)
                    {
                        var obj = new InsuranceLookup()
                        {
                            id = q.Id,
                            insuranceDescription = q.description,
                            unitPrice = (decimal)q.unitPrice,
                            oInsuranceType = new InsuranceTypeLookup()
                            {
                                id = q.insuranceTypeId,
                                insuranceType = q.insuranceType
                            }
                        };

                        insurance_list.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = insurance_list
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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
        public async Task<DefaultAPIResponse> CreateInsuranceAsync(InsuranceLookup payLoad)
        {
            try
            {
                var record = await config.TInsurances.Where(i => i.InsuranceTypeId == payLoad.oInsuranceType.id).Where(z => z.UnitPrice == payLoad.unitPrice)
                                                     .Where(a => a.Description == payLoad.insuranceDescription).FirstOrDefaultAsync();

                if (record == null)
                {
                    TInsurance obj = new TInsurance()
                    {
                        InsuranceTypeId = payLoad.oInsuranceType.id,
                        Description = payLoad.insuranceDescription,
                        UnitPrice = payLoad.unitPrice
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Insurance '{payLoad.insuranceDescription}' created successfully in the data store",
                        data = payLoad
                    };
                }
                else
                {
                    record.Description = payLoad.insuranceDescription;
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = false,
                        message = $"Insurance '{payLoad.oInsuranceType.insuranceType}' already exist: Description updated in the data store"
                    };
                }

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

        #endregion

        #region Sailing Schedule

        public DefaultAPIResponse GetSailingScheduleListAsync()
        {
            //gets sailing schedule list
            List<SailingScheduleLookup> scheduleList = null;

            try
            {
                var query = (from tss in config.TSailingSchedules
                             join v in config.TVessels on tss.VesselId equals v.Id
                             join p in config.Tshippingports on tss.PortOfDepartureId equals p.Id
                             join pp in config.Tshippingports on tss.PortOfDepartureId equals pp.Id

                             select new
                             {
                                 Id = tss.Id,
                                 IdOfVessel = v.Id,
                                 nameOfVessel = v.VesselName,
                                 flagOfVessel = v.VesselFlag,
                                 departurePortId = p.Id,
                                 departurePort = p.NameOfport,
                                 departurePortCode = p.Portcode,
                                 arrivalPortId = pp.Id,
                                 arrivalPort = pp.NameOfport,
                                 arrivalPortCode = pp.Portcode,
                                 closingDate = tss.ClosingDate,
                                 departureDate = tss.DepartureDate,
                                 arrivalDate = tss.ArrivalDate
                             });

                if (query != null)
                {
                    scheduleList = new List<SailingScheduleLookup>();
                    foreach(var q in query)
                    {
                        var obj = new SailingScheduleLookup()
                        {
                            id = q.Id,
                            oVessel = new VesselLookup()
                            {
                                id = q.IdOfVessel,
                                nameOfvessel = q.nameOfVessel,
                                flagOfvessel = q.flagOfVessel
                            },
                            oDeparturePort = new ShippingPortLookup()
                            {
                                id = q.departurePortId,
                                nameOfport = q.departurePort,
                                codeOfport = q.departurePortCode
                            },
                            oArrivalPort = new ShippingPortLookup()
                            {
                                id = q.arrivalPortId,
                                nameOfport = q.arrivalPort,
                                codeOfport = q.arrivalPortCode
                            },
                            closingDate = (DateTime) q.closingDate,
                            dateOfdeparture = (DateTime) q.departureDate,
                            dateOfarrival = (DateTime) q.arrivalDate
                        };

                        scheduleList.Add(obj);
                    }

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = @"success",
                        data = scheduleList
                    };
                }
                else { response = new DefaultAPIResponse() { status = false, message = @"No data" }; }

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
        public async Task<DefaultAPIResponse> CreateSailingScheduleAsync(SailingScheduleLookup payLoad)
        {
            //create a sailing schedule resource
            try
            {
                /*
                var record = await config.TSailingSchedules.Where(x => x.VesselId == payLoad.oVessel.id)
                                                           .Where(a => a.PortOfDepartureId == payLoad.oDeparturePort.id)
                                                           .Where(b => b.PortOfArrivalId == payLoad.oArrivalPort.id)
                                                           .Where(c => c.ClosingDate == payLoad.closingDate)
                                                           .Where(d => d.DepartureDate == payLoad.dateOfdeparture)
                                                           .Where(e => e.ArrivalDate == payLoad.dateOfarrival).FirstOrDefaultAsync();
                */
                var record = (from x in config.TSailingSchedules
                           where x.VesselId == payLoad.oVessel.id
                           && x.PortOfDepartureId == payLoad.oDeparturePort.id
                           && x.PortOfArrivalId == payLoad.oArrivalPort.id
                           && x.ClosingDate == payLoad.closingDate
                           && x.DepartureDate == payLoad.dateOfdeparture
                           && x.ArrivalDate == payLoad.dateOfarrival

                           select new
                           {
                               vesselId = x.Id,
                               portOfDepartureId = x.PortOfDepartureId,
                               portOfArrivalId = x.PortOfArrivalId,
                               closingDate = x.ClosingDate,
                               departureDate = x.DepartureDate,
                               arivalDate = x.ArrivalDate
                           });

                if (record.Count() < 1)
                {
                    TSailingSchedule obj = new TSailingSchedule()
                    {
                        VesselId = payLoad.oVessel.id,
                        PortOfDepartureId = payLoad.oDeparturePort.id,
                        PortOfArrivalId = payLoad.oArrivalPort.id,
                        ClosingDate = payLoad.closingDate,
                        DepartureDate = payLoad.dateOfdeparture,
                        ArrivalDate = payLoad.dateOfarrival
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    response = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Schedule saved successfully into the data store",
                        data = payLoad
                    };
                }
                else
                {
                    response = new DefaultAPIResponse()
                    {
                        status = false,
                        message = $"Sailing schedule already exist in the data store",
                        data = payLoad
                    };
                }

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

        public async Task<UploadAPIResponse> UploadSailingScheduleAsync(IEnumerable<SailingScheduleLookup> payLoad)
        {
            //uploads a sailing schedule into the data store
            UploadAPIResponse rsp = null;
            int success = 0;
            int failed = 0;
            List<SailingScheduleLookup> successList = new List<SailingScheduleLookup>();
            List<SailingScheduleLookup> errorList = new List<SailingScheduleLookup>();
            List<string> errors = new List<string>();

            try
            {
                if (payLoad.Count() > 0)
                {
                    foreach(var schedule in payLoad)
                    {
                        try
                        {
                            var v = await config.TVessels.Where(a => a.VesselName == schedule.oVessel.nameOfvessel).FirstOrDefaultAsync();
                            if (v == null)
                            {
                                var dPort = await config.Tshippingports.Where(sp => sp.NameOfport == schedule.oDeparturePort.nameOfport).FirstOrDefaultAsync();
                                if (dPort != null)
                                {
                                    var aPort = await config.Tshippingports.Where(ap => ap.NameOfport == schedule.oArrivalPort.nameOfport).FirstOrDefaultAsync();
                                    if (aPort != null)
                                    {
                                        TSailingSchedule obj = new TSailingSchedule()
                                        {
                                            VesselId = schedule.oVessel.id,
                                            PortOfDepartureId = dPort.Id,
                                            PortOfArrivalId = aPort.Id,
                                            ClosingDate = schedule.closingDate,
                                            DepartureDate = schedule.dateOfdeparture,
                                            ArrivalDate = schedule.dateOfarrival
                                        };

                                        await config.AddAsync(obj);
                                        await config.SaveChangesAsync();

                                        successList.Add(schedule);
                                        success += 1;
                                    }
                                    else
                                    {
                                        errorList.Add(schedule);
                                        failed += 1;
                                    }
                                }
                                else
                                {
                                    errorList.Add(schedule);
                                    failed += 1;
                                }
                            }
                            else
                            {
                                failed += 1;
                                errors.Add($"Sailing schedule of '{schedule.oVessel.nameOfvessel}' departing port '{schedule.oDeparturePort.nameOfport}' and arriving at '{schedule.oArrivalPort.nameOfport}' already exist in the data store");
                            }
                        }
                        catch(Exception exc)
                        {
                            Debug.Print($"error: {exc.Message}");
                            errorList.Add(schedule);
                            failed += 1;
                        }                     
                    }

                    rsp = new UploadAPIResponse()
                    {
                        status = true,
                        message = $"Total records= {payLoad.Count().ToString()}, successful inserts= {success.ToString()}, failed inserts= {failed.ToString()}",
                        data = successList,
                        successCount = success,
                        errorList = errorList,
                        errorMessageList = errors,
                        errorCount = failed
                    };
                }

                return rsp;
            }
            catch(Exception x)
            {
                return rsp = new UploadAPIResponse()
                {
                    status = false,
                    message = $"error: {x.Message}",
                    errorMessageList = errors
                };
            }
        }


        #endregion

    }
}
