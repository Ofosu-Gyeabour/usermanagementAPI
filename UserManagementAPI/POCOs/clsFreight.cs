#nullable disable
using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public record clsFreightInput
    {
        public int agentId { get; set; }
        public int portId { get; set; }
        public int rateID { get; set; }
        public CountryLookup? oCountry { get; set; }
        public ReferralLookup oReference { get; set; }
        public decimal? cubic { get; set; }
        public int sysuserId { get; set; }
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
        public decimal barrelOne { get; set; } = 0m;
        public decimal? barrelTwo { get; set; } = 0m;
        public decimal barrelThree { get; set; } = 0m;

        public clsFreight()
        {
            config = new swContext();
        }

        public async Task<OrderSummaryDetails> determineFreightBand(clsFreightInput input)
        {
            //determines which band to use in calculation of freight cost
            clsShippingItem barrelObj = null;
            clsAgencyRate agencyObj = null;

            decimal? xresult = 0m;
            OrderSummaryDetails o = new OrderSummaryDetails() { 
                id = 3,
                key = @"FREIGHT"
            };


            try
            {
                var chargeKeys = await o.Get();
                //var ob = await this.getAgentFreightCostAsync(input.agentId, input.portId);
                barrelObj = await this.getVolumeForBarrelAsync();
                if (input.sysuserId == 1051 || input.sysuserId == 1053)
                {
                    input.rateID = 3;  //set to birmingham for rowan
                }

                if (input.oReference.id == 10 || input.oReference.id == 22)
                {
                    agencyObj = await new clsAgencyRate() {
                        portId = input.portId,
                        agentId = input.agentId
                    }.Get();
                }
                else
                {
                    agencyObj = await new clsAgencyRate() {
                        portId = input.portId,
                        rateType = input.rateID
                    }.GetNonAgentRate();
                }

                switch (input.rateID)
                {
                    case 1: //agent
                        if (barrelObj.volume == this.cubic)
                            xresult = agencyObj.barrelOne;

                        if (decimal.Multiply(barrelObj.volume, 2) == this.cubic)
                            xresult = agencyObj.barrelTwo;

                        if (decimal.Multiply(barrelObj.volume, 3) == this.cubic)
                            xresult = agencyObj.barrelThree;

                        if (cubic > barrelObj.volume && cubic < decimal.Multiply(barrelObj.volume, 2))
                        {
                            xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.barrelTwo, decimal.Multiply(barrelObj.volume, 2)), (decimal)cubic);
                            xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                        }

                        if (cubic > decimal.Multiply(barrelObj.volume,2) && cubic < decimal.Multiply(barrelObj.volume, 3))
                        {
                            xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.barrelThree, decimal.Multiply(barrelObj.volume, 3)), (decimal)cubic);
                            xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                        }

                        if (cubic > decimal.Multiply(barrelObj.volume, 3) && cubic <= 10m)
                        {
                            xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightOne);
                            xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                        }

                        if (cubic > 10m && cubic <= 20m)
                        {
                            xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightTwo);
                            xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                        }

                        if (cubic > 20m)
                        {
                            xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightThree);
                            xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                        }

                        //if ()
                        break;
                    case 2:
                        //agent in london OR normal customer in london
                        if (input.oReference.id == 10 || input.oReference.id == 22)  //agent
                        {
                            if (barrelObj.volume == this.cubic)
                                xresult = agencyObj.barrelOne;

                            if (decimal.Multiply(barrelObj.volume, 2) == this.cubic)
                                xresult = agencyObj.barrelTwo;

                            if (decimal.Multiply(barrelObj.volume, 3) == this.cubic)
                                xresult = agencyObj.barrelThree;

                            if (cubic > barrelObj.volume && cubic < decimal.Multiply(barrelObj.volume, 2))
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.barrelTwo, decimal.Multiply(barrelObj.volume, 2)), (decimal)cubic);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }

                            if (cubic > decimal.Multiply(barrelObj.volume, 2) && cubic < decimal.Multiply(barrelObj.volume, 3))
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.barrelThree, decimal.Multiply(barrelObj.volume, 3)), (decimal)cubic);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }

                            if (cubic > decimal.Multiply(barrelObj.volume, 3) && cubic <= 10m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightOne);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }

                            if (cubic > 10m && cubic <= 20m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightTwo);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }

                            if (cubic > 20m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightThree);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }
                        }
                        else
                        {
                            //customer in london
                            if (barrelObj.volume == this.cubic)
                                xresult = agencyObj.barrelOne;

                            if (decimal.Multiply(barrelObj.volume, 2) == this.cubic)
                                xresult = agencyObj.barrelTwo;

                            if (decimal.Multiply(barrelObj.volume, 3) == this.cubic)
                                xresult = agencyObj.barrelThree;

                            if (cubic > barrelObj.volume && cubic <= 0.45m)
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightOne, barrelObj.volume), (decimal)cubic);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > 0.45m && cubic < decimal.Multiply(barrelObj.volume, 2))
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightTwo, decimal.Multiply(barrelObj.volume, 2)), (decimal)cubic);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > decimal.Multiply(barrelObj.volume,2) && cubic < decimal.Multiply(barrelObj.volume, 3))
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightThree, decimal.Multiply(barrelObj.volume, 3)), (decimal)cubic);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > decimal.Multiply(barrelObj.volume,3) && cubic <= 2m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightOne);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > 2m && cubic <= 5m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightTwo);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > 5m && cubic <= 9m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightThree);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > 9m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightFour);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic < barrelObj.volume)
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightOne, barrelObj.volume), (decimal)cubic);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }
                        }
                        break;
                    default:
                        //agent in birmingham or customer in birmingham
                        if (input.oReference.id == 10 || input.oReference.id == 22)
                        {
                            if (barrelObj.volume == this.cubic)
                                xresult = agencyObj.barrelOne;

                            if (decimal.Multiply(barrelObj.volume, 2) == this.cubic)
                                xresult = agencyObj.barrelTwo;

                            if (decimal.Multiply(barrelObj.volume, 3) == this.cubic)
                                xresult = agencyObj.barrelThree;

                            if (cubic > barrelObj.volume && cubic < decimal.Multiply(barrelObj.volume, 2))
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.barrelTwo, decimal.Multiply(barrelObj.volume, 2)), (decimal)cubic);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }

                            if (cubic > decimal.Multiply(barrelObj.volume, 2) && cubic < decimal.Multiply(barrelObj.volume, 3))
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.barrelThree, decimal.Multiply(barrelObj.volume, 3)), (decimal)cubic);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }

                            if (cubic > decimal.Multiply(barrelObj.volume, 3) && cubic <= 10m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightOne);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }

                            if (cubic > 10m && cubic <= 20m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightTwo);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }

                            if (cubic > 20m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightThree);
                                xresult = xresult > agencyObj.barrelOne ? xresult : agencyObj.barrelOne;
                            }
                        }
                        else
                        {
                            //customer
                            if (barrelObj.volume == this.cubic)
                                xresult = agencyObj.barrelOne;

                            if (decimal.Multiply(barrelObj.volume, 2) == this.cubic)
                                xresult = agencyObj.barrelTwo;

                            if (decimal.Multiply(barrelObj.volume, 3) == this.cubic)
                                xresult = agencyObj.barrelThree;

                            if (cubic > barrelObj.volume && cubic <= 0.45m)
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightOne, barrelObj.volume), (decimal)cubic);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > 0.45m && cubic < decimal.Multiply(barrelObj.volume, 2))
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightTwo, decimal.Multiply(barrelObj.volume, 2)), (decimal)cubic);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > decimal.Multiply(barrelObj.volume, 2) && cubic < decimal.Multiply(barrelObj.volume, 3))
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightThree, decimal.Multiply(barrelObj.volume, 3)), (decimal)cubic);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > decimal.Multiply(barrelObj.volume, 3) && cubic <= 2m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightOne);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > 2m && cubic <= 5m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightTwo);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > 5m && cubic <= 9m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightThree);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic > 9m)
                            {
                                xresult = decimal.Multiply((decimal)cubic, (decimal)agencyObj.freightFour);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }

                            if (cubic < barrelObj.volume)
                            {
                                xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightOne, barrelObj.volume), (decimal)cubic);
                                xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                            }
                        }

                        break;

                }

                chargeKeys.value = (decimal)xresult;

                return chargeKeys;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<clsShippingItem> getVolumeForBarrelAsync()
        {
            const int TWO = 2;
            clsShippingItem obj = null;

            try
            {
                var q = (from tsp in config.TShippingItems
                         where tsp.Id == TWO
                         select new
                         {
                             id = tsp.Id,
                             item = tsp.ItemName,
                             price = tsp.ItemPrice,
                             weight = tsp.ItemWeight,
                             barrelVolume = tsp.ItemVolume
                         });

                var ql = await q.ToListAsync().ConfigureAwait(false);

                obj = ql.Select(x => new clsShippingItem()
                {
                    id = x.id,
                    name = x.item,
                    price = x.price,
                    weight = x.weight == null ? 80m: x.weight,
                    volume = (decimal)x.barrelVolume == null ? 0.3m : (decimal)x.barrelVolume
                }).FirstOrDefault();

                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //private async Task<clsFreight> getAgentFreightCostAsync(int agentId, int portId)
        //{
        //    //TODO: determines freight cost to charge
        //    clsFreight obj = null;

        //    try
        //    {
        //        var query = (from ta in config.TAgencyRates
        //                     where ta.AgentId == agentId && ta.PortId == portId
        //                     select new
        //                     {
        //                         id = ta.Id,
        //                         freight1 = ta.Frgt1,
        //                         freight2 = ta.Frgt2,
        //                         freight3 = ta.Frgt3,
        //                         freight4 = ta.Frgt4,
        //                         barrel1 = ta.B1,
        //                         barrel2 = ta.B2,
        //                         barrel3 = ta.B3
        //                     });

        //        var queryList = await query.ToListAsync().ConfigureAwait(false);

        //        return obj = queryList.Select(x => new clsFreight()
        //        {
        //            id = x.id,
        //            freightOne = x.freight1,
        //            freightTwo = x.freight2,
        //            freightThree = x.freight3,
        //            freightFour = x.freight4,
        //            barrelOne = x.barrel1,
        //            barrelTwo = x.barrel2,
        //            barrelThree = x.barrel3
        //        }).FirstOrDefault();
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
