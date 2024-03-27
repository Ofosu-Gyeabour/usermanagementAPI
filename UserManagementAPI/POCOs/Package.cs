#nullable disable

using UserManagementAPI.Data;

namespace UserManagementAPI.POCOs
{
    public class Package
    {
        public int Id { get; set; }
        public int clientId { get; set; }
        public int isInvoiced { get; set; }
        public DateTime? invoiceDate { get; set; }
        public int? createdBy { get; set; }
        public string? deliveryNote { get; set; }
        public int? companyId { get; set; }
        public int? saletypeId { get; set; }
        public string? nameOfDriver { get; set; }
        public DateTime? deliveryDate { get; set; }
        public int deliveryTimeID { get; set; }
        public string? primaryContact { get; set; } = string.Empty;
        public string? secondaryContact { get; set; } = string.Empty; //whatsapp
        public string? address1 { get; set; } = string.Empty;
        public string? address2 { get; set; } = string.Empty;
        public string? address3 { get; set; } = string.Empty;
        public string? orderNumber { get; set; } = string.Empty;
        public int? statusId { get; set; }

        public List<PackageItem> packageItems { get; set; }
        public List<PackagePayment> packagePayments { get; set; }
        public List<PackageCharge> packageCharges { get; set; }
       
    }

    public class PackageItem
    {
        public int Id { get; set; }
        //public Package oPackage { get; set; }
        public int? packageOrderId { get; set; }
        public clsShippingItem? item { get; set; }
        public int? quantity { get; set; }
        public string? itemDescription { get; set; }
        public decimal? itemPrice { get; set; }
        public string? nomCode { get; set; }
    }

    public class orderItem
    {
        public int Id { get; set; }
        public int? qty { get; set; }
        public string? itemdescription { get; set; } = string.Empty;
        public string? itembcode { get; set; } = string.Empty;
        public int? itemid { get; set; } = 0;
        public string? itemName { get; set; } = string.Empty;
        public string? pluralName { get; set; } = string.Empty;
    }

    public class PackagePayment
    {
        public int Id { get; set; }
        public int packageOrderId { get; set; }
        public DateTime? paymentDate { get; set; } = DateTime.Now;
        public decimal? paymentAmt { get; set; } = 0m;
        public PaymentMethod? oPaymentMethod { get; set; }
        public string? paymentReceiptNo { get; set; } = string.Empty;
        public decimal? outstandingAmt { get; set; } = 0m;

        public async Task<int> getID()
        {
            int _Id = 0;
            try
            {
                swContext config = new swContext();
                using (config)
                {
                    var obj = await config.TPaymentMethods.Where(x => x.Method == oPaymentMethod.method).FirstOrDefaultAsync();

                    if (obj != null)
                    {
                        _Id = obj.Id;
                    }

                    return _Id;
                }
            }
            catch(Exception x)
            {
                return _Id;
            }
        }
    }

    public class PackageCharge
    {
        public int Id { get; set; }
        public int packageOrderId { get; set; }
        public ChargeLookup? oCharge { get; set; }
        public decimal? chargeAmt { get; set; } = 0m;
        public string? chargeDescription { get; set; } = string.Empty;
        public clsCurrency? oCurrency { get; set; }
    }
}
