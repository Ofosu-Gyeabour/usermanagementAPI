#nullable disable
using Newtonsoft.Json.Schema;
using UserManagementAPI.Response;
using UserManagementAPI.utils;

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
        public int deliveryMethodId { get; set; }
        public int parishId { get; set; }
        public int zoneId { get; set; }
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

        public async Task<JTSData> determineJTSClearanceAndDelivery(clsFreightInput input)
        {
            //TODO: computes clearance and delivery for JTS
            clsCalculatorZone calcObj = null;
            clsShippingItem barrelObj = null;
            decimal? xresult = 0m;
            decimal? xduty = 0m;
            decimal? xd2d = 0m;

            JTSData jts = new JTSData();

            try
            {
                //var chargeKeys = await o.Get();
                barrelObj = await this.getVolumeForBarrelAsync();
                calcObj = await new clsCalculatorZone() { ParishId = input.parishId, ZoneId = input.zoneId }.getJTSZoneRecordAsync();
                var dutyObj = await new clsDuty() { }.getJTSDutyRecord();

                if (barrelObj.volume == input.cubic)
                    xresult = calcObj.freightBar1;

                if (decimal.Multiply(barrelObj.volume, 2) == input.cubic)
                    xresult = calcObj.freightBar2;

                if (decimal.Multiply(barrelObj.volume, 3) == input.cubic)
                    xresult = calcObj.freightBar3;

                if (decimal.Multiply(barrelObj.volume, 4) == input.cubic)
                    xresult = calcObj.freightBar4;

                //conditions
                if (input.deliveryMethodId == 8)
                {
                    if ((input.cubic > barrelObj.volume) && (input.cubic < decimal.Multiply(barrelObj.volume, 2)))
                    {
                        if ((input.cubic > decimal.Multiply(barrelObj.volume, 1)) && (input.cubic <= 0.4m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3));
                        }
                        else if ((input.cubic > 0.4m) && (input.cubic <= 0.5m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3), 2));
                        }
                        else if ((input.cubic > 0.5m) && (input.cubic < 0.6m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3), 2), 12));
                        }

                        xresult = xresult > (decimal)calcObj.freightBar1 ? xresult : (decimal)calcObj.freightBar1;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume, 2)) && (input.cubic < decimal.Multiply(barrelObj.volume, 3)))
                    {
                        //point by point
                        if ((input.cubic > decimal.Multiply(barrelObj.volume, 2)) && (input.cubic <= 0.7m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3));
                        }
                        else if ((input.cubic > 0.7m) && (input.cubic <= 0.8m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3), 2));
                        }
                        else if ((input.cubic > 0.8m) && (input.cubic < 0.9m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3), 2), 12));
                        }

                        xresult = xresult > calcObj.freightBar4 ? calcObj.freightBar4 : xresult;
                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    //0.9 to 1.2
                    if ((input.cubic > decimal.Multiply(barrelObj.volume, 3)) && (input.cubic < decimal.Multiply(barrelObj.volume, 4)))
                    {
                        //1.0
                        if ((input.cubic > decimal.Multiply(barrelObj.volume, 3)) && (input.cubic <= 1.0m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3));
                        }
                        else if ((input.cubic > 1.0m) && (input.cubic <= 1.1m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3), 2));
                        }
                        else if ((input.cubic > 1.1m) && (input.cubic <= 1.19m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3), 2), 12));
                        }

                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;

                    }

                    //over 1.2
                    if (input.cubic > decimal.Multiply(barrelObj.volume, 4))
                    {
                        decimal d1 = decimal.Subtract((decimal)input.cubic, decimal.Multiply(barrelObj.volume, 4));
                        decimal d2 = decimal.Divide((decimal)calcObj.freightBar5, barrelObj.volume);

                        xresult = decimal.Add(decimal.Multiply(d1, d2), (decimal)calcObj.freightBar4);

                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    //catch-all
                    xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    xresult = xresult < calcObj.freightBar1 ? calcObj.freightBar1 : xresult;
                }
                else if(input.deliveryMethodId == 9)  //door-to-door and duty paid
                {
                    //calcobj, barrelobj and tduty class

                    if (barrelObj.volume == input.cubic)
                        xresult = calcObj.freightBar1;

                    if (decimal.Multiply(barrelObj.volume, 2) == input.cubic)
                        xresult = calcObj.freightBar2;

                    if (decimal.Multiply(barrelObj.volume, 3) == input.cubic)
                        xresult = calcObj.freightBar3;

                    if (decimal.Multiply(barrelObj.volume, 4) == input.cubic)
                        xresult = calcObj.freightBar4;

                    if ((input.cubic > barrelObj.volume) && (input.cubic < decimal.Multiply(barrelObj.volume, 2)))
                    {
                        //0.4
                        //xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar2Duty, decimal.Multiply(barrelObj.volume, 2)), (decimal)input.cubic);

                        if ((input.cubic > barrelObj.volume) && (input.cubic <= 0.4m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3));
                        }
                        else if ((input.cubic > 0.4m) && (input.cubic <= 0.5m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3), 2));
                        }
                        else if ((input.cubic > 0.5m) && (input.cubic < 0.6m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3), 2), 12));
                        }

                        xresult = xresult > (decimal)calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume, 2)) && (input.cubic < decimal.Multiply(barrelObj.volume, 3)))
                    {
                        //xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar3Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);

                        if ((input.cubic > decimal.Multiply(barrelObj.volume, 2)) && (input.cubic <= 0.7m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3));
                        }
                        else if ((input.cubic > 0.7m) && (input.cubic <= 0.8m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3), 2));
                        }
                        else if ((input.cubic > 0.8m) && (input.cubic < 0.9m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3), 2), 12));
                        }

                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume, 3)) && (input.cubic < decimal.Multiply(barrelObj.volume, 4)))
                    {
                        //xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar4Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);

                        if ((input.cubic > decimal.Multiply(barrelObj.volume, 3)) && (input.cubic <= 1.0m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3));
                        }
                        else if ((input.cubic > 1.0m) && (input.cubic <= 1.1m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3), 2));
                        }
                        else if ((input.cubic > 1.1m) && (input.cubic < 1.19m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3), 2), 12));
                        }

                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    if (input.cubic > decimal.Multiply(barrelObj.volume, 4))
                    {
                        decimal d1 = decimal.Subtract((decimal)input.cubic, decimal.Multiply(barrelObj.volume, 4));
                        decimal d2 = decimal.Divide((decimal)calcObj.freightBar5, barrelObj.volume);

                        xresult = decimal.Add(decimal.Multiply(d1, d2), (decimal)calcObj.freightBar4);
                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    //xresult = xresult < calcObj.freightBar1 ? calcObj.freightBar1 : xresult;

                    //calculating duties below


                    if (input.cubic == barrelObj.volume)
                        xduty = dutyObj.frgtBar1Duty;

                    if (input.cubic == decimal.Multiply(barrelObj.volume, 2))
                        xduty = dutyObj.frgtBar2Duty;

                    if (input.cubic == decimal.Multiply(barrelObj.volume, 3))
                        xduty = dutyObj.frgtBar3Duty;

                    if (input.cubic == decimal.Multiply(barrelObj.volume, 4))
                        xduty = dutyObj.frgtBar4Duty;

                    if ((input.cubic > barrelObj.volume) && (input.cubic < decimal.Multiply(barrelObj.volume, 2)))
                    {
                        xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar2Duty, decimal.Multiply(barrelObj.volume, 2)), (decimal)input.cubic);
                        xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume, 2)) && (input.cubic < decimal.Multiply(barrelObj.volume, 3)))
                    {
                        xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar3Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);
                        xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume, 3)) && (input.cubic < decimal.Multiply(barrelObj.volume, 4)))
                    {
                        xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar4Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);
                        xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                    }

                    if (input.cubic > decimal.Multiply(barrelObj.volume, 4))
                    {
                        decimal dt1 = decimal.Subtract((decimal)input.cubic, decimal.Multiply(barrelObj.volume, 4));
                        decimal dt2 = decimal.Divide(dutyObj.frgtBar5Duty, barrelObj.volume);

                        xduty = decimal.Add(decimal.Multiply(dt1, dt2), dutyObj.frgtBar4Duty);
                        xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                    }
                }

                //combining d2d and duty
                jts.jtsduty = xduty;
                jts.jtscd = xresult;

                return jts;

                //chargeKeys.value = (decimal)(xresult + xduty);
                //return chargeKeys;
            }
            catch(Exception x)
            {
                throw x;
            }
        }

        public async Task<OrderSummaryDetails> determineClearanceAndDelivery(clsFreightInput input)
        {
            //TODO: computes clearance and delivery
            clsCalculatorZone calcObj = null;
            clsShippingItem barrelObj = null;
            decimal? xresult = 0m;
            decimal? xduty = 0m;
            decimal? xd2d = 0m;

            OrderSummaryDetails o = new OrderSummaryDetails()
            {
                id = 3,
                key = @"CnD"
            };

            try
            {
                var chargeKeys = await o.Get();
                barrelObj = await this.getVolumeForBarrelAsync();
                calcObj = await new clsCalculatorZone() { ParishId = input.parishId, ZoneId = input.zoneId }.getCalculatorZoneRecordAsync();
                var dutyObj = await new clsDuty() { }.getDutyRecordAsync();

                if (barrelObj.volume == input.cubic)
                    xresult = calcObj.freightBar1;

                if (decimal.Multiply(barrelObj.volume, 2) == input.cubic)
                    xresult = calcObj.freightBar2;

                if (decimal.Multiply(barrelObj.volume, 3) == input.cubic)
                    xresult = calcObj.freightBar3;

                if (decimal.Multiply(barrelObj.volume, 4) == input.cubic)
                    xresult = calcObj.freightBar4;

                //conditions
                if (input.deliveryMethodId == 8)
                {
                    if ((input.cubic > barrelObj.volume) && (input.cubic < decimal.Multiply(barrelObj.volume, 2)))
                    {
                        if ((input.cubic > decimal.Multiply(barrelObj.volume, 1)) && (input.cubic <= 0.4m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3));
                        }
                        else if ((input.cubic > 0.4m) && (input.cubic <= 0.5m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3), 2));
                        }
                        else if ((input.cubic > 0.5m) && (input.cubic < 0.6m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3), 2), 12));
                        }

                        xresult = xresult > (decimal)calcObj.freightBar1 ? xresult : (decimal)calcObj.freightBar1;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume, 2)) && (input.cubic < decimal.Multiply(barrelObj.volume, 3)))
                    {
                        //point by point
                        if ((input.cubic > decimal.Multiply(barrelObj.volume, 2)) && (input.cubic <= 0.7m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3));
                        }
                        else if ((input.cubic > 0.7m) && (input.cubic <= 0.8m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3), 2));
                        }
                        else if ((input.cubic > 0.8m) && (input.cubic < 0.9m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3), 2), 12));
                        }

                        xresult = xresult > calcObj.freightBar4 ? calcObj.freightBar4 : xresult;
                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    //0.9 to 1.2
                    if ((input.cubic > decimal.Multiply(barrelObj.volume, 3)) && (input.cubic < decimal.Multiply(barrelObj.volume, 4)))
                    {
                        //1.0
                        if ((input.cubic > decimal.Multiply(barrelObj.volume, 3)) && (input.cubic <= 1.0m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3));
                        }
                        else if ((input.cubic > 1.0m) && (input.cubic <= 1.1m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3), 2));
                        }
                        else if ((input.cubic > 1.1m) && (input.cubic <= 1.19m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3), 2), 12));
                        }

                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;

                    }

                    //over 1.2
                    if (input.cubic > decimal.Multiply(barrelObj.volume, 4))
                    {
                        decimal d1 = decimal.Subtract((decimal)input.cubic, decimal.Multiply(barrelObj.volume, 4));
                        decimal d2 = decimal.Divide((decimal)calcObj.freightBar5, barrelObj.volume);

                        xresult = decimal.Add(decimal.Multiply(d1, d2), (decimal)calcObj.freightBar4);

                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    //catch-all
                    xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    xresult = xresult < calcObj.freightBar1 ? calcObj.freightBar1 : xresult;
                }
                else if (input.deliveryMethodId == 9)  //door-to-door and duty paid
                {
                    //calcobj, barrelobj and tduty class

                    if (barrelObj.volume == input.cubic)
                        xresult = calcObj.freightBar1;

                    if (decimal.Multiply(barrelObj.volume, 2) == input.cubic)
                        xresult = calcObj.freightBar2;

                    if (decimal.Multiply(barrelObj.volume, 3) == input.cubic)
                        xresult = calcObj.freightBar3;

                    if (decimal.Multiply(barrelObj.volume, 4) == input.cubic)
                        xresult = calcObj.freightBar4;

                    if ((input.cubic > barrelObj.volume) && (input.cubic < decimal.Multiply(barrelObj.volume, 2))){
                        //0.4
                        //xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar2Duty, decimal.Multiply(barrelObj.volume, 2)), (decimal)input.cubic);

                        if((input.cubic > barrelObj.volume) && (input.cubic <= 0.4m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3));
                        }
                        else if ((input.cubic > 0.4m) && (input.cubic <= 0.5m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3), 2));
                        }
                        else if((input.cubic > 0.5m) && (input.cubic < 0.6m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar1 ,decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar2, (decimal)calcObj.freightBar1), 3), 2), 12));
                        }

                        xresult = xresult > (decimal)calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume,2)) && (input.cubic < decimal.Multiply(barrelObj.volume, 3)))
                    {
                        //xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar3Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);

                        if ((input.cubic > decimal.Multiply(barrelObj.volume,2)) && (input.cubic <= 0.7m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3));
                        }
                        else if((input.cubic > 0.7m) && (input.cubic <= 0.8m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3), 2));
                        }
                        else if ((input.cubic > 0.8m) && (input.cubic < 0.9m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar2, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar3, (decimal)calcObj.freightBar2), 3), 2), 12));
                        }

                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume,3)) && (input.cubic < decimal.Multiply(barrelObj.volume, 4)))
                    {
                        //xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar4Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);

                        if ((input.cubic > decimal.Multiply(barrelObj.volume,3)) && (input.cubic <= 1.0m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3));
                        }
                        else if ((input.cubic > 1.0m) && (input.cubic <= 1.1m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3), 2));
                        }
                        else if ((input.cubic > 1.1m) && (input.cubic < 1.19m))
                        {
                            xresult = decimal.Add((decimal)calcObj.freightBar3, decimal.Add(decimal.Multiply(decimal.Divide(decimal.Subtract((decimal)calcObj.freightBar4, (decimal)calcObj.freightBar3), 3), 2), 12));
                        }

                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    if (input.cubic > decimal.Multiply(barrelObj.volume, 4))
                    {
                        decimal d1 = decimal.Subtract((decimal)input.cubic, decimal.Multiply(barrelObj.volume, 4));
                        decimal d2 = decimal.Divide((decimal)calcObj.freightBar5, barrelObj.volume);

                        xresult = decimal.Add(decimal.Multiply(d1, d2), (decimal)calcObj.freightBar4);
                        xresult = xresult > calcObj.freightBar1 ? xresult : calcObj.freightBar1;
                    }

                    //do NOT calculate the duty here
                    /*
                    if (input.cubic == barrelObj.volume)
                        xduty = dutyObj.frgtBar1Duty;

                    if (input.cubic == decimal.Multiply(barrelObj.volume, 2))
                        xduty = dutyObj.frgtBar2Duty;

                    if (input.cubic == decimal.Multiply(barrelObj.volume, 3))
                        xduty = dutyObj.frgtBar3Duty;

                    if (input.cubic == decimal.Multiply(barrelObj.volume, 4))
                        xduty = dutyObj.frgtBar4Duty;

                    if((input.cubic > barrelObj.volume) && (input.cubic < decimal.Multiply(barrelObj.volume, 2)))
                    {
                        xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar2Duty, decimal.Multiply(barrelObj.volume, 2)), (decimal)input.cubic);
                        xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume,2)) && (input.cubic < decimal.Multiply(barrelObj.volume, 3)))
                    {
                        xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar3Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);
                        xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                    }

                    if ((input.cubic > decimal.Multiply(barrelObj.volume,3)) && (input.cubic < decimal.Multiply(barrelObj.volume, 4)))
                    {
                        xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar4Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);
                        xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                    }

                    if (input.cubic > decimal.Multiply(barrelObj.volume, 4))
                    {
                        decimal dt1 = decimal.Subtract((decimal)input.cubic, decimal.Multiply(barrelObj.volume, 4));
                        decimal dt2 = decimal.Divide(dutyObj.frgtBar5Duty, barrelObj.volume);

                        xduty = decimal.Add(decimal.Multiply(dt1, dt2), dutyObj.frgtBar4Duty);
                        xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                    }
                    */
                    //end of duty
                }

                //combining d2d and duty
                chargeKeys.value = (decimal)(xresult + xduty);
                return chargeKeys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OrderSummaryDetails> determineFreightRateUSD(decimal gbpFreight)
        {
            //TODO: gets the freight rate for USD (from GBP)
            FxAPIResponse fxData = new FxAPIResponse();
            OrderSummaryDetails o = new OrderSummaryDetails()
            {
                id = 3,
                key = @"FRT RATE USD"
            };

            try
            {
                Helper helper = new Helper();
                OrderSummaryDetails chargeKeys = await o.Get();

                //check if record exist in the Db before
                clsFx fx = new clsFx();
                var oFx = await fx.Get(DateTime.Now);

                if (oFx == null)
                {
                    fxData = await helper.getFxRatesAsync();
                    chargeKeys.value = fxData.quotes.USDGBP * gbpFreight;

                    //store fx data for future use
                    var fxObj = new clsFx() { usdgbp = fxData.quotes.USDGBP, usdeur = fxData.quotes.USDEUR, forexDate = DateTime.Now };
                    await fx.AddToDbAsync(fxObj);
                }
                else
                {
                    chargeKeys.value = oFx.usdgbp * gbpFreight;
                }

                return chargeKeys;
            }
            catch(Exception x)
            {
                throw x;
            }
        }

        public async Task<OrderSummaryDetails> determineWifDuty(clsFreightInput input)
        {
            //TODO: determines wif duties
            decimal? xduty = 0m;
            clsShippingItem barrelObj = null;

            OrderSummaryDetails o = new OrderSummaryDetails()
            {
                id = 3,
                key = @"DUTIES"
            };

            try
            {
                var chargeKeys = await o.Get();
                barrelObj = await this.getVolumeForBarrelAsync();
                var dutyObj = await new clsDuty() { }.getDutyRecordAsync();

                if (input.cubic == barrelObj.volume)
                    xduty = dutyObj.frgtBar1Duty;

                if (input.cubic == decimal.Multiply(barrelObj.volume, 2))
                    xduty = dutyObj.frgtBar2Duty;

                if (input.cubic == decimal.Multiply(barrelObj.volume, 3))
                    xduty = dutyObj.frgtBar3Duty;

                if (input.cubic == decimal.Multiply(barrelObj.volume, 4))
                    xduty = dutyObj.frgtBar4Duty;

                if ((input.cubic > barrelObj.volume) && (input.cubic < decimal.Multiply(barrelObj.volume, 2)))
                {
                    xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar2Duty, decimal.Multiply(barrelObj.volume, 2)), (decimal)input.cubic);
                    xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                }

                if ((input.cubic > decimal.Multiply(barrelObj.volume, 2)) && (input.cubic < decimal.Multiply(barrelObj.volume, 3)))
                {
                    xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar3Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);
                    xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                }

                if ((input.cubic > decimal.Multiply(barrelObj.volume, 3)) && (input.cubic < decimal.Multiply(barrelObj.volume, 4)))
                {
                    xduty = decimal.Multiply(decimal.Divide(dutyObj.frgtBar4Duty, decimal.Multiply(barrelObj.volume, 3)), (decimal)input.cubic);
                    xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                }

                if (input.cubic > decimal.Multiply(barrelObj.volume, 4))
                {
                    decimal dt1 = decimal.Subtract((decimal)input.cubic, decimal.Multiply(barrelObj.volume, 4));
                    decimal dt2 = decimal.Divide(dutyObj.frgtBar5Duty, barrelObj.volume);

                    xduty = decimal.Add(decimal.Multiply(dt1, dt2), dutyObj.frgtBar4Duty);
                    xduty = xduty > dutyObj.frgtBar1Duty ? xduty : dutyObj.frgtBar1Duty;
                }

                chargeKeys.value = (decimal) xduty;
                return chargeKeys;
            }
            catch(Exception dutyErr)
            {
                throw dutyErr;
            }
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

                        if (xresult < (decimal)agencyObj.barrelOne)
                        {
                            xresult = (decimal)agencyObj.barrelOne;
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

                            if (xresult < (decimal)agencyObj.freightOne)
                            {
                                xresult = (decimal)agencyObj.freightOne;
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

                            if (xresult < agencyObj.minimum)
                            {
                                if (cubic < barrelObj.volume)
                                {
                                    xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightOne, barrelObj.volume), (decimal)cubic);
                                    xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                                }
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

                            if (xresult < (decimal)agencyObj.freightOne)
                            {
                                xresult = (decimal)agencyObj.freightOne;
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
                                                       
                            if (xresult < agencyObj.minimum)
                            {
                                if (cubic < barrelObj.volume)
                                {
                                    xresult = decimal.Multiply(decimal.Divide((decimal)agencyObj.freightOne, barrelObj.volume), (decimal)cubic);
                                    xresult = xresult > agencyObj.minimum ? xresult : agencyObj.minimum;
                                }
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
