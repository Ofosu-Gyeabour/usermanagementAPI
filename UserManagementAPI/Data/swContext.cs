﻿global using Microsoft.EntityFrameworkCore;
global using UserManagementAPI.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Swashbuckle.AspNetCore.SwaggerUI;
using UserManagementAPI.utils;

namespace UserManagementAPI.Data
{
    public partial class swContext : DbContext
    {
        public swContext()
        {
        }

        public swContext(DbContextOptions<swContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SaleTypeLookup> SaleTypeLookups { get; set; } = null!;
        public virtual DbSet<TAdhoc> TAdhocs { get; set; } = null!;
        public virtual DbSet<TAdhocItem> TAdhocItems { get; set; } = null!;
        public virtual DbSet<TAdhocPayment> TAdhocPayments { get; set; } = null!;
        public virtual DbSet<TAdhocType> TAdhocTypes { get; set; } = null!;
        public virtual DbSet<TAirport> TAirports { get; set; } = null!;
        public virtual DbSet<TChannelType> TChannelTypes { get; set; } = null!;
        public virtual DbSet<TChargeEngine> TChargeEngines { get; set; } = null!;
        public virtual DbSet<TChargeLookup> TChargeLookups { get; set; } = null!;
        public virtual DbSet<TCity> TCities { get; set; } = null!;
        public virtual DbSet<TClient> TClients { get; set; } = null!;
        public virtual DbSet<TClientAddress> TClientAddresses { get; set; } = null!;
        public virtual DbSet<TClientType> TClientTypes { get; set; } = null!;
        public virtual DbSet<TCompDigiOutlet> TCompDigiOutlets { get; set; } = null!;
        public virtual DbSet<TCongestionCharge> TCongestionCharges { get; set; } = null!;
        public virtual DbSet<TCountryLookup> TCountryLookups { get; set; } = null!;
        public virtual DbSet<TCreditNote> TCreditNotes { get; set; } = null!;
        public virtual DbSet<TDeliveryCharge> TDeliveryCharges { get; set; } = null!;
        public virtual DbSet<TDeliveryMethod> TDeliveryMethods { get; set; } = null!;
        public virtual DbSet<TDeliveryTimeLookup> TDeliveryTimeLookups { get; set; } = null!;
        public virtual DbSet<TDeliveryZone> TDeliveryZones { get; set; } = null!;
        public virtual DbSet<TDepartment> TDepartments { get; set; } = null!;
        public virtual DbSet<TDialCode> TDialCodes { get; set; } = null!;
        public virtual DbSet<TEvent> TEvents { get; set; } = null!;
        public virtual DbSet<TInsurance> TInsurances { get; set; } = null!;
        public virtual DbSet<TLogger> TLoggers { get; set; } = null!;
        public virtual DbSet<TModule> TModules { get; set; } = null!;
        public virtual DbSet<TOrderCharge> TOrderCharges { get; set; } = null!;
        public virtual DbSet<TOrderStatus> TOrderStatuses { get; set; } = null!;
        public virtual DbSet<TOrderStatusLookup> TOrderStatusLookups { get; set; } = null!;
        public virtual DbSet<TOrderType> TOrderTypes { get; set; } = null!;
        public virtual DbSet<TPackagingItem> TPackagingItems { get; set; } = null!;
        public virtual DbSet<TPackagingPrice> TPackagingPrices { get; set; } = null!;
        public virtual DbSet<TPackagingStock> TPackagingStocks { get; set; } = null!;
        public virtual DbSet<TPaymentMethod> TPaymentMethods { get; set; } = null!;
        public virtual DbSet<TPaymentTerm> TPaymentTerms { get; set; } = null!;
        public virtual DbSet<TProfile> TProfiles { get; set; } = null!;
        public virtual DbSet<TRegionLookup> TRegionLookups { get; set; } = null!;
        public virtual DbSet<TSailingSchedule> TSailingSchedules { get; set; } = null!;
        public virtual DbSet<TSaleTypeLookup> TSaleTypeLookups { get; set; } = null!;
        public virtual DbSet<TSealPrice> TSealPrices { get; set; } = null!;
        public virtual DbSet<TSealType> TSealTypes { get; set; } = null!;
        public virtual DbSet<TShipperCategory> TShipperCategories { get; set; } = null!;
        public virtual DbSet<TShipping> TShippings { get; set; } = null!;
        public virtual DbSet<TShippingCharge> TShippingCharges { get; set; } = null!;
        public virtual DbSet<TShippingItem> TShippingItems { get; set; } = null!;
        public virtual DbSet<TShippingLine> TShippingLines { get; set; } = null!;
        public virtual DbSet<TShippingMethod> TShippingMethods { get; set; } = null!;
        public virtual DbSet<TShippingOrderItem> TShippingOrderItems { get; set; } = null!;
        public virtual DbSet<TSla> TSlas { get; set; } = null!;
        public virtual DbSet<TTask> TTasks { get; set; } = null!;
        public virtual DbSet<TTemplate> TTemplates { get; set; } = null!;
        public virtual DbSet<TTitle> TTitles { get; set; } = null!;
        public virtual DbSet<TUsrDetail> TUsrDetails { get; set; } = null!;
        public virtual DbSet<TVessel> TVessels { get; set; } = null!;
        public virtual DbSet<Tbranch> Tbranches { get; set; } = null!;
        public virtual DbSet<Tclientreferralsource> Tclientreferralsources { get; set; } = null!;
        public virtual DbSet<Tcompany> Tcompanies { get; set; } = null!;
        public virtual DbSet<Tcompanytype> Tcompanytypes { get; set; } = null!;
        public virtual DbSet<TcontainerType> TcontainerTypes { get; set; } = null!;
        public virtual DbSet<TemailConfig> TemailConfigs { get; set; } = null!;
        public virtual DbSet<Thscode> Thscodes { get; set; } = null!;
        public virtual DbSet<Tpackaging> Tpackagings { get; set; } = null!;
        public virtual DbSet<Tshippingport> Tshippingports { get; set; } = null!;
        public virtual DbSet<Tusr> Tusrs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(UserManagementAPI.utils.ConfigObject.DB_CONN);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleTypeLookup>(entity =>
            {
                entity.ToTable("SaleTypeLookup");

                entity.Property(e => e.Describ)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TAdhoc>(entity =>
            {
                entity.ToTable("tAdhoc");

                entity.Property(e => e.AdhocDate)
                    .HasColumnType("datetime")
                    .HasColumnName("adhocDate")
                    .HasComment("date for the adhoc txn");

                entity.Property(e => e.AdhocDescrib)
                    .IsUnicode(false)
                    .HasColumnName("adhocDescrib")
                    .HasComment("description of adhoc txn");

                entity.Property(e => e.AdhocTypeId)
                    .HasColumnName("adhocTypeId")
                    .HasComment("reference to the adhoc type table");

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientId")
                    .HasComment("the Id of the client");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("reference to the company table");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasComment("user creating transaction record");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("datetime")
                    .HasColumnName("invoiceDate")
                    .HasComment("date of invoicing");

                entity.Property(e => e.IsInvoiced)
                    .HasColumnName("isInvoiced")
                    .HasComment("flag determining if transaction has been invoiced");

                entity.Property(e => e.IsuploadtoSage)
                    .HasColumnName("isuploadtoSAGE")
                    .HasComment("flag determining if invoice has been uploaded to SAGE. set to automate this with SAGE 200");

                entity.Property(e => e.LastModifedBy)
                    .HasColumnName("lastModifedBy")
                    .HasComment("user who last modified record");

                entity.Property(e => e.OrderNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("orderNo");

                entity.Property(e => e.PaymentTermsId)
                    .HasColumnName("paymentTermsId")
                    .HasComment("reference to payment terms table");

                entity.Property(e => e.Vat)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("vat")
                    .HasComment("VAT component");

                entity.HasOne(d => d.PaymentTerms)
                    .WithMany(p => p.TAdhocs)
                    .HasForeignKey(d => d.PaymentTermsId)
                    .HasConstraintName("FK_tAdhoc_tPaymentTerm");
            });

            modelBuilder.Entity<TAdhocItem>(entity =>
            {
                entity.ToTable("tAdhocItem");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.AddedBy)
                    .HasColumnName("addedBy")
                    .HasComment("user adding item. important should the order be modified in the future");

                entity.Property(e => e.AddedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("addedDate")
                    .HasComment("date item was added");

                entity.Property(e => e.AdhocId)
                    .HasColumnName("adhocId")
                    .HasComment("foreign key to the adhoc table");

                entity.Property(e => e.Describ)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("describ")
                    .HasComment("description of sale items");

                entity.Property(e => e.NCode)
                    .HasMaxLength(10)
                    .HasColumnName("nCode")
                    .IsFixedLength()
                    .HasComment("nom code");

                entity.Property(e => e.NCodeDescrib)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nCodeDescrib")
                    .HasComment("nom code description");

                entity.Property(e => e.Qty)
                    .HasColumnName("qty")
                    .HasComment("quantity or unit of items");

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("totalPrice")
                    .HasComment("total price");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("unitPrice")
                    .HasComment("unit price of item");

                entity.HasOne(d => d.Adhoc)
                    .WithMany(p => p.TAdhocItems)
                    .HasForeignKey(d => d.AdhocId)
                    .HasConstraintName("FK_tAdhocItem_tAdhoc");
            });

            modelBuilder.Entity<TAdhocPayment>(entity =>
            {
                entity.ToTable("tAdhocPayment");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.AdhocId)
                    .HasColumnName("adhocId")
                    .HasComment("adhoc Id: foreign key to the adhoc table");

                entity.Property(e => e.OutstandingAmt)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("outstandingAmt")
                    .HasComment("amount left outstanding");

                entity.Property(e => e.PayAmt)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("payAmt")
                    .HasComment("amount of payment");

                entity.Property(e => e.PayDate)
                    .HasColumnType("date")
                    .HasColumnName("payDate")
                    .HasComment("date of payment");

                entity.Property(e => e.PayMethodId)
                    .HasColumnName("payMethodId")
                    .HasComment("method of payment");

                entity.HasOne(d => d.Adhoc)
                    .WithMany(p => p.TAdhocPayments)
                    .HasForeignKey(d => d.AdhocId)
                    .HasConstraintName("FK_tAdhocPayment_tAdhoc");
            });

            modelBuilder.Entity<TAdhocType>(entity =>
            {
                entity.ToTable("tAdhocType");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.AdhocName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("adhocName")
                    .HasComment("name of adhoc type");

                entity.Property(e => e.Nomcode)
                    .HasMaxLength(12)
                    .HasColumnName("nomcode")
                    .IsFixedLength()
                    .HasComment("nom code of adhoc type");
            });

            modelBuilder.Entity<TAirport>(entity =>
            {
                entity.ToTable("tAirport");

                entity.Property(e => e.Airport)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("airport")
                    .HasComment("name of airport");

                entity.Property(e => e.CountryId)
                    .HasColumnName("countryId")
                    .HasComment("country Id of airport");

                entity.Property(e => e.Mnemonic)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mnemonic")
                    .IsFixedLength()
                    .HasComment("mnemonic or short name of airport");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TAirports)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tAirport_tCountryLookup");
            });

            modelBuilder.Entity<TChannelType>(entity =>
            {
                entity.HasKey(e => e.ChannelTypeId)
                    .HasName("PK_tchannelType");

                entity.ToTable("tChannelType");

                entity.Property(e => e.ChannelTypeId)
                    .HasColumnName("channelTypeID")
                    .HasComment("primary key of table");

                entity.Property(e => e.Channel)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("channel")
                    .HasComment("the type of channel (i.e. fb, twitter, tiktok, instagram, etc)");

                entity.Property(e => e.Describ)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("describ")
                    .HasComment("brief description of channel type");
            });

            modelBuilder.Entity<TChargeEngine>(entity =>
            {
                entity.ToTable("tChargeEngine");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.ChargeId)
                    .HasColumnName("chargeId")
                    .HasComment("description of the order");

                entity.Property(e => e.ChargeRate)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("chargeRate")
                    .HasComment("the rate applied");

                entity.Property(e => e.IsLabel)
                    .HasColumnName("isLabel")
                    .HasComment("label or calc");

                entity.Property(e => e.OrdertypeId)
                    .HasColumnName("ordertypeId")
                    .HasComment("the Id of the order type");

                entity.Property(e => e.ThresholdAmt)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("thresholdAmt")
                    .HasComment("amount for threshold to kick in");

                entity.Property(e => e.ThresholdRate)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("thresholdRate")
                    .HasComment("threshold rate");

                entity.HasOne(d => d.Charge)
                    .WithMany(p => p.TChargeEngines)
                    .HasForeignKey(d => d.ChargeId)
                    .HasConstraintName("FK_tChargeEngine_tChargeLookup");

                entity.HasOne(d => d.Ordertype)
                    .WithMany(p => p.TChargeEngines)
                    .HasForeignKey(d => d.OrdertypeId)
                    .HasConstraintName("FK_tChargeEngine_tOrderType");
            });

            modelBuilder.Entity<TChargeLookup>(entity =>
            {
                entity.ToTable("tChargeLookup");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Charge)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("charge")
                    .HasComment("charge");
            });

            modelBuilder.Entity<TCity>(entity =>
            {
                entity.ToTable("tCity");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.CityName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cityName")
                    .HasComment("name of city");

                entity.Property(e => e.CountryId)
                    .HasColumnName("countryId")
                    .HasComment("Id of country");
            });

            modelBuilder.Entity<TClient>(entity =>
            {
                entity.ToTable("tClient");

                entity.Property(e => e.AssociatedCompanyId).HasColumnName("associatedCompanyId");

                entity.Property(e => e.CanLogin).HasColumnName("canLogin");

                entity.Property(e => e.ChannelTypeId).HasColumnName("channelTypeId");

                entity.Property(e => e.ClientAccNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("clientAccNo");

                entity.Property(e => e.ClientBusinessName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("clientBusinessName");

                entity.Property(e => e.ClientCityId).HasColumnName("clientCityId");

                entity.Property(e => e.ClientCountryId).HasColumnName("clientCountryId");

                entity.Property(e => e.ClientEmailAddr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clientEmailAddr");

                entity.Property(e => e.ClientEmailAddr2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clientEmailAddr2");

                entity.Property(e => e.ClientPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("clientPassword");

                entity.Property(e => e.ClientPostCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("clientPostCode");

                entity.Property(e => e.ClientTypeId).HasColumnName("clientTypeId");

                entity.Property(e => e.CollectionInstruction)
                    .IsUnicode(false)
                    .HasColumnName("collectionInstruction");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.DialcodeId).HasColumnName("dialcodeId");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.HomeTelephone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("homeTelephone");

                entity.Property(e => e.IsShipper).HasColumnName("isShipper");

                entity.Property(e => e.LastModifiedBy).HasColumnName("lastModifiedBy");

                entity.Property(e => e.Middlenames)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("middlenames");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mobileNo");

                entity.Property(e => e.ReferralId).HasColumnName("referralId");

                entity.Property(e => e.Surname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("surname");

                entity.Property(e => e.WhatsappNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("whatsappNo");

                entity.Property(e => e.WorkTelephone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("workTelephone");

                entity.HasOne(d => d.AssociatedCompany)
                    .WithMany(p => p.TClients)
                    .HasForeignKey(d => d.AssociatedCompanyId)
                    .HasConstraintName("FK_tClient_tcompany");

                entity.HasOne(d => d.ChannelType)
                    .WithMany(p => p.TClients)
                    .HasForeignKey(d => d.ChannelTypeId)
                    .HasConstraintName("FK_tClient_tChannelType");

                entity.HasOne(d => d.ClientCity)
                    .WithMany(p => p.TClients)
                    .HasForeignKey(d => d.ClientCityId)
                    .HasConstraintName("FK_tClient_tCity");

                entity.HasOne(d => d.ClientCountry)
                    .WithMany(p => p.TClients)
                    .HasForeignKey(d => d.ClientCountryId)
                    .HasConstraintName("FK_tClient_tCountryLookup");

                entity.HasOne(d => d.ClientType)
                    .WithMany(p => p.TClients)
                    .HasForeignKey(d => d.ClientTypeId)
                    .HasConstraintName("FK_tClient_tClientType");

                entity.HasOne(d => d.Referral)
                    .WithMany(p => p.TClients)
                    .HasForeignKey(d => d.ReferralId)
                    .HasConstraintName("FK_tClient_tclientreferralsource");
            });

            modelBuilder.Entity<TClientAddress>(entity =>
            {
                entity.ToTable("tClientAddress");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.ClientAddr1)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("clientAddr1")
                    .HasComment("first address part");

                entity.Property(e => e.ClientAddr2)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("clientAddr2")
                    .HasComment("second address part");

                entity.Property(e => e.ClientAddr3)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("clientAddr3")
                    .HasComment("third address part");

                entity.Property(e => e.ClientAddr4)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("clientAddr4")
                    .HasComment("fourth address part");

                entity.Property(e => e.ClientId)
                    .HasColumnName("clientId")
                    .HasComment("the Id of the client...reference from tClient");

                entity.Property(e => e.IsUk)
                    .HasColumnName("isUK")
                    .HasComment("bit signalling if the address is a UK address");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.TClientAddresses)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_tClientAddress_tClient");
            });

            modelBuilder.Entity<TClientType>(entity =>
            {
                entity.ToTable("tClientType");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Describ)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("describ")
                    .HasComment("description of client type");
            });

            modelBuilder.Entity<TCompDigiOutlet>(entity =>
            {
                entity.HasKey(e => e.ChannelId)
                    .HasName("PK_tcompanyDigitalChannel");

                entity.ToTable("tCompDigiOutlet");

                entity.Property(e => e.ChannelId)
                    .HasColumnName("channelId")
                    .HasComment("primary key of the digital channel table");

                entity.Property(e => e.ChannelTypeId)
                    .HasColumnName("channelTypeId")
                    .HasComment("Id of the digital channel type (i.e. fb, twitter, tiktok, etc)");

                entity.Property(e => e.CompanyHandle)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("companyHandle")
                    .HasComment("digital handle for the company");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("id of the company");

                entity.HasOne(d => d.ChannelType)
                    .WithMany(p => p.TCompDigiOutlets)
                    .HasForeignKey(d => d.ChannelTypeId)
                    .HasConstraintName("FK_tcompDigiOutlet_tchannelType");
            });

            modelBuilder.Entity<TCongestionCharge>(entity =>
            {
                entity.ToTable("tCongestionCharge");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Charge)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("charge")
                    .HasComment("charge associated with the congestion");

                entity.Property(e => e.PostCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("postCode")
                    .IsFixedLength()
                    .HasComment("postal code");
            });

            modelBuilder.Entity<TCountryLookup>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("PK_tcountry");

                entity.ToTable("tCountryLookup");

                entity.Property(e => e.CountryId)
                    .HasColumnName("countryId")
                    .HasComment("primary key of country table");

                entity.Property(e => e.CountryCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("countryCode")
                    .IsFixedLength()
                    .HasComment("country code for country");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("countryName")
                    .HasComment("the name of the country");

                entity.Property(e => e.RegionId)
                    .HasColumnName("regionId")
                    .HasComment("foreign key to region table");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.TCountryLookups)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_tcountry_tRegion");
            });

            modelBuilder.Entity<TCreditNote>(entity =>
            {
                entity.ToTable("tCreditNote");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.ApprovedBy)
                    .HasColumnName("approvedBy")
                    .HasComment("the user approving the credit note");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasComment("the user creating the credit note");

                entity.Property(e => e.CreditNoteDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creditNoteDate")
                    .HasComment("date for the credit note");

                entity.Property(e => e.Lineamount)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("lineamount")
                    .HasComment("the line amount for the credit note");

                entity.Property(e => e.NoteDescrib)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("noteDescrib")
                    .HasComment("description given in credit note");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderId")
                    .HasComment("the order id: foreign key to the order table (adhoc/sales, packaging or shipping order)");

                entity.Property(e => e.Qty)
                    .HasColumnName("qty")
                    .HasComment("quantity");

                entity.Property(e => e.UploadedToSage)
                    .HasColumnName("uploadedToSage")
                    .HasComment("flag determining if credit note has been uploaded to SAGE");
            });

            modelBuilder.Entity<TDeliveryCharge>(entity =>
            {
                entity.ToTable("tDeliveryCharge");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.ChargeAmt)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("chargeAmt")
                    .HasComment("the charge amount");

                entity.Property(e => e.ChargeId)
                    .HasColumnName("chargeId")
                    .HasComment("the name of the charge");

                entity.Property(e => e.CountryId)
                    .HasColumnName("countryId")
                    .HasComment("the country Id");

                entity.Property(e => e.QtyCummulativeRate).HasColumnType("numeric(9, 2)");

                entity.HasOne(d => d.Charge)
                    .WithMany(p => p.TDeliveryCharges)
                    .HasForeignKey(d => d.ChargeId)
                    .HasConstraintName("FK_tDeliveryCharge_tChargeLookup");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TDeliveryCharges)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tCharge_tCountryLookup");
            });

            modelBuilder.Entity<TDeliveryMethod>(entity =>
            {
                entity.ToTable("tDeliveryMethod");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .HasComment("delivery method description");

                entity.Property(e => e.Method)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("method")
                    .HasComment("delivery method");
            });

            modelBuilder.Entity<TDeliveryTimeLookup>(entity =>
            {
                entity.ToTable("tDeliveryTimeLookup");

                entity.Property(e => e.DeliverytimeDescrib)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("deliverytimeDescrib")
                    .HasComment("delivery time");
            });

            modelBuilder.Entity<TDeliveryZone>(entity =>
            {
                entity.ToTable("tDeliveryZone");

                entity.Property(e => e.CountryId)
                    .HasColumnName("countryId")
                    .HasComment("reference to country");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasComment("zone Description");

                entity.Property(e => e.Zone)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("zone")
                    .HasComment("name of zone");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TDeliveryZones)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tDeliveryZone_tCountryLookup");
            });

            modelBuilder.Entity<TDepartment>(entity =>
            {
                entity.ToTable("tDepartment");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("the Id of the company");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("departmentName")
                    .HasComment("name of department");

                entity.Property(e => e.Describ)
                    .HasColumnType("text")
                    .HasColumnName("describ")
                    .HasComment("the description of the company");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TDepartments)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_tDepartment_tcompany");
            });

            modelBuilder.Entity<TDialCode>(entity =>
            {
                entity.ToTable("tDialCode");

                entity.Property(e => e.Id).HasComment("primary key for the table");

                entity.Property(e => e.Code)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("code")
                    .HasComment("the dialling code");

                entity.Property(e => e.CountryId)
                    .HasColumnName("countryId")
                    .HasComment("the associated country for the dialling code");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TDialCodes)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tDialCode_tCountryLookup");
            });

            modelBuilder.Entity<TEvent>(entity =>
            {
                entity.ToTable("tEvent");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.EventDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("eventDescription");
            });

            modelBuilder.Entity<TInsurance>(entity =>
            {
                entity.ToTable("tInsurance");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .HasComment("description of insurance");

                entity.Property(e => e.InsuranceType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("insuranceType");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("unitPrice")
                    .HasComment("unit price of insurance");
            });

            modelBuilder.Entity<TLogger>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.ToTable("tLogger");

                entity.Property(e => e.LogId)
                    .HasColumnName("logId")
                    .HasComment("id of the log table");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("the company the user belongs to");

                entity.Property(e => e.LogActor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("logActor")
                    .HasComment("the user performing whose actions are being logged");

                entity.Property(e => e.LogDate)
                    .HasColumnType("date")
                    .HasColumnName("logDate")
                    .HasComment("the date user's action is taken place");

                entity.Property(e => e.LogEntity)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("logEntity")
                    .HasComment("the entity being persisted or modified");

                entity.Property(e => e.LogEntityValue)
                    .IsUnicode(false)
                    .HasColumnName("logEntityValue")
                    .HasComment("the serialized data of the entity being persisted");

                entity.Property(e => e.LogEvent)
                    .HasColumnName("logEvent")
                    .HasComment("the type of event being logged (for the purpose of charging)");

                entity.HasOne(d => d.LogEventNavigation)
                    .WithMany(p => p.TLoggers)
                    .HasForeignKey(d => d.LogEvent)
                    .HasConstraintName("FK_tLogger_tEvent");
            });

            modelBuilder.Entity<TModule>(entity =>
            {
                entity.HasKey(e => e.ModuleId);

                entity.ToTable("tModule");

                entity.Property(e => e.ModuleId)
                    .HasColumnName("moduleId")
                    .HasComment("primary key of the module table");

                entity.Property(e => e.DteAdded)
                    .HasColumnType("date")
                    .HasColumnName("dteAdded");

                entity.Property(e => e.InUse)
                    .HasColumnName("inUse")
                    .HasComment("flag indicating if module is in use");

                entity.Property(e => e.PublicName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("publicName")
                    .HasComment("description of the module");

                entity.Property(e => e.SysName)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("sysName")
                    .HasComment("name of the module");
            });

            modelBuilder.Entity<TOrderCharge>(entity =>
            {
                entity.ToTable("tOrderCharge");

                entity.Property(e => e.ChargeId)
                    .HasColumnName("chargeId")
                    .HasComment("the Id of the charge");

                entity.Property(e => e.ChargeRate)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("chargeRate")
                    .HasComment("the rate of the charge");

                entity.Property(e => e.ChargeValue)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("chargeValue")
                    .HasComment("the nominal value upon computation");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderId")
                    .HasComment("order id...for all orders");

                entity.HasOne(d => d.Charge)
                    .WithMany(p => p.TOrderCharges)
                    .HasForeignKey(d => d.ChargeId)
                    .HasConstraintName("FK_tOrderCharge_tChargeLookup");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TOrderCharges)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_tOrderCharge_tShipping");
            });

            modelBuilder.Entity<TOrderStatus>(entity =>
            {
                entity.ToTable("tOrderStatus");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.ActionedBy)
                    .HasColumnName("actionedBy")
                    .HasComment("user performing action");

                entity.Property(e => e.ActionedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("actionedDate")
                    .HasComment("the date of the action");

                entity.Property(e => e.OrderId)
                    .HasColumnName("orderId")
                    .HasComment("Id of order. foreign key to adhoc, packaging or shipping table");

                entity.Property(e => e.OrderStatus)
                    .HasColumnName("orderStatus")
                    .HasComment("the Id of the status (APPROVED, CANCELLED, etc)");

                entity.Property(e => e.OrderTypeId)
                    .HasColumnName("orderTypeId")
                    .HasComment("the type of order. foreign key to ordertype table");

                entity.Property(e => e.Reason)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("reason")
                    .HasComment("the reason for the order status");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.TOrderStatuses)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_tOrderStatus_tShipping");

                entity.HasOne(d => d.OrderStatusNavigation)
                    .WithMany(p => p.TOrderStatuses)
                    .HasForeignKey(d => d.OrderStatus)
                    .HasConstraintName("FK_tOrderStatus_tOrderStatusLookup");

                entity.HasOne(d => d.OrderType)
                    .WithMany(p => p.TOrderStatuses)
                    .HasForeignKey(d => d.OrderTypeId)
                    .HasConstraintName("FK_tOrderStatus_tOrderType");
            });

            modelBuilder.Entity<TOrderStatusLookup>(entity =>
            {
                entity.ToTable("tOrderStatusLookup");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Describ)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("describ")
                    .HasComment("lookup value: APPROVED , CANCELLED");
            });

            modelBuilder.Entity<TOrderType>(entity =>
            {
                entity.ToTable("tOrderType");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Describ)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("describ")
                    .HasComment("description of order type: Sales Order, Packaging Order, Shipping Order, etc");
            });

            modelBuilder.Entity<TPackagingItem>(entity =>
            {
                entity.ToTable("tPackagingItem");

                entity.Property(e => e.Id).HasComment("primary key of the table");

                entity.Property(e => e.PackagingDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("packagingDescription")
                    .HasComment("packaging description");

                entity.Property(e => e.PackagingItem)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("packagingItem")
                    .HasComment("packaging Item");
            });

            modelBuilder.Entity<TPackagingPrice>(entity =>
            {
                entity.ToTable("tPackagingPrice");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("the company selling these packaging items");

                entity.Property(e => e.PackagingItemId)
                    .HasColumnName("packagingItemID")
                    .HasComment("ID of the packaging item");

                entity.Property(e => e.RetailPrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("retailPrice")
                    .HasComment("recommended retail price for packaging item");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("unitPrice")
                    .HasComment("unit price for packaging item");

                entity.Property(e => e.WholesalePrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("wholesalePrice")
                    .HasComment("wholesale price for packaging item");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TPackagingPrices)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_tPackagingPrice_tcompany");

                entity.HasOne(d => d.PackagingItem)
                    .WithMany(p => p.TPackagingPrices)
                    .HasForeignKey(d => d.PackagingItemId)
                    .HasConstraintName("FK_tPackagingPrice_tPackagingItem");
            });

            modelBuilder.Entity<TPackagingStock>(entity =>
            {
                entity.ToTable("tPackagingStock");

                entity.Property(e => e.Id).HasComment("primary id of key");

                entity.Property(e => e.CeilingThreshold)
                    .HasColumnName("ceilingThreshold")
                    .HasComment("maximum stock to be kept for item. alert responsible parties when breached");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("reference to the company");

                entity.Property(e => e.FloorThreshold)
                    .HasColumnName("floorThreshold")
                    .HasComment("floor threshold for packaging item. when breached, trigger a mail and alert responsible parties");

                entity.Property(e => e.InStock)
                    .HasColumnName("inStock")
                    .HasComment("current stock level for packaging item");

                entity.Property(e => e.TpackagingItemId)
                    .HasColumnName("tpackagingItemID")
                    .HasComment("ID of packaging item");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TPackagingStocks)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_tPackagingStock_tcompany");

                entity.HasOne(d => d.TpackagingItem)
                    .WithMany(p => p.TPackagingStocks)
                    .HasForeignKey(d => d.TpackagingItemId)
                    .HasConstraintName("FK_tPackagingStock_tPackagingItem");
            });

            modelBuilder.Entity<TPaymentMethod>(entity =>
            {
                entity.ToTable("tPaymentMethod");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.IsAccnt)
                    .HasColumnName("isAccnt")
                    .HasComment("flag: find out more");

                entity.Property(e => e.Method)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("method")
                    .HasComment("payment method");
            });

            modelBuilder.Entity<TPaymentTerm>(entity =>
            {
                entity.ToTable("tPaymentTerm");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.PaymentTermDescrib)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("paymentTermDescrib")
                    .HasComment("payment term description (WEEKLY, MONTHLY, QUARTERLY, SEMI-ANNUALLY)");
            });

            modelBuilder.Entity<TProfile>(entity =>
            {
                entity.HasKey(e => e.ProfileId);

                entity.ToTable("tProfile");

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profileId")
                    .HasComment("Id of the profile");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("the company using the profile");

                entity.Property(e => e.DteAdded)
                    .HasColumnType("date")
                    .HasColumnName("dteAdded")
                    .HasComment("date profile was added");

                entity.Property(e => e.InUse)
                    .HasColumnName("inUse")
                    .HasComment("flag indicating whether profile is in use or not");

                entity.Property(e => e.ProfileName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("profileName");

                entity.Property(e => e.ProfileString)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("profileString")
                    .HasComment("profile string");
            });

            modelBuilder.Entity<TRegionLookup>(entity =>
            {
                entity.HasKey(e => e.RegionId)
                    .HasName("PK_tRegion");

                entity.ToTable("tRegionLookup");

                entity.Property(e => e.RegionId).HasColumnName("regionId");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("regionName")
                    .HasComment("name of region");
            });

            modelBuilder.Entity<TSailingSchedule>(entity =>
            {
                entity.ToTable("tSailingSchedule");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.ArrivalDate)
                    .HasColumnType("date")
                    .HasColumnName("arrivalDate")
                    .HasComment("arrival date");

                entity.Property(e => e.ClosingDate)
                    .HasColumnType("date")
                    .HasColumnName("closingDate")
                    .HasComment("closing date");

                entity.Property(e => e.DepartureDate)
                    .HasColumnType("date")
                    .HasColumnName("departureDate")
                    .HasComment("departure date");

                entity.Property(e => e.PortOfArrivalId)
                    .HasColumnName("portOfArrivalID")
                    .HasComment("reference to shipping port table. port of arrival");

                entity.Property(e => e.PortOfDepartureId)
                    .HasColumnName("portOfDepartureID")
                    .HasComment("reference to shipping port table. port of departure");

                entity.Property(e => e.VesselId)
                    .HasColumnName("vesselID")
                    .HasComment("reference to vessel table");

                entity.HasOne(d => d.Vessel)
                    .WithMany(p => p.TSailingSchedules)
                    .HasForeignKey(d => d.VesselId)
                    .HasConstraintName("FK_tSailingSchedule_tVessel");
            });

            modelBuilder.Entity<TSaleTypeLookup>(entity =>
            {
                entity.ToTable("tSaleTypeLookup");

                entity.Property(e => e.SaleTypeDescrib)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("saleTypeDescrib")
                    .HasComment("sale type");
            });

            modelBuilder.Entity<TSealPrice>(entity =>
            {
                entity.ToTable("tSealPrice");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("The Id of the company");

                entity.Property(e => e.Price)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("price")
                    .HasComment("the price of the seal type");

                entity.Property(e => e.SealtypeId)
                    .HasColumnName("sealtypeId")
                    .HasComment("the type of seal");

                entity.Property(e => e.Sellingprice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("sellingprice")
                    .HasComment("the selling or trading price of the seal");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TSealPrices)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_tSealPrice_tcompany");

                entity.HasOne(d => d.Sealtype)
                    .WithMany(p => p.TSealPrices)
                    .HasForeignKey(d => d.SealtypeId)
                    .HasConstraintName("FK_tSealPrice_tSealType");
            });

            modelBuilder.Entity<TSealType>(entity =>
            {
                entity.ToTable("tSealType");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.SealTypeDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("sealTypeDescription")
                    .HasComment("description of seal");
            });

            modelBuilder.Entity<TShipperCategory>(entity =>
            {
                entity.ToTable("tShipperCategory");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .HasComment("category of shipping");
            });

            modelBuilder.Entity<TShipping>(entity =>
            {
                entity.ToTable("tShipping");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("primary key");

                entity.Property(e => e.ArrivalPortId)
                    .HasColumnName("arrivalPortId")
                    .HasComment("id of the arrival port");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("company id");

                entity.Property(e => e.ConsignorId)
                    .HasColumnName("consignorId")
                    .HasComment("Id of the consignee");

                entity.Property(e => e.ConsolidatorDescrib)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("consolidatorDescrib")
                    .HasComment("consolidator description");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasComment("system user creating order");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerId")
                    .HasComment("customer placing order");

                entity.Property(e => e.DelMethodId)
                    .HasColumnName("delMethodId")
                    .HasComment("id of the delivery method");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnType("date")
                    .HasColumnName("invoiceDate")
                    .HasComment("date of invoicing");

                entity.Property(e => e.IsConsolidated)
                    .HasColumnName("isConsolidated")
                    .HasComment("flag to determine if order is consolidated");

                entity.Property(e => e.IsInvoiced)
                    .HasColumnName("isInvoiced")
                    .HasComment("flag determining if order has been invoiced");

                entity.Property(e => e.NotifyPartyId)
                    .HasColumnName("notifyPartyId")
                    .HasComment("Id of the notify party");

                entity.Property(e => e.PayMethodId)
                    .HasColumnName("payMethodId")
                    .HasComment("Id of the payment method");

                entity.Property(e => e.ReceipientId)
                    .HasColumnName("receipientId")
                    .HasComment("Id of the receipient");

                entity.Property(e => e.RoutingId)
                    .HasColumnName("routingId")
                    .HasComment("route of shipment");

                entity.Property(e => e.SealPrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("sealPrice")
                    .HasComment("price of seal");

                entity.Property(e => e.SealQty)
                    .HasColumnName("sealQty")
                    .HasComment("seal quantity");
            });

            modelBuilder.Entity<TShippingCharge>(entity =>
            {
                entity.ToTable("tShippingCharge");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("primary key");

                entity.Property(e => e.CnD)
                    .HasColumnType("numeric(9, 2)")
                    .HasComment("C&D");

                entity.Property(e => e.Cs)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("CS")
                    .HasComment("CS charge");

                entity.Property(e => e.Customs)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("customs")
                    .HasComment("customs charge");

                entity.Property(e => e.DestinationThc)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("destinationTHC")
                    .HasComment("terminal haulage charge for the destination");

                entity.Property(e => e.Dmtlcar)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("DMTLCar")
                    .HasComment("DMTL Car");

                entity.Property(e => e.Docs)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("docs")
                    .HasComment("docs charge");

                entity.Property(e => e.Duties)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("duties")
                    .HasComment("duties");

                entity.Property(e => e.Freight)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("freight")
                    .HasComment("nominal value of freight charge");

                entity.Property(e => e.FreightDenom)
                    .HasColumnName("freightDenom")
                    .HasComment("the denomination of the freight");

                entity.Property(e => e.FreightRate)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("freightRate")
                    .HasComment("freight rate");

                entity.Property(e => e.Haulage)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("haulage")
                    .HasComment("haulage charge");

                entity.Property(e => e.Hazmat)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("hazmat")
                    .HasComment("hazmat charge");

                entity.Property(e => e.ImportLic)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("importLic")
                    .HasComment("import license charge");

                entity.Property(e => e.InsuranceId)
                    .HasColumnName("insuranceId")
                    .HasComment("Id of the insurance");

                entity.Property(e => e.InsuranceValue)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("insuranceValue")
                    .HasComment("the nominal value of the insurance");

                entity.Property(e => e.Packing)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("packing")
                    .HasComment("packing charge");

                entity.Property(e => e.ShippingId)
                    .HasColumnName("shippingId")
                    .HasComment("Id of the shipping order");

                entity.Property(e => e.TerminalHandlingCharge)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("terminalHandlingCharge")
                    .HasComment("terminal handling charge (thc)");

                entity.Property(e => e.VerifiedGrossMass)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("verifiedGrossMass")
                    .HasComment("verified gross mass (VGM)");

                entity.Property(e => e.Wrapping)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("wrapping")
                    .HasComment("wrapping charge");
            });

            modelBuilder.Entity<TShippingItem>(entity =>
            {
                entity.ToTable("tShippingItem");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CountryId).HasColumnName("countryId");

                entity.Property(e => e.ItemDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("itemDescription");

                entity.Property(e => e.ItemName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("itemName");

                entity.Property(e => e.ItemPrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("itemPrice");

                entity.Property(e => e.PluralName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("pluralName");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TShippingItems)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tShippingItem_tCountryLookup");
            });

            modelBuilder.Entity<TShippingLine>(entity =>
            {
                entity.ToTable("tShippingLine");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.ShippingLine)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("shippingLine")
                    .HasComment("name of shipping line");
            });

            modelBuilder.Entity<TShippingMethod>(entity =>
            {
                entity.ToTable("tShippingMethod");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Method)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("method")
                    .HasComment("shipping method");

                entity.Property(e => e.Route)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("route")
                    .HasComment("route to use (Air, Land, Sea, etc)");
            });

            modelBuilder.Entity<TShippingOrderItem>(entity =>
            {
                entity.ToTable("tShippingOrderItem");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasComment("primary key");

                entity.Property(e => e.Hscode)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("hscode")
                    .HasComment("hs code");

                entity.Property(e => e.ItemDescription)
                    .HasMaxLength(4000)
                    .IsUnicode(false)
                    .HasColumnName("itemDescription")
                    .HasComment("description of the item");

                entity.Property(e => e.ItemId)
                    .HasColumnName("itemId")
                    .HasComment("the Item Id");

                entity.Property(e => e.ItemPicPath)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("itemPicPath")
                    .HasComment("path to uploaded reference image or picture");

                entity.Property(e => e.ItemVolume)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("itemVolume")
                    .HasComment("volume of the item");

                entity.Property(e => e.ItemWeight)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("itemWeight")
                    .HasComment("weight of the item");

                entity.Property(e => e.LpId)
                    .HasColumnName("lpId")
                    .HasComment("lpId");

                entity.Property(e => e.Marks)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("marks")
                    .HasComment("marks");

                entity.Property(e => e.Qty)
                    .HasColumnName("qty")
                    .HasComment("the quantity of the items");

                entity.Property(e => e.ShippingorderId)
                    .HasColumnName("shippingorderId")
                    .HasComment("shipping order Id");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("unitPrice")
                    .HasComment("unitprice of the item");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.TShippingOrderItems)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_tShippingOrderItem_tShippingItem");
            });

            modelBuilder.Entity<TSla>(entity =>
            {
                entity.ToTable("tSLA");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.TaskId)
                    .HasColumnName("taskId")
                    .HasComment("reference to task table");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.TSlas)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK_tSLA_tTask");
            });

            modelBuilder.Entity<TTask>(entity =>
            {
                entity.ToTable("tTask");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.TaskDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("taskDescription");

                entity.Property(e => e.TaskName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("taskName")
                    .HasComment("task");
            });

            modelBuilder.Entity<TTemplate>(entity =>
            {
                entity.HasKey(e => e.TemplateId);

                entity.ToTable("tTemplate");

                entity.Property(e => e.TemplateId).HasColumnName("templateId");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("Id of the company");

                entity.Property(e => e.TemplateDetails)
                    .HasColumnType("text")
                    .HasColumnName("templateDetails")
                    .HasComment("serialized data making up the template");

                entity.Property(e => e.TemplateName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("templateName")
                    .HasComment("name of the template");
            });

            modelBuilder.Entity<TTitle>(entity =>
            {
                entity.ToTable("tTitle");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title")
                    .HasComment("title name");
            });

            modelBuilder.Entity<TUsrDetail>(entity =>
            {
                entity.ToTable("tUsrDetail");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.MobileTel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mobileTel")
                    .HasComment("mobile telephone");

                entity.Property(e => e.OfficeTel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("officeTel")
                    .HasComment("office telephone");

                entity.Property(e => e.TuserId)
                    .HasColumnName("tuserId")
                    .HasComment("the user Id (foreign key)");

                entity.HasOne(d => d.Tuser)
                    .WithMany(p => p.TUsrDetails)
                    .HasForeignKey(d => d.TuserId)
                    .HasConstraintName("FK_tUsrDetail_tusr");
            });

            modelBuilder.Entity<TVessel>(entity =>
            {
                entity.ToTable("tVessel");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.ShippingLineId)
                    .HasColumnName("shippingLineID")
                    .HasComment("reference to shipping Line table");

                entity.Property(e => e.VesselFlag)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("vesselFlag")
                    .HasComment("the flag of the vessel");

                entity.Property(e => e.VesselName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("vesselName")
                    .HasComment("the name of the vessel");

                entity.HasOne(d => d.ShippingLine)
                    .WithMany(p => p.TVessels)
                    .HasForeignKey(d => d.ShippingLineId)
                    .HasConstraintName("FK_tVessel_tShippingLine");
            });

            modelBuilder.Entity<Tbranch>(entity =>
            {
                entity.ToTable("tbranch");

                entity.Property(e => e.BranchName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("branchName")
                    .HasComment("name of the branch");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("Id of the company");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Tbranches)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_tbranch_tcompany");
            });

            modelBuilder.Entity<Tclientreferralsource>(entity =>
            {
                entity.ToTable("tclientreferralsource");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("primary key");

                entity.Property(e => e.ReferralSource)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("referralSource")
                    .HasComment("referral source (newspapers, radio, etc)");
            });

            modelBuilder.Entity<Tcompany>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.ToTable("tcompany");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("primary key");

                entity.Property(e => e.Company)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("company")
                    .HasComment("name of the company");

                entity.Property(e => e.CompanyAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("companyAddress")
                    .HasComment("address of the company");

                entity.Property(e => e.CompanyCountryId)
                    .HasColumnName("companyCountryId")
                    .HasComment("region where company is located");

                entity.Property(e => e.CompanyLogo)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("companyLogo")
                    .HasComment("logo of company");

                entity.Property(e => e.CompanyTownId).HasColumnName("companyTownId");

                entity.Property(e => e.IncorporationDate)
                    .HasColumnType("date")
                    .HasColumnName("incorporationDate")
                    .HasComment("date of incorporation");

                entity.HasOne(d => d.CompanyCountry)
                    .WithMany(p => p.Tcompanies)
                    .HasForeignKey(d => d.CompanyCountryId)
                    .HasConstraintName("FK_tcompany_tcountry");
            });

            modelBuilder.Entity<Tcompanytype>(entity =>
            {
                entity.ToTable("tcompanytype");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<TcontainerType>(entity =>
            {
                entity.ToTable("tcontainerType");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("primary key");

                entity.Property(e => e.Ctype)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ctype")
                    .HasComment("container type");

                entity.Property(e => e.Cvolume)
                    .HasColumnType("decimal(9, 2)")
                    .HasColumnName("cvolume")
                    .HasComment("container volume");
            });

            modelBuilder.Entity<TemailConfig>(entity =>
            {
                entity.ToTable("temailConfig");

                entity.Property(e => e.BifaSign)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bifaSign")
                    .HasComment("signature of BIFA");

                entity.Property(e => e.BifaUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("bifaURL")
                    .HasComment("bifa URL");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("the Id of the company");

                entity.Property(e => e.Host)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("host")
                    .HasComment("mail host");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasComment("flag determining if mail account is active");

                entity.Property(e => e.LogoUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("logoURL")
                    .HasComment("logo for the URL");

                entity.Property(e => e.MBcc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mBCC")
                    .HasComment("mail blind copied to");

                entity.Property(e => e.MCc)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("mCC")
                    .HasComment("mail copied to");

                entity.Property(e => e.MTo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mTo")
                    .HasComment("receipient of mail");

                entity.Property(e => e.Mfrom)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mfrom")
                    .HasComment("mail originator");

                entity.Property(e => e.Port)
                    .HasColumnName("port")
                    .HasComment("mail port");

                entity.Property(e => e.UsrCredential)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usrCredential")
                    .HasComment("user credentail for mail");

                entity.Property(e => e.UsrPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usrPassword")
                    .HasComment("user password for mail");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.TemailConfigs)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_temailConfig_tcompany");
            });

            modelBuilder.Entity<Thscode>(entity =>
            {
                entity.ToTable("thscode");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description")
                    .HasComment("description of code");

                entity.Property(e => e.Hscode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("hscode")
                    .HasComment("hs code");
            });

            modelBuilder.Entity<Tpackaging>(entity =>
            {
                entity.ToTable("tpackaging");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("primary key");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("the Id of the company having the packaging item");

                entity.Property(e => e.Dimension)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dimension")
                    .HasComment("dimensions of packaging item");

                entity.Property(e => e.Instock)
                    .HasColumnName("instock")
                    .HasComment("unit of items in stock. gets updated as items gets used");

                entity.Property(e => e.Itemcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("itemcode")
                    .HasComment("code of the item: to differentiate it when two companies have the same items");

                entity.Property(e => e.Itemdescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("itemdescription")
                    .HasComment("brief description of packaging item");

                entity.Property(e => e.Packagingitem)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("packagingitem")
                    .HasComment("packaging item");

                entity.Property(e => e.Rrp)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("rrp")
                    .HasComment("rrp of packaging item. Find out what rrp means");

                entity.Property(e => e.Stockthreshold)
                    .HasColumnName("stockthreshold")
                    .HasComment("the threshold at which a notification should be sent to management");

                entity.Property(e => e.Unitprice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("unitprice")
                    .HasComment("unit price for packaging item");

                entity.Property(e => e.Wholesaleprice)
                    .HasColumnType("numeric(9, 2)")
                    .HasColumnName("wholesaleprice")
                    .HasComment("wholesale price for packaging item");
            });

            modelBuilder.Entity<Tshippingport>(entity =>
            {
                entity.ToTable("tshippingport");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("primary key");

                entity.Property(e => e.CountryId)
                    .HasColumnName("countryId")
                    .HasComment("country Id");

                entity.Property(e => e.NameOfport)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nameOfport")
                    .HasComment("name of port");

                entity.Property(e => e.Portcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("portcode")
                    .HasComment("port code");

                entity.Property(e => e.TraveltimeInDays)
                    .HasColumnName("traveltimeInDays")
                    .HasComment("days it will take to travel");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Tshippingports)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tshippingport_tCountryLookup");
            });

            modelBuilder.Entity<Tusr>(entity =>
            {
                entity.HasKey(e => e.UsrId);

                entity.ToTable("tusr");

                entity.Property(e => e.UsrId)
                    .HasColumnName("usrId")
                    .HasComment("primary key");

                entity.Property(e => e.CompanyId)
                    .HasColumnName("companyId")
                    .HasComment("Id of company user belongs to");

                entity.Property(e => e.DepartmentId)
                    .HasColumnName("departmentId")
                    .HasComment("the department the user belongs to");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstname")
                    .HasComment("first name of the user");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isActive")
                    .HasComment("determines if user is active or has been de-activated");

                entity.Property(e => e.IsAdmin)
                    .HasColumnName("isAdmin")
                    .HasComment("flag determining if user is and Administration");

                entity.Property(e => e.IsLogged)
                    .HasColumnName("isLogged")
                    .HasComment("flag determining if user is logged");

                entity.Property(e => e.Othernames)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("othernames")
                    .HasComment("other names of the user");

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profileId")
                    .HasComment("the Id of the profile");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("surname")
                    .HasComment("surname of the user");

                entity.Property(e => e.Usrname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usrname")
                    .HasComment("user name ");

                entity.Property(e => e.Usrpassword)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("usrpassword")
                    .HasComment("password for user name");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Tusrs)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_tusr_tcompany");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Tusrs)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_tusr_tDepartment");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Tusrs)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_tusr_tProfile");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
