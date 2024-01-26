#nullable disable

using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class clsShipping
    {
        swContext config;
        public clsShipping()
        {
            config = new swContext();
        }

        #region Properties

        public int shippingOrderId { get; set; }
        public CompanyLookup? oCompany { get; set; }
        public int? isConsolidated { get; set; } 
        public string? consolidatedDescription { get; set; } = string.Empty;
        public int? isInvoiced { get; set; } 
        public DateTime? invoiceDate { get; set; } = DateTime.Now;
        public int? createdBy { get; set; }
        public int? customerId { get; set; }
        public int? consignorId { get; set; }
        public int? recipientId { get; set; }
        public int? notifyPartyId { get; set; }
        public int? sealQty { get; set; }
        public decimal? sealPrice { get; set; }
        public int? routingId { get; set; }
        public int? deliveryMethodId { get; set; }
        public int? payMethodId { get; set; }
        //public int? arrivalPortId { get; set; }
        public ShippingPortLookup? oArrivalPort { get; set; }

        public string? contactInstruction { get; set; } = string.Empty;
        public string? orderNote { get; set; } = string.Empty;
        public string? cargoDescription { get; set; } = string.Empty;
        public DateTime? orderCreationDate { get; set; } = DateTime.Now;
        public clsShippingOrderStatus? oShippingStatus { get; set; }
        public string? bolNumber { get; set; } = string.Empty;
        
        #endregion


    }

    public record consigneeParam
    {
        public int consignorId { get; set; }
        public int totalFetched { get; set; }
    }

    public class clsConsigee : clsShipping
    {
        swContext config;
        public clsConsigee() 
        {
            config = new swContext();
        }

        #region Properties

        public GenericCustomerLookup? oCustomer { get; set; }
        public ShippingPortLookup? oPort { get; set; }
        public DeliveryMethodLookup? oDeliveryMethod { get; set; }
        public ShippingMethodLookup? oShippingMethod { get; set; }

        #endregion

        #region Methods

        public async Task<IEnumerable<clsConsigee>> fetchTopN(consigneeParam param)
        {
            //todo: gets the top N consignees for the consignor
            List<clsConsigee> results = null;

            try
            {
                var query = (from s in config.TShippings
                             join c in config.TClients on s.ReceipientId equals c.Id
                             join ca in config.TClientAddresses on c.Id equals ca.ClientId
                             join cnt in config.TCountryLookups on c.ClientCountryId equals cnt.CountryId
                             join p in config.Tshippingports on s.ArrivalPortId equals p.Id
                             join pcnt in config.TCountryLookups on p.CountryId equals pcnt.CountryId
                             join dm in config.TDeliveryMethods on s.DelMethodId equals dm.Id
                             join sm in config.TShippingMethods on s.RoutingId equals sm.Id
                             where s.ConsignorId == param.consignorId
                             orderby s.Id descending

                             select new
                             {
                                 recordId = s.Id,
                                 uniqueId = c.Id,
                                 fullName = c.ClientTypeId == 1? $"{c.Firstname} {c.Middlenames} { c.Surname}" : c.ClientBusinessName,
                                 postCode = c.ClientPostCode == null? string.Empty: c.ClientPostCode,
                                 address = $"{ca.ClientAddr1} {ca.ClientAddr2} {ca.ClientAddr3}",
                                 mobileNo = c.MobileNo == null? string.Empty: c.MobileNo,
                                 countryId = c.ClientCountryId,
                                 countryName = cnt.CountryName,
                                 portId = s.ArrivalPortId,
                                 portName = p.NameOfport,
                                 portCountry = pcnt.CountryName,
                                 deliveryMethod = dm.Method,
                                 shippingMethod = sm.Method
                             }).Take(param.totalFetched);

                var queryList = await query.ToListAsync().ConfigureAwait(false);

                results = queryList.Select(x => new clsConsigee()
                {
                    oCustomer = new GenericCustomerLookup()
                    {
                        //id = x.recordId,
                        id = x.uniqueId,
                        nameOrcompany = x.fullName,
                        postCode = x.postCode,
                        address = x.address,
                        mobileNo = x.mobileNo,
                        oCountry = new CountryLookup()
                        {
                            id = (int) x.countryId,
                            nameOfcountry = x.countryName
                        }
                    },
                    oPort = new ShippingPortLookup()
                    {
                        id = (int) x.portId,
                        nameOfport = x.portName,
                        oCountry = new CountryLookup()
                        {
                            nameOfcountry = x.portCountry, //name of arrival country
                        }
                    },
                    oDeliveryMethod = new DeliveryMethodLookup()
                    {
                        method = x.deliveryMethod
                    },
                    oShippingMethod = new ShippingMethodLookup()
                    {
                        shippingMethod = x.shippingMethod
                    }
                }).ToList();

                return results;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }

}
