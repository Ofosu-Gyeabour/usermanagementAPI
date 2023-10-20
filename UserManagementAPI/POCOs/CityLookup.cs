#nullable disable

namespace UserManagementAPI.POCOs
{
    public class CityLookup
    {
        public int id { get; set; } = 0;
        public string nameOfcity { get; set; } = string.Empty;
        public CountryLookup oCountry { get; set; }
    }

    public class CountryLookup
    {
        public int id { get; set; } = 0;
        public RegionLookup oRegion { get; set; }
        public string nameOfcountry { get; set; } = string.Empty;
        public string codeOfcountry { get; set; } = string.Empty;
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
        public DateTime dateOfIncorporation { get; set; }
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
    }

    public class CorporateCustomerLookup
    {
        public int id { get; set; }
        public string accountNo { get; set; } = string.Empty;
        public CityLookup oCity { get; set; }
        public CountryLookup oCountry { get; set; }
        public string postCode { get; set; } = string.Empty;
        public string mobileNo { get; set; } = string.Empty;
        public string whatsappNo { get; set; } = string.Empty;
        public string homeTelephone { get; set; } = string.Empty;
        public string workTelephone { get; set; } = string.Empty;
        public string clientEmail { get; set; } = string.Empty;
    }

    public class IndividualCustomerLookup
    {
        public int id { get; set; } = 0;
        public string accountNo { get; set; } = string.Empty;
        public string surname { get; set; } = string.Empty;
        public string firstname { get; set; } = string.Empty;
        public string middlenames { get; set; } = string.Empty;
        public CityLookup oCity { get; set; }
        public CountryLookup oCountry { get; set; }
        public string postCode { get; set; } = string.Empty;
        public string mobileNo { get; set; } = string.Empty;
        public string whatsappNo { get; set; } = string.Empty;
        public string homeTelephone { get; set; } = string.Empty;
        public string workTelephone { get; set; } = string.Empty;
        public string clientEmail { get; set; } = string.Empty;
    }

}
