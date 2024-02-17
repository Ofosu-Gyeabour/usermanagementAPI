#nullable disable

using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class clsShippingOrder
    {
        
        #region Properties

        public clsShipping? oShipping { get; set; }  //main shipping order
        public  List<clsShippingOrderItem> oShippingOrderItems { get; set; } //items being shipped
        public clsShippingOrderCharge[] oShippingOrderCharges { get; set; }

        public clsConsigneeItem oConsigneeItem { get; set; }
        public List<clsShippingOrderPayment> oShippingOrderPayments { get; set; }
        public List<clsShippingPackageItem> oShippingPackageItems { get; set; }

        #region properties added

        public int? transportTypeId { get; set; } = 1;
        public int? driveruserId { get; set; } = 0;
        public string? driverName { get; set; } = string.Empty;
        public DateTime? driverdeliverydte { get; set; } = DateTime.Now;
        public int? driverdeliveryTime { get; set; } = 1;
        public string? driverNote { get; set; } = string.Empty;

        public string? agencycompany { get; set; } = string.Empty;
        public DateTime? agencydeliveryDate { get; set; } = DateTime.Now;
        public int? agencytime { get; set; } = 1;

        public string? agencydeliveryNote { get; set; } = string.Empty;

        public int? dropoffreceivedBy { get; set; } = 10000;
        public DateTime? dropoffreceiveddte { get; set; } = DateTime.Now;

        public string warehouseNote { get; set; } = string.Empty;

        #region added for jts earnings and duties computation

        public decimal? wifcd { get; set; } = 0m;
        public decimal? jtsearning { get; set; } = 0m;
        public decimal? wifduty { get; set; } = 0m;
        public decimal? jtsduty { get; set; } = 0m;
        public decimal? cubic { get; set; } = 0;
        public int destinationCountry { get; set; } = 8;  //defaulted to JAMAICA

        #endregion

        #endregion


        #endregion

        #region Methods



        #endregion

    }
}
