global using UserManagementAPI.Models;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
//using Swashbuckle.AspNetCore.SwaggerUI;

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

        public virtual DbSet<TChannelType> TChannelTypes { get; set; } = null!;
        public virtual DbSet<TCity> TCities { get; set; } = null!;
        public virtual DbSet<TCompDigiOutlet> TCompDigiOutlets { get; set; } = null!;
        public virtual DbSet<TCountryLookup> TCountryLookups { get; set; } = null!;
        public virtual DbSet<TDepartment> TDepartments { get; set; } = null!;
        public virtual DbSet<TLogger> TLoggers { get; set; } = null!;
        public virtual DbSet<TModule> TModules { get; set; } = null!;
        public virtual DbSet<TProfile> TProfiles { get; set; } = null!;
        public virtual DbSet<TRegionLookup> TRegionLookups { get; set; } = null!;
        public virtual DbSet<TTemplate> TTemplates { get; set; } = null!;
        public virtual DbSet<TUsrDetail> TUsrDetails { get; set; } = null!;
        public virtual DbSet<Tcompany> Tcompanies { get; set; } = null!;
        public virtual DbSet<Tusr> Tusrs { get; set; } = null!;
        public virtual DbSet<TEvent> TEvents { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(ConfigObject.DB_CONN);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("logEntityValue")
                    .HasComment("the serialized data of the entity being persisted");

                entity.Property(e => e.LogEvent)
                    .HasColumnName("logEvent")
                    .HasComment("the type of event being logged (for the purpose of charging)");
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
                    .HasColumnName("firstname");

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
                    .HasColumnName("othernames");

                entity.Property(e => e.ProfileId)
                    .HasColumnName("profileId")
                    .HasComment("the Id of the profile");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("surname");

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

            modelBuilder.Entity<TEvent>(entity =>
            {
                entity.ToTable("tEvent");

                entity.Property(e => e.Id).HasComment("primary key");

                entity.Property(e => e.EventDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("eventDescription");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
