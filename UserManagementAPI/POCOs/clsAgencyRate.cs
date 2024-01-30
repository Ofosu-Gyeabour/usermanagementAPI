#nullable disable

using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class clsAgencyRate
    {
        private swContext config;

        public clsAgencyRate()
        {
            config = new swContext();
        }

        public int id { get; set; }
        public int portId { get; set; }
        public decimal? trade { get; set; }
        public decimal? agency { get; set; }
        public decimal? retail { get; set; }
        public decimal? freightOne { get; set; }
        public decimal? freightTwo { get; set; }
        public decimal? freightThree { get; set; }
        public decimal? freightFour { get; set; }
        public decimal? barrelOne { get; set; }
        public decimal? barrelTwo { get; set; }
        public decimal? barrelThree { get; set; }
        public decimal? surcharge { get; set; }
        public decimal? minimum { get; set; }
        public int? rateType { get; set; }
        public int? agentId { get; set; }

        public async Task<clsAgencyRate> Get()
        {
            //gets a agency rate record using portId and agencyId
            clsAgencyRate objAgencyRate = null;

            try
            {
                var query = (from ag in config.TAgencyRates
                             where ag.PortId == this.portId && ag.AgentId == this.agentId
                             select new
                             {
                                 id = ag.Id,
                                 portId = ag.PortId,
                                 tradeRate = ag.Trade,
                                 retailRate = ag.Retail,
                                 freightRateOne = ag.Frgt1,
                                 freightRateTwo = ag.Frgt2,
                                 freightRateThree = ag.Frgt3,
                                 freightRateFour = ag.Frgt4,
                                 freightBarrelOne = ag.B1,
                                 freightBarrelTwo = ag.B2,
                                 freightBarrelThree = ag.B3,
                                 surchargeRate = ag.Surcharge,
                                 minimumRate = ag.Minimum,
                                 rateTypeID = ag.RtypeId == null? 0: ag.RtypeId 
                             });

                var queryRecord = await query.ToListAsync().ConfigureAwait(false);
                objAgencyRate = queryRecord
                                    .Select(a => new clsAgencyRate()
                                    {
                                        id = a.id,
                                        portId = (int) a.portId,
                                        trade = a.tradeRate,
                                        retail = a.retailRate,
                                        freightOne = a.freightRateTwo,
                                        freightTwo = a.freightRateTwo,
                                        freightThree = a.freightRateThree,
                                        freightFour = a.freightRateFour,
                                        barrelOne = a.freightBarrelOne,
                                        barrelTwo = a.freightBarrelTwo,
                                        barrelThree = a.freightBarrelThree,
                                        surcharge = a.surchargeRate,
                                        minimum = a.minimumRate,
                                        rateType = a.rateTypeID
                                    }).FirstOrDefault();

                return objAgencyRate;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
   
        public async Task<clsAgencyRate> GetNonAgentRate()
        {
            //gets a rate for a non-agent
            clsAgencyRate objAgencyRate = null;
            const int ZERO = 0;

            try
            {
                var query = (from ag in config.TAgencyRates
                             where ag.PortId == this.portId && ag.RtypeId == this.rateType && ag.AgentId == ZERO
                             select new
                             {
                                 id = ag.Id,
                                 portId = ag.PortId,
                                 tradeRate = ag.Trade,
                                 retailRate = ag.Retail,
                                 freightRateOne = ag.Frgt1,
                                 freightRateTwo = ag.Frgt2,
                                 freightRateThree = ag.Frgt3,
                                 freightRateFour = ag.Frgt4,
                                 freightBarrelOne = ag.B1,
                                 freightBarrelTwo = ag.B2,
                                 freightBarrelThree = ag.B3,
                                 surchargeRate = ag.Surcharge,
                                 minimumRate = ag.Minimum,
                                 rateTypeID = ag.RtypeId == null ? 0 : ag.RtypeId
                             });

                var queryRecord = await query.ToListAsync().ConfigureAwait(false);
                objAgencyRate = queryRecord
                                    .Select(a => new clsAgencyRate()
                                    {
                                        id = a.id,
                                        portId = (int)a.portId,
                                        trade = a.tradeRate,
                                        retail = a.retailRate,
                                        freightOne = a.freightRateTwo,
                                        freightTwo = a.freightRateTwo,
                                        freightThree = a.freightRateThree,
                                        freightFour = a.freightRateFour,
                                        barrelOne = a.freightBarrelOne,
                                        barrelTwo = a.freightBarrelTwo,
                                        barrelThree = a.freightBarrelThree,
                                        surcharge = a.surchargeRate,
                                        minimum = a.minimumRate,
                                        rateType = a.rateTypeID
                                    }).FirstOrDefault();

                return objAgencyRate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class clsCalculatorZone
    {
        private swContext config;

        public clsCalculatorZone()
        {
            //config = new swContext();
        }

        public int Id { get; set; }
        public int PortId { get; set; }
        public int ZoneId { get; set; }
        public decimal? freightBar1 { get; set; }
        public decimal? freightBar2 { get; set; }
        public decimal? freightBar3 { get; set; }
        public decimal? freightBar4 { get; set; }
        public decimal? freightBar5 { get; set; }
        public decimal? minimumCharge { get; set; }
        public decimal? duty { get; set; }


        public async Task<clsCalculatorZone> getCalculatorRecord()
        {
            //TODO: uses the port and zone IDs to get a record to use for the computation
            clsCalculatorZone obj = null;

            try
            {
                config = new swContext();
                var query = (from calc in config.TCalculators
                             where calc.PId == this.PortId && calc.ZId == this.ZoneId
                             select new
                             {
                                 id = calc.Id,
                                 f1 = calc.Frgtbar1,
                                 f2 = calc.Frgtbar2,
                                 f3 = calc.Frgtbar3,
                                 f4 = calc.Frgtbar4,
                                 f5 = calc.Frgtbar5
                             });

                var queryList = await query.ToListAsync().ConfigureAwait(false);

                obj = queryList
                            .Select(a => new clsCalculatorZone()
                            {
                                Id = a.id,
                                freightBar1 = a.f1,
                                freightBar2 = a.f2,
                                freightBar3 = a.f3,
                                freightBar4 = a.f4,
                                freightBar5 = a.f5
                            }).FirstOrDefault();

                return obj;
            }
            catch(Exception x)
            {
                throw x;
            }
        }
    }
}
