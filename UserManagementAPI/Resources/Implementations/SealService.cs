#nullable disable
using UserManagementAPI.POCOs;
using UserManagementAPI.Response;
using UserManagementAPI.Resources.Interfaces;

namespace UserManagementAPI.Resources.Implementations
{
    public class SealService : ISealService
    {
        swContext config;

        public SealService()
        {
            config = new swContext();
        }
        public async Task<DefaultAPIResponse> GetSealTypeListAsync()
        {
            //gets seal types from data store
            DefaultAPIResponse rs = null;
            List<SealTypeLookup> sealtypes = null;

            try
            {
                var dta = await config.TSealTypes.ToListAsync();
                if (dta != null)
                {
                    sealtypes = new List<SealTypeLookup>();
                    foreach(var d in dta)
                    {
                        var obj = new SealTypeLookup()
                        {
                            id = d.Id,
                            sealDescription = d.SealTypeDescription
                        };

                        sealtypes.Add(obj);
                    }

                    rs = new DefaultAPIResponse() { 
                        status = true, 
                        message = @"success",
                        data = sealtypes
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
        public async Task<DefaultAPIResponse> GetSealPriceListAsync()
        {
            DefaultAPIResponse rs = null;
            List<SealPriceLookup> sealprices = null;

            try
            {
                var query = (from tsp in config.TSealPrices
                             join c in config.Tcompanies on tsp.CompanyId equals c.CompanyId
                             join tst in config.TSealTypes on tsp.SealtypeId equals tst.Id
                             select new
                             {
                                 id = tsp.Id,
                                 price = tsp.Price,
                                 sellingPrice = tsp.Sellingprice,
                                 companyId = c.CompanyId,
                                 nameOfcompany = c.Company,
                                 sealtypeId = tst.Id,
                                 sealtypeDescription = tst.SealTypeDescription
                             });

                if (query != null)
                {
                    sealprices = new List<SealPriceLookup>();
                    foreach(var q in query)
                    {
                        var sealprice = new SealPriceLookup()
                        {
                            id = q.id,
                            Price = (decimal) q.price,
                            sellingPrice = (decimal) q.sellingPrice,
                            oCompany = new CompanyLookup()
                            {
                                id = q.companyId,
                                nameOfcompany = q.nameOfcompany
                            },
                            oSealType = new SealTypeLookup()
                            {
                                id = q.sealtypeId,
                                sealDescription = q.sealtypeDescription
                            }
                        };
                    }
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


        public async Task<DefaultAPIResponse> CreateSealTypeAsync(SealTypeLookup payLoad)
        {
            //creates a seal type resource in the data store
            DefaultAPIResponse rs = null;

            try
            {
                var dt = await config.TSealTypes.Where(st => st.SealTypeDescription == payLoad.sealDescription).FirstOrDefaultAsync();
                if (dt == null)
                {
                    TSealType sealtype = new TSealType()
                    {
                        SealTypeDescription = payLoad.sealDescription
                    };

                    await config.AddAsync(sealtype);
                    await config.SaveChangesAsync();

                    rs = new DefaultAPIResponse()
                    {
                        status = true, 
                        message = $"Seal Type '{payLoad.sealDescription}' added to the data store",
                        data = payLoad
                    };
                }
                else { rs = new DefaultAPIResponse() { status = false, message = $"Seal Type '{payLoad.sealDescription}' already exist in the data store"}; }

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
        public async Task<DefaultAPIResponse> CreateSealPriceAsync(SealPriceLookup payLoad)
        {
            //creates a seal price resource
            DefaultAPIResponse rs = null;

            try
            {
                var dta = await config.TSealPrices.Where(sp => sp.SealtypeId == payLoad.oSealType.id).Where(x => x.CompanyId == payLoad.oCompany.id).FirstOrDefaultAsync();
                if (dta == null)
                {
                    TSealPrice sealprice = new TSealPrice()
                    {
                        Price = payLoad.Price,
                        Sellingprice = payLoad.sellingPrice,
                        CompanyId = payLoad.oCompany.id,
                        SealtypeId = payLoad.oSealType.id
                    };

                    await config.AddAsync(sealprice);
                    await config.SaveChangesAsync();

                    rs = new DefaultAPIResponse()
                    {
                        status = true,
                        message = $"Seal Price '{payLoad.oSealType.sealDescription}' added to company '{payLoad.oCompany.nameOfcompany}' in the data store",
                        data = payLoad
                    };
                }
                else { rs = new DefaultAPIResponse() { status = false, message = $"Seal Price '{payLoad.oSealType.sealDescription}' already exist for company '{payLoad.oCompany.nameOfcompany}' in the data store" }; }

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
