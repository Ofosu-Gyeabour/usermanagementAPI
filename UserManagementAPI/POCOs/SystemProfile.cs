#nullable disable

namespace UserManagementAPI.POCOs
{
    public class SystemProfile
    {
        public int Id { get; set; } = 0;
        public string profileModules { get; set; } = string.Empty;
        public string nameOfProfile { get; set; } = string.Empty;
        public int companyId { get; set; } = 1;
        public int inUse { get; set; } = 1;
        public DateTime dateAdded { get; set; }
    }

    public class UserProfile
    {
        public string username { get; set; } = string.Empty;
        public SystemProfile profile { get; set; }
    }

    public class departmentLookup
    {
        public int id { get; set; }
        public string name { get; set; }
        public string describ { get; set; }
        public string companyName { get; set; }
    }

    public class userRecord
    {
        public int id { get; set; } = 0;
        public string sname { get; set; }
        public string fname { get; set; }
        public string othernames { get; set; }
        public UserInfo userCredentials { get; set; }
        public int companyId { get; set; }
        public int departmentid { get; set; }
        public int isAdministrator { get; set; }
        public int isLogged { get; set; }
        public int isActive { get; set; }
        public int profileid { get; set; }
    }

}
