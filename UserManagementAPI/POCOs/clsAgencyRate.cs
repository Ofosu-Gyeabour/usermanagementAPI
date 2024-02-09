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
                                        freightOne = a.freightRateOne,
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
        public int ParishId { get; set; }
        public int ZoneId { get; set; }
        public decimal? freightBar1 { get; set; }
        public decimal? freightBar2 { get; set; }
        public decimal? freightBar3 { get; set; }
        public decimal? freightBar4 { get; set; }
        public decimal? freightBar5 { get; set; }
        public decimal? minimumCharge { get; set; }
        public decimal? duty { get; set; }
        public decimal? m1 { get; set; }
        public decimal? m2 { get; set; }
        public decimal? m3 { get; set; }
        public decimal? m4 { get; set; }
        public decimal? m5 { get; set; }

        public async Task<clsCalculatorZone> getCalculatorZoneRecordAsync()
        {
            //TODO: uses the port and zone IDs to get a record to use for the computation
            clsCalculatorZone obj = null;

            try
            {
                config = new swContext();
                var query = (from calc in config.TD2dukDeliveries
                             where calc.PId == this.ParishId && calc.ZoneId == this.ZoneId
                             select new
                             {
                                 id = calc.Id,
                                 f1 = calc.Fb1,
                                 f2 = calc.Fb2,
                                 f3 = calc.Fb3,
                                 f4 = calc.Fb4,
                                 f5 = calc.Additionalfb,
                                 min = calc.Minimum,
                                 m1 = calc.M1,
                                 m2 = calc.M2,
                                 m3 = calc.M3,
                                 m4 = calc.M4,
                                 m5 = calc.M5
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
                                freightBar5 = a.f5, 

                                minimumCharge = a.min,
                                m1 = a.m1,
                                m2 = a.m2,
                                m3 = a.m3,
                                m4 = a.m4,
                                m5 = a.m5
                            }).FirstOrDefault();

                return obj;
            }
            catch(Exception x)
            {
                throw x;
            }
        }

        public async Task<clsCalculatorZone> getJTSZoneRecordAsync()
        {
            //TODO: uses the port and zone IDs to get a record to use for the JTS computation
            clsCalculatorZone obj = null;
            try
            {
                config = new swContext();
                var query = (from calc in config.TD2djamaicaDeliveries
                             where calc.PId == this.ParishId && calc.ZoneId == this.ZoneId
                             select new
                             {
                                 id = calc.Id,
                                 f1 = calc.Fb1,
                                 f2 = calc.Fb2,
                                 f3 = calc.Fb3,
                                 f4 = calc.Fb4,
                                 f5 = calc.Additionalfb,
                                 min = calc.Minimum,
                                 m1 = calc.M1,
                                 m2 = calc.M2,
                                 m3 = calc.M3,
                                 m4 = calc.M4,
                                 m5 = calc.M5
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
                                freightBar5 = a.f5,

                                minimumCharge = a.min,
                                m1 = a.m1,
                                m2 = a.m2,
                                m3 = a.m3,
                                m4 = a.m4,
                                m5 = a.m5
                            }).FirstOrDefault();

                return obj;
            }
            catch (Exception x)
            {
                throw x;
            }
        }

    }
}
