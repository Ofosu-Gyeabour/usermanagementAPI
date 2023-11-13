#nullable disable

namespace UserManagementAPI.POCOs
{
    public class PostCodeAddress
    {
        public PostCodeAddress()
        {
            
        }

        public string Id { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;

    }

    public class PostCodeAddressItems
    {
        public List<PostCodeAddress> Items { get; set; }
    }


    public class PostCodeSearchTerm
    {
        public string searchTerm { get; set; } = string.Empty;

        public static ValueTask<PostCodeSearchTerm> BindAsync(HttpContext context)
        {
            var result = new PostCodeSearchTerm { 
                searchTerm = context.Request.Query[nameof(searchTerm)]
            };

            return ValueTask.FromResult(result);
        }
    }
}
