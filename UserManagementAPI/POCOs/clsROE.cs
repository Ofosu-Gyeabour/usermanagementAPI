#nullable disable

using UserManagementAPI.Models;

namespace UserManagementAPI.POCOs
{
    public class clsROE
    {
        public int Id { get; set; }
        public string month { get; set; }
        public int year { get; set; }
        public decimal roe { get; set; }
        public decimal jamaicanDollar { get; set; }
        public decimal fx { get; set; }

        public async Task<IEnumerable<clsROE>> Get()
        {
            //TODO: get all rate of exchange in the data store
            List<clsROE> list = new List<clsROE>();

            try
            {
                var config = new swContext();
                using (config)
                {
                    try
                    {
                        var q = (from ro in config.TjtsRoes
                                 select new
                                 {
                                     id = ro.Id,
                                     month = ro.Mth,
                                     year = ro.Yr,
                                     rateOfexchange = ro.Roe,
                                     jam = ro.Jam,
                                     forex = ro.Fx
                                 });

                        var qList = await q.ToListAsync().ConfigureAwait(false);

                        return list = qList
                                  .Select(a => new clsROE()
                                  {
                                      Id = a.id,
                                      month = a.month,
                                      year = (int) a.year,
                                      roe = (decimal) a.rateOfexchange,
                                      jamaicanDollar = (decimal) a.jam,
                                      fx = (decimal)a.forex
                                  }).ToList();
                    }
                    catch(Exception qEr)
                    {
                        return list;
                    }
                }
            }
            catch(Exception ex)
            {
                return list;
            }
        }

        public async Task<clsROE> Get(int id)
        {
            //TODO: gets a rate of exchange using Id
            clsROE obj = null;

            try
            {
                using (var config = new swContext())
                {
                    var o = await config.TjtsRoes.Where(jt => jt.Id == id).FirstOrDefaultAsync();
                    obj = o != null ? new clsROE() { 
                        Id = o.Id,
                        month = o.Mth,
                        year = (int) o.Yr,
                        roe = (decimal) o.Roe,
                        jamaicanDollar = (decimal) o.Jam,
                        fx = (decimal) o.Fx
                    } : null;
                }

                return obj;
            }
            catch(Exception x)
            {
                throw x;
            }
        }

        public async Task<clsROE> Get(string month, int year)
        {
            //TODO: gets a rate of exchange using month and year 
            clsROE obj = null;

            try
            {
                using(var config = new swContext())
                {
                    var o = await config.TjtsRoes.Where(jt => jt.Mth == month).Where(j => j.Yr == year).FirstOrDefaultAsync();
                    obj = o != null ? new clsROE()
                    {
                        Id = o.Id,
                        month = o.Mth,
                        year = (int)o.Yr,
                        roe = (decimal)o.Roe,
                        jamaicanDollar = (decimal) o.Jam,
                        fx = (decimal)o.Fx
                    } : null;
                }

                return obj;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AddRateOfExchangeAsync()
        {
            //TOOD: adds a record to the rate of exchange data store in the data base
            bool bln = false;

            try
            {
                var config = new swContext();
                using (config)
                {
                    TjtsRoe obj = new TjtsRoe() { 
                        Mth = month,
                        Yr = year,
                        Roe = roe,
                        Jam = jamaicanDollar,
                        Fx = fx
                    };

                    await config.AddAsync(obj);
                    await config.SaveChangesAsync();

                    this.Id = obj.Id;

                    bln = true;
                }

                return bln;
            }
            catch(Exception ex)
            {
                return bln;
            }
        }

        public async Task<decimal> computeForex(decimal pRoe)
        {
            //TODO: computes the forex value based on the rate of exchange value passed
            //should I do this on the client side?
            return 0m;
        }
    
       

    }
}
