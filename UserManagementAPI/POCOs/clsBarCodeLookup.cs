#nullable disable
using Microsoft.AspNetCore.Server.IIS.Core;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UserManagementAPI.Models;

namespace UserManagementAPI.POCOs
{
    public class clsBarCodeLookup
    {
        public clsBarCodeLookup()
        {
            
        }

        public int Id { get; set; }
        public string operationBarCode { get; set; }
        public string operationDescription { get; set; }

        public async Task<IEnumerable<clsBarCodeLookup>> ListOperationalBarCodesAsync()
        {
            //TODO: get a list of all operational barcodes from the data store
            List<clsBarCodeLookup> barcodelist = new List<clsBarCodeLookup>();

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var query = (from tbc in config.TBarCodeOps
                                     where tbc.Id > 0
                                     select new
                                     {
                                         id = tbc.Id,
                                         opbarcode = tbc.Opbcode,
                                         description = tbc.BcodeDescrib
                                     });

                        var queryList = await query.ToListAsync().ConfigureAwait(false);
                        barcodelist = queryList
                                          .Select(a => new clsBarCodeLookup()
                                          {
                                              Id = a.id,
                                              operationBarCode = a.opbarcode,
                                              operationDescription = a.description
                                          }).ToList();

                        return barcodelist;
                    }
                    catch(Exception configErr)
                    {
                        throw configErr;
                    }
                }
            }
            catch(Exception x)
            {
                return barcodelist;
            }
        }

        public async Task<string> GenerateRandomNumbers(string opBarCode)
        {
            //TODO: generates random numbers
            string randomNos = string.Empty;
            string result = string.Empty;

            try
            {
                Random rnd = new Random();

                //generates the next 7 random numbers
                for(int i = 0; i < 7; i++)
                {
                    randomNos += rnd.Next(10);  //returns random numbers less than 10
                }

                result = string.Format("{0}{1}", opBarCode, randomNos);
                return result;
            }
            catch(Exception x)
            {
                return result;
            }
        }
    }

    public class clsBarCodeGenerator
    {
        public clsBarCodeGenerator()
        {

        }

        public int Id { get; set; }
        public int barcodeId { get; set; }
        public string generatedBarCode { get; set; }
        public DateTime generatedDate { get; set; }

        public string Location { get; set; } = string.Empty;

        public string barcodeIdValue { get; set; }

        public async Task<bool> AddOpBarCodeAndLocationAsync()
        {
            //TODO: adds generated barcode and the warehouse section to the data store
            bool bln = false;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var trans = config.Database.BeginTransaction();

                        TBarCodeGenerator barcodeGenerator = new TBarCodeGenerator()
                        {
                            BarcodeId = barcodeId,
                            Genbarcode = generatedBarCode,
                            Dte = generatedDate
                        };

                        await config.AddAsync(barcodeGenerator);
                        await config.SaveChangesAsync();

                        TwhouseSection warehouse_section = new TwhouseSection() { 
                            WhouseSection = Location,
                            AssocBarcode = generatedBarCode
                        };

                        await config.AddAsync(warehouse_section);
                        await config.SaveChangesAsync();

                        //commit transaction
                        await trans.CommitAsync();
                        bln = true;
                    }
                    catch(Exception configErr)
                    {
                        throw configErr;
                    }
                }

                return bln;
            }
            catch(Exception x)
            {
                return bln;
            }
        }

        public async Task<bool> AddAsync()
        {
            //TODO: adds generated barcode to the data store
            bool bln = false;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        TBarCodeGenerator barcodeGenerator = new TBarCodeGenerator() { 
                            BarcodeId = barcodeId,
                            Genbarcode = generatedBarCode,
                            Dte = generatedDate
                        };

                        await config.AddAsync(barcodeGenerator);
                        await config.SaveChangesAsync();

                        bln = true;
                    }
                    catch(Exception configErr)
                    {
                        throw configErr;
                    }
                }

                return bln;
            }
            catch(Exception x)
            {
                return bln;
            }
        }

        public async Task<IEnumerable<clsBarCodeGenerator>> ListAsync()
        {
            //TODO: gets the list of barcode generators out there
            List<clsBarCodeGenerator> list = new List<clsBarCodeGenerator>();

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var q = (from tbc in config.TBarCodeGenerators
                                 join tbo in config.TBarCodeOps on tbc.BarcodeId equals tbo.Id
                                 select new
                                 {
                                     id = tbc.Id,
                                     barcodeid = tbc.BarcodeId,
                                     barcode = tbo.Opbcode,
                                     genbarcode = tbc.Genbarcode,
                                     dte = tbc.Dte
                                 });

                        var qList = await q.ToListAsync().ConfigureAwait(false);
                        list = qList
                                   .Select(a => new clsBarCodeGenerator()
                                   {
                                       Id = a.id,
                                       barcodeId = (int) a.barcodeid,
                                       barcodeIdValue = a.barcode,
                                       generatedBarCode = a.genbarcode,
                                       generatedDate = (DateTime) a.dte
                                   }).ToList();
                        return list;
                    }
                    catch(Exception configErr)
                    {
                        throw configErr;
                    }
                }
            }
            catch(Exception x)
            {
                return list;
            }
        }
    }

    public class clsWarehouse
    {
        public int Id { get; set; }
        public string warehouseSection { get; set; }
        public string associatedBarCode { get; set; }

        public async Task<IEnumerable<clsWarehouse>> ListSectionsAsync()
        {
            //TODO: list all the sections in the warehouse
            List<clsWarehouse> list = null;
            const int ZERO = 0;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var qry = (from tw in config.TwhouseSections
                                   where tw.Id > ZERO
                                   select new
                                   {
                                       id = tw.Id,
                                       whouseSection = tw.WhouseSection,
                                       assocBarCode = tw.AssocBarcode
                                   });

                        var qryList = await qry.ToListAsync().ConfigureAwait(false);
                        list = qryList
                                    .Select(a => new clsWarehouse()
                                    {
                                        Id = a.id,
                                        warehouseSection = a.whouseSection,
                                        associatedBarCode = a.assocBarCode
                                    }).ToList();

                        return list;
                    }
                    catch(Exception cErr)
                    {
                        throw cErr;
                    }
                }
            }
            catch(Exception x)
            {
                return list;
            }
        }

        public async Task<int> getID(string barcode)
        {
            //gets the id for an associated barcode

            int result = 0;

            try
            {
                swContext config = new swContext();
                using (config)
                {
                    try
                    {
                        var obj = await config.TwhouseSections.Where(x => x.AssocBarcode == barcode).FirstOrDefaultAsync();

                        if (obj != null)
                        {
                            result = obj.Id;
                        }
                    }
                    catch(Exception x)
                    {
                        throw x;
                    }
                }

                return result;
            }
            catch(Exception e)
            {
                return result;
            }
        }

    }

}
