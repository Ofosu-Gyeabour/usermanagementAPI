namespace UserManagementAPI.Response
{
    public class DefaultAPIResponse
    {
        public bool status { get; set; } = false;
        public string message { get; set; } = string.Empty;
        public object data { get; set; }
    }
}
