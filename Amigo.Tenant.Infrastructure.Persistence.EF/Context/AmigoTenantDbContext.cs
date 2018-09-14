using System;
using System.Data.Entity;
using Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context
{
    public class AmigoTenantDbContext : DbContext
    {
        public AmigoTenantDbContext() : base("Name=amigotenantDb")
        {
            Database.SetInitializer<AmigoTenantDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ActionMap());
            modelBuilder.Configurations.Add(new ActivityTypeMap());
            modelBuilder.Configurations.Add(new AppVersionMap());
            modelBuilder.Configurations.Add(new AuditLogMap());
            modelBuilder.Configurations.Add(new BrandMap());
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new CostCenterMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new DeviceMap());
            modelBuilder.Configurations.Add(new DispatchingPartyMap());
            modelBuilder.Configurations.Add(new DriverReportMap());
            modelBuilder.Configurations.Add(new EquipmentMap());
            modelBuilder.Configurations.Add(new EquipmentSizeMap());
            modelBuilder.Configurations.Add(new EquipmentStatuMap());
            modelBuilder.Configurations.Add(new EquipmentTypeMap());
            modelBuilder.Configurations.Add(new LocationMap());
            modelBuilder.Configurations.Add(new LocationCoordinateMap());
            modelBuilder.Configurations.Add(new LocationTypeMap());
            modelBuilder.Configurations.Add(new ModelMap());
            modelBuilder.Configurations.Add(new ModuleMap());
            modelBuilder.Configurations.Add(new OSVersionMap());
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new PlatformMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new RateMap());
            modelBuilder.Configurations.Add(new ServiceMap());
            modelBuilder.Configurations.Add(new ServiceTypeMap());
            modelBuilder.Configurations.Add(new AmigoTenantTEventLogMap());
            modelBuilder.Configurations.Add(new AmigoTenantTRoleMap());
            modelBuilder.Configurations.Add(new AmigoTenantTServiceMap());
            modelBuilder.Configurations.Add(new AmigoTenantTServiceChargeMap());
            modelBuilder.Configurations.Add(new AmigoTenantTUserMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new RequestLogMap());
            modelBuilder.Configurations.Add(new PaymentPeriodMap());

            modelBuilder.Configurations.Add(new HouseServiceMap());
            modelBuilder.Configurations.Add(new ServiceHouseMap());
            modelBuilder.Configurations.Add(new ServiceHousePeriodMap());
            modelBuilder.Configurations.Add(new HouseServicePeriodMap());

            modelBuilder.Configurations.Add(new RentalApplicationMap());

            modelBuilder.Configurations.Add(new ServicePeriodMap());
            modelBuilder.Configurations.Add(new PeriodMap());
            modelBuilder.Configurations.Add(new InvoiceMap());
            modelBuilder.Configurations.Add(new InvoiceDetailMap());

            modelBuilder.Configurations.Add(new ExpenseMap());
            modelBuilder.Configurations.Add(new ExpenseDetailMap());

            //TODO: Llevar a clases de Mapeos
            modelBuilder.Entity<AppSetting>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<AppSetting>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AppSetting>()
                .Property(e => e.AppSettingValue)
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
                .Property(e => e.ConceptAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Contract>()
                .Property(e => e.RentDeposit)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Contract>()
                .Property(e => e.RentPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Contract>()
                .Property(e => e.ContractCode)
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                .Property(e => e.ReferencedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Contract>()
                /*.Property(e => e.FrecuencyTypeId)
                .IsFixedLength()
                .IsUnicode(false)*/;

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

            modelBuilder.Entity<ContractDetailObligationPay>()
                .Property(e => e.PayAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<EntityStatus>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatus>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatus>()
                .Property(e => e.EntityCode)
                .IsUnicode(false);

            modelBuilder.Entity<EntityStatus>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.EntityStatus)
                .HasForeignKey(e => e.ContractStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EntityStatus>()
                .HasMany(e => e.ContractDetails)
                .WithRequired(e => e.EntityStatus)
                .HasForeignKey(e => e.ContractDetailStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EntityStatus>()
                .HasMany(e => e.Houses)
                .WithRequired(e => e.EntityStatus)
                .HasForeignKey(e => e.HouseStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EntityStatus>()
                .HasMany(e => e.HouseFeatures)
                .WithRequired(e => e.EntityStatus)
                .HasForeignKey(e => e.HouseFeatureStatusId)
                .WillCascadeOnDelete(false);

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
                .HasMany(e => e.HouseFeatures)
                .WithRequired(e => e.Feature)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FeatureImage>()
                .Property(e => e.ImagePath)
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
                .Property(e => e.Latitude)
                .HasPrecision(18, 15);

            modelBuilder.Entity<House>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 15);

            modelBuilder.Entity<House>()
                .HasMany(e => e.Contracts)
                .WithRequired(e => e.House)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<House>()
                .HasMany(e => e.HouseFeatures)
                .WithRequired(e => e.House)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<IncomeDetail>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<IncomeDetail>()
                .Property(e => e.TotalAmount)
                .HasPrecision(8, 2);

            modelBuilder.Entity<IncomeDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<Role>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.PassportNo)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.PhoneN01)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.Reference)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.PhoneNo2)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.ContactName)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.ContactPhone)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.ContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.ContactRelation)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .Property(e => e.IdRef)
                .IsUnicode(false);

            modelBuilder.Entity<MainTenant>()
                .HasMany(e => e.ContractDetailObligations)
                .WithOptional(e => e.Tenant)
                .HasForeignKey(e => e.TenantId);

            modelBuilder.Entity<MainTenant>()
                .HasMany(e => e.ContractDetailObligations1)
                .WithOptional(e => e.TenantInfractor)
                .HasForeignKey(e => e.TenantInfractorId);
        }
    }
}
