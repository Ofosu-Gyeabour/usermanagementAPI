namespace UserManagementAPI.utils
{
    public class PostCodeAnywhereAPI
    {
        public string userName { get; set; } = string.Empty;
        public string apiKey { get; set; } = string.Empty;
        public string findEndPoint { get; set; } = string.Empty;
        public string retrieveEndPoint { get; set; } = string.Empty;
        public string contentType { get; set; } = string.Empty;
    }

    public static class PostCodeConfigObject
    {
        public static string USER { get; set; }
        public static string KEY { get; set; }
        public static string FIND_POST_CODE { get; set; }
        public static string RETRIEVE_ADDRESS { get; set; }
        public static string CONTENT_TYPE { get; set; }
    }

}
