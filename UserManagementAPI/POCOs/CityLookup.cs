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

}
