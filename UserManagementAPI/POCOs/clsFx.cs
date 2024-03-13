#nullable disable

using System.Diagnostics;
using UserManagementAPI.Models;

namespace UserManagementAPI.POCOs
{
    public class clsFx
    {
        public clsFx()
        {
            
        }

        public int Id { get; set; }
        public decimal usdgbp { get; set; } = 0m;
        public decimal usdeur { get; set; } = 0m;
        public DateTime forexDate { get; set; }


        #region methods

        public async Task<IEnumerable<clsFx>> Get()
        {
            //TODO: gets all stored fx values from the database
            List<clsFx> list = new List<clsFx>();

            try
            {
                var config = new swContext();
                using (config)
                {
                    var q = (from tf in config.TFxes
                             where tf.Id > 0
                             select new
                             {
                                 id = tf.Id,
                                 usdgbp = tf.Usdgbp,
                                 usdeur = tf.Usdeur,
                                 fdate = tf.FxDate
                             });

                    var qList = await q.ToListAsync().ConfigureAwait(false);

                    list = qList
                              .Select(a => new clsFx()
                              {
                                  Id = a.id,
                                  usdgbp = (decimal)a.usdgbp,
                                  usdeur = (decimal)a.usdeur,
                                  forexDate = (DateTime)a.fdate
                              }).ToList();

                    return list;
                }
            }
            catch(Exception x)
            {
                return list;
            }
        }

        public async Task<clsFx> Get(DateTime dt)
        {
            //TODO: gets the current forex rate from the data store
            clsFx fx = null;
            
            try
            {
                var config = new swContext();
                using (config)
                {
                    var q = (from tf in config.TFxes
                             where tf.FxDate == dt
                             select new
                             {
                                 id = tf.Id,
                                 usdgbp = tf.Usdgbp,
                                 usdeur = tf.Usdeur,
                                 fdate = tf.FxDate
                             });

                    var qList = await q.ToListAsync().ConfigureAwait(false);

                    fx = qList
                             .Select(a => new clsFx()
                             {
                                 Id = a.id,
                                 usdgbp = (decimal)a.usdgbp,
                                 usdeur = (decimal)a.usdeur,
                                 forexDate = (DateTime)a.fdate
                             }).FirstOrDefault();

                    return fx;
                }
            }
            catch(Exception x)
            {
                return fx;
            }
        }

        public async Task<bool> AddToDbAsync(clsFx fx)
        {
            //TODO: adds fx record to the data store
            bool bln = false;

            try
            {
                using (var config = new swContext())
                {
                    TFx tfx = new TFx()
                    {
                        Usdgbp = fx.usdgbp,
                        Usdeur = fx.usdeur,
                        FxDate = fx.forexDate
                    };

                    await config.AddAsync(tfx);
                    await config.SaveChangesAsync();

                    bln = true;
                }

                return bln;
            }
            catch(Exception x)
            {
                return bln;
            }
        }

        #endregion

    }
}
