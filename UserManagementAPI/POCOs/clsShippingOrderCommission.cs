#nullable disable

namespace UserManagementAPI.POCOs
{
    public class clsShippingOrderCommission
    {
        public clsShippingOrderCommission()
        {
            
        }

        #region properties

        public int Id { get; set; }
        public int shippingOrderId { get; set; } 
        public decimal? wifduty { get; set; } = 0m;
        public decimal? jtsduty { get; set; } = 0m;
        public decimal? dutyEarnings { get; set; } = 0m; //earnings on the duties for wif
        public decimal? wifcd { get; set; } = 0m;  //clearance and delivery for wif
        public decimal? jtscd { get; set; } = 0m;  //clearance and delivery for jts
        public decimal? earningsOnCnD { get; set; } = 0m;
        public decimal? cubicPerMeter { get; set; } = 0m;

        #endregion

    }
}
