#nullable disable

namespace UserManagementAPI.POCOs
{
    public class clsShippingOrderCharge
    {

        public int Id { get; set; }
        public ChargeLookup? oCharge { get; set; }
        public decimal? chargeAmt { get; set; } = 0m;
        public string? chargeDescription { get; set; } = string.Empty;
        public clsCurrency? oCurrency { get; set; }

        

    }
}
