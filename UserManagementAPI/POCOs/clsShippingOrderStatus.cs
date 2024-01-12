#nullable disable
using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class clsShippingOrderStatus
    {
        public int Id { get; set; }
        public string shippingStatus { get; set; }

        private swContext config;

        public clsShippingOrderStatus()
        {
            config = new swContext();
        }
        public async Task<int> getId()
        {
            try
            {
                var ob = await config.TShippingOrderStatuses.Where(s => s.StatusDescription == this.shippingStatus).FirstOrDefaultAsync();
                return ob != null ? ob.Id : 0;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

    }
}
