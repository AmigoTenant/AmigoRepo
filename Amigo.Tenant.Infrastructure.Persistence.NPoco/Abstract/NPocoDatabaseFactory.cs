using NPoco;
using NPoco.FluentMappings;
using Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Abstract
{
    public static class NPocoDatabaseFactory
    {
        public static DatabaseFactory DbFactory { get; set; }

        public static void Setup()
        {
            var fluentConfig = FluentMappingConfiguration.Configure(new IMap[]
            {
                new ActivityTypeMapping(),
                new RequestLogMapping(),
                new CityMapping(),
                new CostCenterMapping(),
                new EquipmentTypeMapping(),
                new EquipmentSizeMapping(),
                new EquipmentStatusMapping(),
                new LocationMapping(),
                new LocationCoordinateMapping(),
                new EquipmentMapping(),
                new PermissionMapping(),
                new AmigoTenantTRoleMapping(),
                new AmigoTenantTRoleBasicMapping(),
                new RoleMapping(), 
                new ModuleMapping(),
                new DeviceMapping(),
                new ProductsMapping(),
                new ServiceMapping(),
                new AmigoTenantTUserMapping(),
                new DispatchingPartyMapping(),
                new AmigoTenantTServiceReportMapping(),
                new ActionMapping(),
                new AmigoTenantTEventLogSearchResultMapping(),
                new ServiceReportMapping(),
                new LatestPositionMapping(),
                new MainMenuMapping(),
                new ModelMapping(),
                new OSVersionMapping(),
                new AmigoTenantTServiceApproveRateMapping(),
                new AppVersionMapping(),
                new LocationTypeMapping(),
                new Last24HoursMapping(),
                new ParentLocationMapping(),
                new ActivityEventLogMapping(),
                new DriverPayReportMapping(),
                new AmigoTenantTServiceLatestMapping(),
                new AmigoTenantParameterMapping() ,
                new UserAuthorizationMapping(),
                new AmigoTenantTUserBasicMapping(),
                new AmigoTenantTServiceMapping(),
                new AmigoTenantTEventLogPerHourMapping(),
                new RateMapping(),
                new ContractMapping(),
                new ContractSearchMapping(),
                new OtherTenantMapping(),
                new ContractHouseDetailMapping(),
                new EntityStatusMapping(),
                new HouseMapping(),
                new PeriodMapping(),
                new ConceptMapping(),
                new FeatureMapping(),
                new GeneralTableMapping(),
                new HouseFeatureAndDetailMapping(),
                new MainTenantMapping(),
                new HouseBasicMapping(),
                new MainTenantBasicMapping(),
                new CountryMapping(),
                new HouseFeatureDetailContractMapping(),
                new HouseTypeMapping(),
                new HouseStatusMapping(),
                new HouseFeatureMapping(),
                new HouseServiceMapping(),
                new ServiceHouseMapping(),
                new PaymentPeriodMapping(),
                new PaymentPeriodByContractMapping(),
                new ServiceHousePeriodMapping(),
                new RentalApplicationMapping(),
                new RentalApplicationRegisterMapping(),
                new UtilityHouseServicePeriodMapping(),
                new PaymentPeriodPrintMapping(),
                new ExpenseSearchMapping(),
                new ExpenseRegisterMapping(),
                new ExpenseDetailSearchMapping(),
                new ExpenseDetailRegisterMapping()

            });            

            DbFactory = DatabaseFactory.Config(x =>
            {
                x.UsingDatabase(() => new Database("amigotenantDb"));
                x.WithFluentConfig(fluentConfig);
                //x.WithMapper();
            });
        }
    }
}
