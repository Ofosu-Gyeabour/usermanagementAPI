﻿#nullable disable

using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel;
using System.Security.Policy;

namespace UserManagementAPI.POCOs
{
    public class CityLookup
    {
        public int id { get; set; } = 0;
        public string nameOfcity { get; set; } = string.Empty;
        public CountryLookup oCountry { get; set; }
    }
    public record CityRecord
    {
        public int id { get; set; } = 0;
        public string nameOfcity { get; set; } = string.Empty;
        public int countryId { get; set; } = 0;
        public string nameOfcountry { get; set; } = string.Empty;
    }

    public record DateRangeRecord
    {
        public DateTime df { get; set; }
        public DateTime dt { get; set; }
    }

    public class CountryLookup
    {
        public int id { get; set; } = 0;
        public RegionLookup oRegion { get; set; }
        public string nameOfcountry { get; set; } = string.Empty;
        public string codeOfcountry { get; set; } = string.Empty;

        #region methods

        public async Task<CountryLookup> Get(int id)
        {
            //TODO: gets a country lookup using the record's ID
            CountryLookup obj = null;

            try
            {
                var config = new swContext();
                using (config)
                {
                    var o = await config.TCountryLookups.Where(c => c.CountryId == id).FirstOrDefaultAsync();
                    obj = o != null ? new CountryLookup() { id = o.CountryId, nameOfcountry = o.CountryName } : null;
                }

                return obj;
            }
            catch(Exception ex)
            {
                return obj;
            }
        }

        public async Task<CountryLookup> Get(string country)
        {
            //TODO: gets a country record using the record's name
            CountryLookup obj = null;

            try
            {
                var config = new swContext();
                using (config)
                {
                    var o = await config.TCountryLookups.Where(c => c.CountryName == country).FirstOrDefaultAsync();
                    obj = o != null ? new CountryLookup() { id = o.CountryId, nameOfcountry = o.CountryName } : null;
                }

                return obj;
            }
            catch(Exception ex)
            {
                return obj;
            }
        }

        #endregion

    }

    public class RegionLookup
    {
        public int id { get; set; } = 0;
        public string nameOfregion { get; set; } = string.Empty;
    }

    public class DepartmentLookup
    {
        public int id { get; set; } = 0;
        public string nameOfdepartment { get; set; } = string.Empty;
        public string departmentDescription { get; set; } = string.Empty;
        public CompanyLookup oCompany { get; set; }
    }

    public class BranchLookup
    {
        public int id { get; set; } = 0;
        public string nameOfbranch { get; set; } = string.Empty;
        public CompanyLookup oCompany { get; set; }
    }

    public class CompanyLookup
    {
        public int id { get; set; } = 0;
        public string nameOfcompany { get; set; } = string.Empty;
        public string addressOfcompany { get; set; } = string.Empty;
        public CityLookup oCity { get; set; }
        public CountryLookup oCountry { get; set; }
        public string companyLogo { get; set; } = string.Empty;
        public DateTime dateOfIncorporation { get; set; } = DateTime.Now;
    }

    public class ReferralLookup
    {
        public int id { get; set; }
        public string sourceOfReferral { get; set; } = string.Empty;
    }

    public class CompanyTypeLookup
    {
        public int id { get; set; }
        public string typeOfCompany { get; set; }
    }

    public class TitleLookup
    {
        public int id { get; set; } = 0;
        public string nameOftitle { get; set; } = string.Empty;
    }

    public class ShippingPortLookup
    {
        public int id { get; set; } = 0;
        public string nameOfport { get; set; } = string.Empty;
        public string codeOfport { get; set; } = string.Empty;
        public int sailingTimeInDays { get; set; } = 0;
        public CountryLookup oCountry { get; set; }

    }

    public record ShippingPortRecord
    {
        public int id { get; set; } = 0;
        public string nameOfport { get; set; } = string.Empty;
        public string codeOfport { get; set; } = string.Empty;
        public string nameOfcountry { get; set; } = string.Empty;
        public int countryId { get; set; } = 0;
        public int sailingTimeInDays { get; set; } = 0;
    }

    public class AirportLookup
    {
        public int id { get; set; } = 0;
        public string nameOfairport { get; set; } = string.Empty;
        public string airportMnemonic { get; set; } = string.Empty;
        public CountryLookup oCountry { get; set; }
    }

    public class DialCodeLookup
    {
        public int id { get; set; } = 0;
        public string dialCode { get; set; } = string.Empty;
        public CountryLookup oCountry { get; set; }
    }

    public record DialCodeRecord
    {
        public int id { get; set; }
        public string dialCode { get; set; }
        public string nameOfcountry { get; set; } 
        public int countryId { get; set; }
    }

    public class ContainerTypeLookup
    {
        public int id { get; set; } = 0;
        public string containerType { get; set; } = string.Empty;
        public decimal containerVolume { get; set; } = 0m;
    }

    public class AdhocTypeLookup
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string nomCode { get; set; } = string.Empty;
    }


    public class PackageItemLookup
    {
        public int id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }

    public class PackagepriceLookup
    {
        public int id { get; set; } = 0;
        public decimal unitPrice { get; set; } = 0m;
        public decimal wholesalePrice { get; set; } = 0m;
        public decimal retailPrice { get; set; } = 0m;
        public PackageItemLookup oPackageItem { get; set; }
        public CompanyLookup oCompany { get; set; }

        //added nomcode
        public string? nomCode { get; set; } = string.Empty;
    }

    public class PackageStockLookup
    {
        public int id { get; set; }
        public PackageItemLookup oPackageItem { get; set; }
        public int inStock { get; set; } = 0;
        public int floor { get; set; } = 0;
        public int ceiling { get; set; } = 0;
        public CompanyLookup oCompany { get; set; }
        public int idOfpackage { get; set; }
        public int idOfcompany { get; set; }
        public string nameOfpackage { get; set; }
        public string nameOfcompany { get; set; }
    }
    public class SealTypeLookup
    {
        public int id { get; set; } = 0;
        public string sealDescription { get; set; } = string.Empty;
    }
    public class SealPriceLookup
    {
        public int id { get; set; } = 0;
        public decimal Price { get; set; } = 0m;
        public decimal sellingPrice { get; set; } = 0m;
        public SealTypeLookup oSealType { get; set; }
        public CompanyLookup oCompany { get; set; }
    }

    public class ShippingLineLookup
    {
        public int id { get; set; } = 0;
        public string shippingLine { get; set; } = string.Empty;
    }

    public class StockControlLookup
    {
        public int id { get; set; } = 0;
        public int currentStock { get; set; } = 0;
        public int minimumStockAllowed { get; set; } = 0; //floorThreshold
        public int maximumStockAllowed { get; set; } = 0; //ceilingThreshold
        public PackageItemLookup oPackageItem { get; set; }
        public CompanyLookup oCompany { get; set; }
    }

    public class VesselLookup
    {
        public int id { get; set; } = 0;
        public string nameOfvessel { get; set; } = string.Empty;
        public string flagOfvessel { get; set; } = string.Empty;
        public ShippingLineLookup oShippingLine { get; set; }
        public int shippingLineId { get; set; }
        public string shippingLine { get; set; }
    }

    public class ShippingMethodLookup
    {
        public int id { get; set; } = 0;
        public string shippingMethod { get; set; } = string.Empty;
        public string shippingRoute { get; set; } = string.Empty;
    }

    public class ShipperCategoryLookup
    {
        public int id { get; set; } = 0;
        public string description { get; set; } = string.Empty;
    }

    public class DeliveryMethodLookup
    {
        public int id { get; set; } = 0;
        public string method { get; set; } = string.Empty;
        public string methodDescription { get; set; } = string.Empty;
    }

    public class DeliveryZoneLookup
    {
        public int id { get; set; } = 0;
        public string zoneName { get; set; } = string.Empty;
        //public DeliveryMethodLookup oDeliveryMethod { get; set; }
        public string zoneDescription { get; set; } = string.Empty;
        public CountryLookup oCountry { get; set; }
    }

    public class HSCodeLookup
    {
        public int id { get; set; } = 0;
        public string code { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }

    //public class InsuranceTypeLookup
    //{
    //    public int id { get; set; } = 0;
    //    public string insuranceType { get; set; } = string.Empty;
    //}

    public class InsuranceLookup
    {
        public int id { get; set; } = 0;
        public string insuranceType { get; set; } = string.Empty;
        public string insuranceDescription { get; set; } = string.Empty;
        public decimal unitPrice { get; set; } = 0m;
        //public InsuranceTypeLookup oInsuranceType { get; set; }
    }

    public class SailingScheduleLookup
    {
        public int id { get; set; } = 0;
        public VesselLookup oVessel { get; set; }
        public ShippingPortLookup oDeparturePort { get; set; }
        public ShippingPortLookup oArrivalPort { get; set; }
        public DateTime closingDate { get; set; }
        public DateTime dateOfdeparture { get; set; }
        public DateTime dateOfarrival { get; set; }

        public string nameOfvessel { get; set; }
        public string nameOfDeparturePort { get; set; }
        public string nameOfArrivalPort { get; set; }
        public string shippingLine { get; set; }
        public int shippingLineId { get; set; }
        public int countryId { get; set; }
        public string nameOfcountry { get; set; }
    }

    public class PaymentTermLookup
    {
        public int id { get; set; } = 0;
        public string paymentTermDescrib { get; set; } = string.Empty;
    }

    public class OrderSummaryDetails
    {
        public int id { get; set; }
        public string key { get; set; } = string.Empty;
        public decimal value { get; set; } = 0m;

        private swContext config;

        public async Task<OrderSummaryDetails> Get()
        {

            //gets a specified order charge with Id and Key
            OrderSummaryDetails details = null;

            try
            {
                using (config = new swContext())
                {
                    var query = (from ce in config.TChargeEngines
                                 join ot in config.TOrderTypes on ce.OrdertypeId equals ot.Id
                                 join ch in config.TChargeLookups on ce.ChargeId equals ch.Id
                                 where ce.OrdertypeId == this.id && ch.Charge == this.key

                                 select new
                                 {
                                     id = ce.Id,
                                     chargeName = ch.Charge,
                                     rate = ce.ChargeRate
                                 });

                    var qList = await query.ToListAsync().ConfigureAwait(false);
                    details = qList.Select(x => new OrderSummaryDetails()
                    {
                        id = x.id,
                        key = x.chargeName,
                        value = (decimal)x.rate
                    }).FirstOrDefault();

                    return details;
                }
            }
            catch(Exception x)
            {
                return details;
            }
        }
    }

    public class OrderStat
    {
        public List<OrderSummaryDetails> summary { get; set; }
        public decimal? totAmt { get; set; } = 0m;
        public int? itemCount { get; set; } = 0;
        public decimal? deliveryCharge { get; set; } = 0m;
        public decimal? congestionCharge { get; set; } = 0m;
        public decimal? VAT { get; set; } = 0m;
        public OrderTypeLookup? oOrderType { get; set; }
    }

    public class OrderSummaryParameter
    {
        public decimal total { get; set; } = 0m;
        public decimal vatRate { get; set; } = 0m;
        public decimal paid { get; set; } = 0m;
    }

    public class OrderTypeLookup
    {
        public int id { get; set; } = 0;
        public string? orderDescription { get; set; } = string.Empty;
    }
    
    public class ChargeEngineLookup
    {
        public int id { get; set; } = 0;
        public OrderTypeLookup? oOrderType { get; set; }
        
        public ChargeLookup? oChargeLookup { get; set; } = null;
        public decimal? chargeRate { get; set; } = 0m;
        //public decimal? thresholdValue { get; set; } = 0m;
        public decimal? thresholdAmt { get; set; } = 0m;
        public decimal? thresholdRate { get; set; } = 0m;
        public int? isLabel { get; set; } = 0;

    }

    public class ChargeLookup
    {
        public int id { get; set; }
        public string? nameOfcharge { get; set; } = string.Empty;
        
    }

    public class PaymentMethod
    {
        public int id { get; set; } = 0;
        public string? method { get; set; } = string.Empty;
        public bool? isAccount { get; set; } = false;
    }

    public class GenericLookup
    {
        public int id { get; set; } = 0;
        public string? idValue { get; set; } = string.Empty;
    }

}
