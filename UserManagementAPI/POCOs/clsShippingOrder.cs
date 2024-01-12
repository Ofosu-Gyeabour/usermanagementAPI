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

        #endregion

        #region Methods



        #endregion

    }
}
