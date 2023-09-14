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

}
