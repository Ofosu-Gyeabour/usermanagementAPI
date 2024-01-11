#nullable disable
using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public record clsFreightInput
    {
        public int agentId { get; set; }
        public int portId { get; set; }
        public decimal? cubic { get; set; }
    }
    public class clsFreight
    {
        private swContext config;

        public int id { get; set; }
        public decimal? cubic { get; set; } = 0m;
        public decimal? freightOne { get; set; } = 0m;
        public decimal? freightTwo { get; set; } = 0m;
        public decimal? freightThree { get; set; } = 0m;
        public decimal? freightFour { get; set; } = 0m;
        public decimal? barrelOne { get; set; } = 0m;
        public decimal? barrelTwo { get; set; } = 0m;
        public decimal? barrelThree { get; set; } = 0m;

        public clsFreight()
        {
            config = new swContext();
        }

        public async Task<decimal> determineFreightBand(clsFreightInput input)
        {
            //determines which band to use in calculation of freight cost
            decimal? firstBand = 0.3m;
            decimal? secondBand = 0.6m;
            decimal? thirdBand = 0.9m;
            decimal? fourthBand = 1.2m;

            decimal? xresult = 0m;

            try
            {
                var ob = await this.getAgentFreightCostAsync(input.agentId, input.portId);
                if (cubic == firstBand)
                {
                    xresult = (decimal) ob.barrelOne;
                }

                if ((cubic > firstBand) && (cubic < secondBand))
                {
                    xresult = (ob.barrelTwo / (firstBand * 2)) * cubic;
                }

                if (cubic == secondBand)
                {
                    xresult = (decimal)ob.barrelTwo;
                }

                if ((cubic > secondBand) && (cubic < thirdBand))
                {
                    xresult = (ob.barrelThree / (firstBand * 3)) * cubic;
                }

                if (cubic == thirdBand)
                {
                    xresult = (decimal)ob.barrelThree;
                }

                if ((cubic > thirdBand) && (cubic < fourthBand))
                {
                    xresult = (ob.freightFour / (firstBand * 4)) * cubic;
                }

                return (decimal)xresult;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task<clsFreight> getAgentFreightCostAsync(int agentId, int portId)
        {
            //TODO: determines freight cost to charge
            clsFreight obj = null;

            try
            {
                var query = (from ta in config.TAgencyRates
                             where ta.AgentId == agentId && ta.PortId == portId
                             select new
                             {
                                 id = ta.Id,
                                 freight1 = ta.Frgt1,
                                 freight2 = ta.Frgt2,
                                 freight3 = ta.Frgt3,
                                 freight4 = ta.Frgt4,
                                 barrel1 = ta.B1,
                                 barrel2 = ta.B2,
                                 barrel3 = ta.B3
                             });

                var queryList = await query.ToListAsync().ConfigureAwait(false);

                return obj = queryList.Select(x => new clsFreight()
                {
                    id = x.id,
                    freightOne = x.freight1,
                    freightTwo = x.freight2,
                    freightThree = x.freight3,
                    freightFour = x.freight4,
                    barrelOne = x.barrel1,
                    barrelTwo = x.barrel2,
                    barrelThree = x.barrel3
                }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
