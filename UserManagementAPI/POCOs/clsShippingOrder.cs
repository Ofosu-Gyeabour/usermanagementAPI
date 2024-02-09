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

        #endregion


        #endregion

        #region Methods



        #endregion

    }
}
