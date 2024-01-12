#nullable disable

using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class clsShippingOrderItem
    {
        swContext config;
        public clsShippingOrderItem()
        {
            config = new swContext();
        }

        #region Properties

        public int id { get; set; }
        public int? shippingorderId { get; set; }
        public clsShippingItem? item { get; set; }
        public int? quantity { get; set; }
        public string? itemDescription { get; set; }
        public decimal? itemWeight { get; set; }
        public decimal? itemVolume { get; set; }
        public decimal? unitPrice { get; set; }
        public string? marks { get; set; }
        public string? hscode { get; set; }
        public string? picturePath { get; set; }

        

        #endregion

        #region Methods

        public async Task<IEnumerable<clsShippingOrderItem>> getShippingOrderItemsAsync()
        {
            IEnumerable<clsShippingOrderItem> results;

            try
            {
                var Q = (from tsp in config.TShippingOrderItems
                         join ts in config.TShippingItems on tsp.ItemId equals ts.Id
                         select new
                         {
                             uniqueId = tsp.Id,
                             shippingorderId = tsp.ShippingorderId,
                             itemId = tsp.ItemId,
                             itemName = ts.ItemName,
                             qty = tsp.Qty,
                             description = tsp.ItemDescription,
                             weight = tsp.ItemWeight,
                             volume = tsp.ItemVolume,
                             unitprice = tsp.UnitPrice,
                             marks = tsp.Marks,
                             hscode = tsp.Hscode,
                             picPath = tsp.ItemPicPath
                         }).Take(1000);

                var QList = await Q.ToListAsync().ConfigureAwait(false);

                return results = QList.Select(x => new clsShippingOrderItem()
                {
                    id = x.uniqueId,
                    shippingorderId = x.shippingorderId,
                    item = new clsShippingItem()
                    {
                        id = (int) x.itemId,
                        name = x.itemName
                    },
                    quantity = (int)x.qty,
                    itemDescription =x.description,
                    itemWeight = x.weight,
                    itemVolume = x.volume,
                    unitPrice = x.unitprice,
                    marks = x.marks,
                    hscode = x.hscode,
                    picturePath = x.picPath
                }).ToList();
            }
            catch(Exception x)
            {
                throw x;
            }
        }

        #endregion

    }
}
