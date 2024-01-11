#nullable disable
namespace UserManagementAPI.POCOs
{
    public class clsCurrency
    {
        public int Id { get; set; }
        public string? currencyCode { get; set; } = string.Empty;
        public string? currencyDescription { get; set; } = string.Empty;

        private swContext config;

        public async Task<int> getID()
        {
            //Gets the ID of a given currency
            try
            {
                config = new swContext();
                var _obj = await config.TCurrencyLookups.Where(c => c.CurrencyCode == this.currencyCode).FirstOrDefaultAsync();
                return _obj != null ? _obj.Id : 0;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

    }
}
