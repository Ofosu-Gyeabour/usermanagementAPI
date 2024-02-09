#nullable disable
namespace UserManagementAPI.POCOs
{
    public class clsShippingPackageItem
    {
        public clsShippingPackageItem()
        {
            
        }

        #region properties

        public int? qty { get; set; }
        public string? description { get; set; } = string.Empty;
        public decimal? itemPrice { get; set; } = 0m;
        public string? nomCode { get; set; } = string.Empty;
        public int? addedByUsr { get; set; } = 10000;
        public DateTime? addedDate { get; set; } = DateTime.Now;

        #endregion

    }
}
