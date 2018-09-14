namespace Amigo.Tenant.CommandModel.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model : DbContext
    {
        public Model()
            : base("name=amigoTenantDB")
        {
        }

        public virtual DbSet<AppSetting> AppSettings { get; set; }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<BusinessPartner> BusinessPartners { get; set; }
        public virtual DbSet<Concept> Concepts { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractDetail> ContractDetails { get; set; }
        public virtual DbSet<ContractDetailObligation> ContractDetailObligations { get; set; }
        public virtual DbSet<ContractDetailObligationPay> ContractDetailObligationPays { get; set; }
        public virtual DbSet<ContractHouseDetail> ContractHouseDetails { get; set; }
        public virtual DbSet<EntityStatu> EntityStatus { get; set; }
        public virtual DbSet<ExpenseDetail> ExpenseDetails { get; set; }
        public virtual DbSet<Feature> Features { get; set; }
        public virtual DbSet<FeatureAccesory> FeatureAccesories { get; set; }
        public virtual DbSet<FeatureImage> FeatureImages { get; set; }
        public virtual DbSet<GeneralTable> GeneralTables { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<HouseFeature> HouseFeatures { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<IncomeDetail> IncomeDetails { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<OtherTenant> OtherTenants { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppSetting>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AppSetting>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AppSetting>()
                .Property(e => e.AppSettingValue)
                .IsUnicode(false);

            modelBuilder.Entity<AppSetting>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AppSetting>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.ExternalInternal)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<AppUser>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessPartner>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessPartner>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessPartner>()
                .Property(e => e.RUC)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessPartner>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessPartner>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessPartner>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessPartner>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BusinessPartner>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Concept>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Concept>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Concept>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<Concept>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Concept>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Concept>()
                .Property(e => e.ConceptAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Concept>()
                .HasMany(e => e.ExpenseDetails)
                .WithRequired(e => e.Concept)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contract>()
                .Property(e => e.RentDeposit)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Contract>()
                .Property(e => e.RentPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Contract>()
                .Property(e => e.Period)
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                .Property(e => e.ContractCode)
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                .Property(e => e.ReferencedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                .Property(e => e.FrecuencyTypeId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                .HasMany(e => e.ContractHouseDetails)
                .WithRequired(e => e.Contract)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Contract>()
                .HasMany(e => e.ContractDetails)
                .WithRequired(e => e.Contract)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.Rent)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.FinePerDay)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.FineAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.TotalPayment)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ContractDetail>()
                .Property(e => e.PaymentReferenceNo)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetailObligation>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetailObligation>()
                .Property(e => e.InfractionAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ContractDetailObligation>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetailObligation>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetailObligationPay>()
                .Property(e => e.PayAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ContractDetailObligationPay>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ContractDetailObligationPay>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ContractHouseDetail>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ContractHouseDetail>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatu>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatu>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatu>()
                .Property(e => e.EntityCode)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatu>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatu>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatu>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.EntityStatu)
                .HasForeignKey(e => e.ContractStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EntityStatu>()
                .HasMany(e => e.ContractDetails)
                .WithRequired(e => e.EntityStatu)
                .HasForeignKey(e => e.ContractDetailStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EntityStatu>()
                .HasMany(e => e.Houses)
                .WithRequired(e => e.EntityStatu)
                .HasForeignKey(e => e.HouseStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EntityStatu>()
                .HasMany(e => e.HouseFeatures)
                .WithRequired(e => e.EntityStatu)
                .HasForeignKey(e => e.HouseFeatureStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExpenseDetail>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseDetail>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseDetail>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Feature>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Feature>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Feature>()
                .Property(e => e.Measure)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Feature>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Feature>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Feature>()
                .HasMany(e => e.HouseFeatures)
                .WithRequired(e => e.Feature)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FeatureAccesory>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<FeatureAccesory>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<FeatureImage>()
                .Property(e => e.ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<FeatureImage>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<FeatureImage>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralTable>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralTable>()
                .Property(e => e.TableName)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralTable>()
                .Property(e => e.Value)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralTable>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralTable>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<GeneralTable>()
                .HasMany(e => e.BusinessPartners)
                .WithOptional(e => e.GeneralTable)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<GeneralTable>()
                .HasMany(e => e.Concepts)
                .WithRequired(e => e.GeneralTable)
                .HasForeignKey(e => e.PayTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GeneralTable>()
                .HasMany(e => e.Concepts1)
                .WithRequired(e => e.GeneralTable1)
                .HasForeignKey(e => e.TypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GeneralTable>()
                .HasMany(e => e.ContractDetails)
                .WithOptional(e => e.GeneralTable)
                .HasForeignKey(e => e.PayTypeId);

            modelBuilder.Entity<GeneralTable>()
                .HasMany(e => e.FeatureAccesories)
                .WithRequired(e => e.GeneralTable)
                .HasForeignKey(e => e.AccesoryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GeneralTable>()
                .HasMany(e => e.Houses)
                .WithRequired(e => e.GeneralTable)
                .HasForeignKey(e => e.HouseTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GeneralTable>()
                .HasMany(e => e.Tenants)
                .WithOptional(e => e.GeneralTable)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<House>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<House>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<House>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<House>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<House>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<House>()
                .Property(e => e.RentPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<House>()
                .Property(e => e.RentDeposit)
                .HasPrecision(8, 2);

            modelBuilder.Entity<House>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<House>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<House>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.House)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<House>()
                .HasMany(e => e.HouseFeatures)
                .WithRequired(e => e.House)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HouseFeature>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HouseFeature>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<HouseFeature>()
                .Property(e => e.AdditionalAddressInfo)
                .IsUnicode(false);

            modelBuilder.Entity<HouseFeature>()
                .HasMany(e => e.ContractHouseDetails)
                .WithRequired(e => e.HouseFeature)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HouseFeature>()
                .HasMany(e => e.FeatureImages)
                .WithRequired(e => e.HouseFeature)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Income>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<Income>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<Income>()
                .Property(e => e.TotalAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Income>()
                .Property(e => e.Tax)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Income>()
                .Property(e => e.SubTotalAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Income>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Income>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<IncomeDetail>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<IncomeDetail>()
                .Property(e => e.TotalAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<IncomeDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<IncomeDetail>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<IncomeDetail>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.Module1)
                .WithOptional(e => e.Module2)
                .HasForeignKey(e => e.ParentModuleId);

            modelBuilder.Entity<OtherTenant>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<OtherTenant>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Period>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Period>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Period>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.PassportNo)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.PhoneN01)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.Reference)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.PhoneNo2)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.ContactName)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.ContactRelation)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Tenant>()
                .HasMany(e => e.ContractDetailObligations)
                .WithOptional(e => e.Tenant)
                .HasForeignKey(e => e.TenantId);

            modelBuilder.Entity<Tenant>()
                .HasMany(e => e.ContractDetailObligations1)
                .WithOptional(e => e.Tenant1)
                .HasForeignKey(e => e.TenantInfractorId);
        }
    }
}
