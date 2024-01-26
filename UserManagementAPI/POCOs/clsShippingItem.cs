#nullable disable

using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class clsShippingItem
    {
        swContext config;
        public clsShippingItem()
        {
            config = new swContext();
        }

        #region Properties

        public int id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public decimal? price { get; set; }
        public decimal? weight { get; set; }
        public decimal volume { get; set; }
        public string? pluralName { get; set; }
        public CountryLookup? oCountry { get; set; }

        #endregion

        #region Methods

        public async Task<IEnumerable<clsShippingItem>> getShippingItemsAsync()
        {
            //TODO: gets shipping items
            IEnumerable<clsShippingItem> shippingItems = null;

            try
            {
                var query = (from ts in config.TShippingItems 
                             join cnt in config.TCountryLookups on ts.CountryId equals cnt.CountryId

                             select new
                             {
                                 uniqueId = ts.Id,
                                 itemName = ts.ItemName,
                                 itemDescription = ts.ItemDescription,
                                 countryId = ts.CountryId,
                                 countryName = cnt.CountryName,
                                 itemPrice = ts.ItemPrice,
                                 itemWeight = ts.ItemWeight,
                                 itemVolume = ts.ItemVolume,
                                 pluralName = ts.PluralName
                             });

                var queryList = await query.ToListAsync().ConfigureAwait(false);

                shippingItems = queryList.Select(x => new clsShippingItem()
                {
                    id = x.uniqueId,
                    name = x.itemName,
                    description = x.itemDescription,
                    price = x.itemPrice,
                    pluralName = x.pluralName,
                    oCountry = new CountryLookup()
                    {
                        id = (int) x.countryId,
                        nameOfcountry = x.countryName
                    }
                }).ToList();

                return shippingItems;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> getPackageItemID()
        {
            //get the id of the packaging item
            try
            {
                var obj = await config.TPackagingItems.Where(x => x.PackagingItem == this.name).FirstOrDefaultAsync();
                return obj != null ? obj.Id : 0;
            }
            catch (Exception x)
            {
                return 0;
            }
        }

        public async Task<int> getShippingItemID()
        {
            try
            {
                var obj = await config.TShippingItems.Where(x => x.ItemName == this.name).FirstOrDefaultAsync();
                return obj != null ? obj.Id : 0;
            }
            catch(Exception x)
            {
                return 0;
            }
        }

        #endregion

    }
}
