#nullable disable
using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class clsZone
    {
        private swContext config;

        public clsZone()
        {
            config = new swContext();
        }

        public int id { get; set; }
        public string? nameOfzone { get; set; } = string.Empty;
        public CountryLookup? oCountry { get; set; }

        public async Task<IEnumerable<clsZone>> getZonesAsync()
        {
            //TODO: gets zones from the data store
            List<clsZone> zones = null;

            try
            {
                var q = (from z in config.TZones
                         join cnt in config.TCountryLookups on z.CountryId equals cnt.CountryId
                         select new
                         {
                             id = z.Id,
                             zoneName = z.ZoneName,
                             countryId = z.CountryId,
                             countryName = cnt.CountryName
                         });

                var qList = await q.ToListAsync().ConfigureAwait(false);

                return zones = qList.Select(x => new clsZone()
                {
                    id = x.id,
                    nameOfzone = x.zoneName,
                    oCountry = new CountryLookup()
                    {
                        id = (int) x.countryId,
                        nameOfcountry = x.countryName
                    }
                }).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<clsZone> getZoneAsync(clsParish obj)
        {
            //TODO: gets zone from parish using parishId
            clsZone zone = null;

            try
            {
                var qry = (from z in config.TZones
                           join p in config.TParishes on z.Id equals p.ZoneId
                           join cnt in config.TCountryLookups on z.CountryId equals cnt.CountryId
                           where p.Id == obj.id
                           select new
                           {
                               id = z.Id,
                               zoneName = z.ZoneName,
                               countryId = z.CountryId,
                               countryName = cnt.CountryName
                           });

                var qryList = await qry.ToListAsync().ConfigureAwait(false);
                return zone = qryList.Select(x => new clsZone()
                {
                    id = x.id,
                    nameOfzone = x.zoneName,
                    oCountry = new CountryLookup()
                    {
                        id = (int)x.countryId,
                        nameOfcountry = x.countryName
                    }
                }).FirstOrDefault();
            }
            catch(Exception x)
            {
                throw x;
            }
        }

    }
}
