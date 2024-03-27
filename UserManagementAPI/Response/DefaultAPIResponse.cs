namespace UserManagementAPI.Response
{
    public class DefaultAPIResponse
    {
        public bool status { get; set; } = false;
        public string message { get; set; } = string.Empty;
        public object data { get; set; }
    }

    public class DownloadAPIResponse
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public byte[]? data { get; set; }
    }


    public class PaginationAPIResponse : DefaultAPIResponse
    {
        public int total { get; set; }
    }

    public class UploadAPIResponse
    {
        public bool status { get; set; } = false;
        public string message { get; set; } = string.Empty;
        public object data { get; set; } = null;
        public int successCount { get; set; } = 0;
        
        public int errorCount { get; set; } = 0;
        public List<string> errorMessageList { get; set; }

        public IEnumerable<object> errorList { get; set; }
    }

    public class UserAPIResponse
    {
        public bool status { get; set; }
        public string message { get; set; }

        public User user { get; set; }
        public Company company { get; set; }
        public Profile profile { get; set; }
        public Department department { get; set; }
    }

    public class FxAPIResponse
    {
        public bool success { get; set; } = false;
        public string terms { get; set; } = string.Empty;
        public string privacy { get; set; } = string.Empty;
        public long timestamp { get; set; } = 0;
        public string source { get; set; } = string.Empty;
        public Quotation quotes { get; set; }
        public FxError? error { get; set; }
    }

    
    public class FxError
    {
        public string? code { get; set; } = string.Empty;
        public string? info { get; set; } = string.Empty;

    }
    public class Quotation { 
    
        //public decimal USDAED { get; set; }
        //public decimal USDAFN { get; set; }
        //public decimal USDALL { get; set; }
        //public decimal USDAMD { get; set; }
        //public decimal USDANG { get; set; }
        //public decimal USDAOA { get; set; }
        //public decimal USDARS { get; set; }
        //public decimal USDAUD { get; set; }
        //public decimal USDAWG { get; set; }
        //public decimal USDAZN { get; set; }
        //public decimal USDBAM { get; set; }
        //public decimal USDBBD { get; set; }
        //public decimal USDBDT { get; set; }
        //public decimal USDBGN { get; set; }
        //public decimal USDBHD { get; set; }
        //public decimal USDBIF { get; set; }
        //public decimal USDBMD { get; set; }
        //public decimal USDBND { get; set; }
        //public decimal USDBOB { get; set; }
        //public decimal USDBRL { get; set; }
        //public decimal USDBSD { get; set; }
        //public decimal USDBTC { get; set; }
        //public decimal USDBTN { get; set; }
        //public decimal USDBWP { get; set; }
        //public decimal USDBYN { get; set; }
        //public decimal USDBYR { get; set; }
        //public decimal USDBZD { get; set; }
        //public decimal USDCAD { get; set; }
        //public decimal USDCDF { get; set; }
        //public decimal USDCHF { get; set; }
        //public decimal USDCLF { get; set; }
        //public decimal USDCLP { get; set; }
        //public decimal USDCNY { get; set; }
        //public decimal USDCOP { get; set; }
        public decimal USDGBP { get; set; }
        public decimal USDEUR { get; set; }
    }


    public class User
    {
        public int id { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string othernames { get; set; }
        public string usrname { get; set; }
        public string usrpassword { get; set; }
        public int? isAdmin { get; set; }
        public int? isLogged { get; set; }
        public int? isActive { get; set; }
        public int? lockAttempt { get; set; }
        public int? invalidLogAttempt { get; set; }

    }

    public class Company
    {
        public int id { get; set; }
        public string company { get; set; }
        public string companyAddress { get; set; }
        public DateTime? incorporationDate { get; set; }

    }

    public class Profile
    {
        public int id { get; set; }
        public string? profileString { get; set; }
        public int? inUse { get; set; }
        public DateTime? dateAdded { get; set; }
        public List<Company> associatedCompanies { get; set; }
    }

    public class Department
    {
        public int id { get; set; }
        public string departmentName { get; set; }
        public string departmentDescription { get; set; }
    }

    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }

    public class PagedResponseAPI : DefaultAPIResponse
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PagedResponseAPI(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

    }

}
