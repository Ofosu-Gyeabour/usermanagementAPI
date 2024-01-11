#nullable disable
using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class clsParish
    {
        private swContext config;

        public clsParish()
        {
            config = new swContext();
        }

        #region Properties

        public int id { get; set; }
        public string? nameOfparish { get; set; } = string.Empty;
        public clsZone? oZone { get; set; }

        #endregion

        #region Methods

        public async Task<IEnumerable<clsParish>> getAllParishesAsync()
        {
            //TODO: gets all parishes in the data store
            List<clsParish> parishes = null;

            try
            {
                var q = (from p in config.TParishes
                         join z in config.TZones on p.ZoneId equals z.Id
                         select new
                         {
                             id = p.Id,
                             parish = p.ParishName,
                             zoneId = p.ZoneId,
                             zoneName = z.ZoneName
                         });

                var qList = await q.ToListAsync().ConfigureAwait(false);

                return parishes = qList.Select(x => new clsParish()
                {
                    id = x.id,
                    nameOfparish = x.parish,
                    oZone = new clsZone()
                    {
                        id = (int)x.zoneId,
                        nameOfzone = x.zoneName
                    }
                }).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
