#nullable disable
namespace UserManagementAPI.POCOs
{
    public class clsShipmentReport
    {
        #region Properties

        public int shippingId { get; set; }
        public string bolNo { get; set; } = string.Empty; //bill of laden number
        public string consignee { get; set; }
        public decimal wifduties { get; set; } = 0m;
        public decimal jtsduties { get; set; } = 0m;
        public decimal earningsOnDuties { get; set; } = 0m;
        public decimal wifCD { get; set; } = 0m;
        public decimal jtsCD { get; set; } = 0m;
        public decimal earningsOnCD { get; set; } = 0m;
        public decimal totalEarnings { get; set; } = 0m;
        public decimal cbm { get; set; } = 0m;
        public string parish { get; set; } = string.Empty;

        public decimal roe { get; set; } = 0m;
        public DateTime dte { get; set; } = DateTime.Now;  //now set to creation date. change to sailing date when system is up and running

        #endregion

        #region methods

        public async Task<IEnumerable<clsShipmentReport>> GetShipmentReportAsync(DateTime df, DateTime dt)
        {
            //TODO: gets shipment report for the specified date range
            //find a way to add the parish

            List<clsShipmentReport> results = null;

            try
            {
                var config = new swContext();
                using (config)
                {
                    var q = (from ts in config.TShippings
                             join cl in config.TClients on ts.ConsignorId equals cl.Id
                             join tsc in config.Tshippingordercommissions on ts.Id equals tsc.Orderid

                             where ts.OrderCreationDate >= df && ts.OrderCreationDate <= dt
                             select new
                             {
                                 id = ts.Id,
                                 bolNo = ts.BolNo,
                                 consigneeId = ts.ConsignorId,
                                 consignee = cl.ClientTypeId == 1 ? $"{cl.Firstname} {cl.Middlenames} {cl.Surname}" : $"{cl.ClientBusinessName}",
                                 wifdt = tsc.Wifduties,
                                 jtsdt = tsc.Jtsduties,
                                 dtearnings = tsc.Earningsonduties,
                                 wifcd = tsc.Wifcd,
                                 jtscd = tsc.Jtscd,
                                 cdearnings = tsc.Earningsoncd,
                                 totalearnings = 0m, //(tsc.Earningsonduties + tsc.Earningsoncd),
                                 cubic = tsc.Cbm,
                                 referenceDate = ts.OrderCreationDate
                             }); 

                    var qList = await q.ToListAsync().ConfigureAwait(false);

                    results = qList
                                  .Select(a => new clsShipmentReport()
                                  {
                                      shippingId = a.id,
                                      bolNo = a.bolNo,
                                      consignee = a.consignee,
                                      wifduties = (decimal) a.wifdt,
                                      jtsduties = (decimal) a.jtsdt,
                                      earningsOnDuties = (decimal) a.dtearnings,
                                      wifCD = (decimal) a.wifcd,
                                      jtsCD = (decimal) a.jtscd,
                                      earningsOnCD = (decimal) a.cdearnings,
                                      totalEarnings = (decimal) a.totalearnings,
                                      cbm = (decimal) a.cubic,

                                      dte = (DateTime) a.referenceDate
                                  }).ToList();

                    return results;
                }
            }
            catch(Exception x)
            {
                throw x;
            }
        }

        public async Task<decimal> getRateForDate(DateTime date)
        {
            //TODO: gets the rate for the date specified
            return 0m;
        }

        #endregion

    }
}
