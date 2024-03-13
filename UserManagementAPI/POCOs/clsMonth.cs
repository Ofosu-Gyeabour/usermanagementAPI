#nullable disable
namespace UserManagementAPI.POCOs
{
    public class clsMonth
    {
        public int id { get; set; }
        public string nameOfmonth { get; set; }

        public async Task<IEnumerable<clsMonth>> Get()
        {
            //todo: gets all the months in the months table
            List<clsMonth> list = null;

            try
            {
                var config = new swContext();
                using (config)
                {
                    var q = (from tm in config.TMonths
                             select new
                             {
                                 id = tm.Id,
                                 monthName = tm.Month
                             });

                    var qList = await q.ToListAsync().ConfigureAwait(false);
                    return list = qList
                                     .Select(a => new clsMonth()
                                     {
                                         id = a.id,
                                         nameOfmonth = a.monthName
                                     }).ToList();
                }
            }
            catch(Exception ex)
            {
                return list;
            }
        }
        public async Task<string> Get(string monthCode)
        {
            //gets month using month code
            string result = string.Empty;

            var d = new Dictionary<string, string>();

            //populate with months
            d.Add(@"01", @"JANUARY");
            d.Add(@"02", @"FEBRUARY");
            d.Add(@"03", @"MARCH");
            d.Add(@"04", @"APRIL");
            d.Add(@"05", @"MAY");
            d.Add(@"06", @"JUNE");
            d.Add(@"07", @"JULY");
            d.Add(@"08", @"AUGUST");
            d.Add(@"09", @"SEPTEMBER");
            d.Add(@"10", @"OCTOBER");
            d.Add(@"11", @"NOVEMBER");
            d.Add(@"12", @"DECEMBER");

            try
            {
                result = d[monthCode];
                return result;
            }
            catch(Exception rex)
            {
                return result;
            }
        }

    }
}
