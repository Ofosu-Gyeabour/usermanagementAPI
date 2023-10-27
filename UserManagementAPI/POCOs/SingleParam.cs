#nullable disable
namespace UserManagementAPI.POCOs
{
    public class SingleParam
    {
        public string stringValue { get; set; } = string.Empty;
    }

    public class SearchParam : SingleParam
    {
        public string searchCriteria { get; set; }
    }
}
