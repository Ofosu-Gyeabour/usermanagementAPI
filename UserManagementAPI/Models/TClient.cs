using System;
using System.Collections.Generic;

namespace UserManagementAPI.Models
{
    public partial class TClient
    {
        public TClient()
        {
            TClientAddresses = new HashSet<TClientAddress>();
        }

        public int Id { get; set; }
        public int? ClientTypeId { get; set; }
        public int? AssociatedCompanyId { get; set; }
        public int? ChannelTypeId { get; set; }
        public string? Firstname { get; set; }
        public string? Middlenames { get; set; }
        public string? Surname { get; set; }
        public string? ClientBusinessName { get; set; }
        public int? DialcodeId { get; set; }
        public string? MobileNo { get; set; }
        public string? WhatsappNo { get; set; }
        public string? HomeTelephone { get; set; }
        public string? WorkTelephone { get; set; }
        public string? ClientEmailAddr { get; set; }
        public string? ClientEmailAddr2 { get; set; }
        public int? ClientCityId { get; set; }
        public int? ClientCountryId { get; set; }
        public string? ClientPostCode { get; set; }
        public int? ReferralId { get; set; }
        public string? CollectionInstruction { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool? IsShipper { get; set; }
        public string? ClientAccNo { get; set; }
        public string? ClientPassword { get; set; }
        public bool? CanLogin { get; set; }

        public virtual Tcompany? AssociatedCompany { get; set; }
        public virtual TChannelType? ChannelType { get; set; }
        public virtual TCity? ClientCity { get; set; }
        public virtual TCountryLookup? ClientCountry { get; set; }
        public virtual TClientType? ClientType { get; set; }
        public virtual Tclientreferralsource? Referral { get; set; }
        public virtual ICollection<TClientAddress> TClientAddresses { get; set; }
    }
}
