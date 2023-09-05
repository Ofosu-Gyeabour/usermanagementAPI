namespace UserManagementAPI.POCOs
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public string ItemDescription { get; set; }
        public double ItemAmount { get; set; }
    }

    public class InvoiceItemAddingOperation
    {
        //creating a dictionary of invoice item objects. It could also be a LIST

        Dictionary<int, InvoiceItem> dict = new Dictionary<int, InvoiceItem>();
        public static Dictionary<int, InvoiceItem> static_dict;

        public bool AddInvoiceItem(InvoiceItem item)
        {
            //method adds an invoice item to the dictionary object
            int counter = dict.Count();
            dict.Add(dict.Count(), item);

            //store the current state of dict into the static dict object so as to retain data on postBack
            static_dict = dict;

            return true;
        }
    }

    public class implementationClass
    {
        //fictional object
        InvoiceItem obj;
        InvoiceItemAddingOperation op;
        public implementationClass() {
            //instantiation
            obj = new InvoiceItem()
            {
                Id = 1,
                ItemDescription = @"Container fees",
                ItemAmount = 3400
            };
        }

        public bool Add()
        {
            //method adds invoice item
            op = new InvoiceItemAddingOperation();
            var result = op.AddInvoiceItem(obj);
            return result;
        }
        

        
    }
}
