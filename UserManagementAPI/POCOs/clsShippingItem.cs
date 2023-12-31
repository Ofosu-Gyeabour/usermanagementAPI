﻿#nullable disable

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

        #endregion

    }
}
