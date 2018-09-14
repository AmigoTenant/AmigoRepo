--DATABASE OBJECTS CREATION
GO
ALTER TABLE [sec].[UserRole] DROP CONSTRAINT [fkUser_UserRole]
GO
ALTER TABLE [sec].[UserRole] DROP CONSTRAINT [fkRole_UserRole]
GO
ALTER TABLE [sec].[UserLogin] DROP CONSTRAINT [fkUser_UserLogin]
GO
ALTER TABLE [sec].[UserClaim] DROP CONSTRAINT [fkUser_UserClaim]
GO
ALTER TABLE [sec].[ScopeSecrets] DROP CONSTRAINT [fkScopes_ScopeSecrets]
GO
ALTER TABLE [sec].[ScopeClaims] DROP CONSTRAINT [fkScopes_ScopeClaims]
GO
ALTER TABLE [sec].[ClientSecrets] DROP CONSTRAINT [fkClients_ClientSecrets]
GO
ALTER TABLE [sec].[ClientScopes] DROP CONSTRAINT [fkClients_ClientScopes]
GO
ALTER TABLE [sec].[ClientRedirectUris] DROP CONSTRAINT [fkClients_ClientRedirectUris]
GO
ALTER TABLE [sec].[ClientPostLogoutRedirectUris] DROP CONSTRAINT [fkClients_ClientPostLogoutRedirectUris]
GO
ALTER TABLE [sec].[ClientIdPRestrictions] DROP CONSTRAINT [fkClients_ClientIdPRestrictions]
GO
ALTER TABLE [sec].[ClientCustomGrantTypes] DROP CONSTRAINT [fkClients_ClientCustomGrantTypes]
GO
ALTER TABLE [sec].[ClientCorsOrigins] DROP CONSTRAINT [fkClients_ClientCorsOrigins]
GO
ALTER TABLE [sec].[ClientClaims] DROP CONSTRAINT [fkClients_ClientClaims]
GO
ALTER TABLE [dbo].[Tenant] DROP CONSTRAINT [R_58]
GO
ALTER TABLE [dbo].[Tenant] DROP CONSTRAINT [fk_Tenant_CountryId]
GO
ALTER TABLE [dbo].[State] DROP CONSTRAINT [fkCountry_State]
GO
ALTER TABLE [dbo].[ServicePeriodStatus] DROP CONSTRAINT [fkServicePeriodStatus_EntityStatusId]
GO
ALTER TABLE [dbo].[ServicePeriod] DROP CONSTRAINT [R_92]
GO
ALTER TABLE [dbo].[ServicePeriod] DROP CONSTRAINT [R_91]
GO
ALTER TABLE [dbo].[ServicePeriod] DROP CONSTRAINT [R_90]
GO
ALTER TABLE [dbo].[ServicePeriod] DROP CONSTRAINT [R_87]
GO
ALTER TABLE [dbo].[ServicePeriod] DROP CONSTRAINT [R_124]
GO
ALTER TABLE [dbo].[ServicePeriod] DROP CONSTRAINT [R_123]
GO
ALTER TABLE [dbo].[ServicePeriod] DROP CONSTRAINT [fkServicePeriod_HouseServicePeriodId]
GO
ALTER TABLE [dbo].[ServiceHousePeriod] DROP CONSTRAINT [fkServiceHousePeriod_ServiceId]
GO
ALTER TABLE [dbo].[ServiceHouse] DROP CONSTRAINT [fkService_ServiceTypeId]
GO
ALTER TABLE [dbo].[ServiceHouse] DROP CONSTRAINT [fkService_ConceptId]
GO
ALTER TABLE [dbo].[ServiceHouse] DROP CONSTRAINT [fkService_BusinessPartnerId]
GO
ALTER TABLE [dbo].[Service] DROP CONSTRAINT [fkServiceType_Service]
GO
ALTER TABLE [dbo].[RentalApplicationFeature] DROP CONSTRAINT [FK_RentalApplicationFeature_RentalApplicationId]
GO
ALTER TABLE [dbo].[RentalApplicationFeature] DROP CONSTRAINT [FK_RentalApplicationFeature_FeatureId]
GO
ALTER TABLE [dbo].[RentalApplicationCity] DROP CONSTRAINT [FK_RentalApplicationCity_RentalApplicationId]
GO
ALTER TABLE [dbo].[RentalApplicationCity] DROP CONSTRAINT [FK_RentalApplicationCity_CityId]
GO
ALTER TABLE [dbo].[RentalApplication] DROP CONSTRAINT [fkRentalApplication_ReferredById]
GO
ALTER TABLE [dbo].[RentalApplication] DROP CONSTRAINT [fkRentalApplication_OutInDownId]
GO
ALTER TABLE [dbo].[RentalApplication] DROP CONSTRAINT [fkRentalApplication_HousePartId]
GO
ALTER TABLE [dbo].[RentalApplication] DROP CONSTRAINT [fkRentalApplication_CityOfInterestId]
GO
ALTER TABLE [dbo].[RentalApplication] DROP CONSTRAINT [FK_RentalApplication_ResidenseCountryId]
GO
ALTER TABLE [dbo].[RentalApplication] DROP CONSTRAINT [FK_RentalApplication_PropertyTypeId]
GO
ALTER TABLE [dbo].[RentalApplication] DROP CONSTRAINT [FK_RentalApplication_PeriodId]
GO
ALTER TABLE [dbo].[RentalApplication] DROP CONSTRAINT [FK_RentalApplication_BudgetId]
GO
ALTER TABLE [dbo].[Rate] DROP CONSTRAINT [fkService_Rate]
GO
ALTER TABLE [dbo].[Permission] DROP CONSTRAINT [R_13]
GO
ALTER TABLE [dbo].[Permission] DROP CONSTRAINT [fkAmigoTenantTRole_Permission]
GO
ALTER TABLE [dbo].[Permission] DROP CONSTRAINT [fkAction_Permission]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [R_117]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [R_114]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [R_113]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [R_112]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [R_111]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [fkPaymentPeriod_TenantId]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [fkPaymentPeriod_PeriodId]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [fkPaymentPeriod_PaymentTypeId]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [fkPaymentPeriod_PaymentPeriodStatusId]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [fkPaymentPeriod_ContractId]
GO
ALTER TABLE [dbo].[PaymentPeriod] DROP CONSTRAINT [fkPaymentPeriod_ConceptId]
GO
ALTER TABLE [dbo].[OtherTenant] DROP CONSTRAINT [R_44]
GO
ALTER TABLE [dbo].[OtherTenant] DROP CONSTRAINT [R_43]
GO
ALTER TABLE [dbo].[OSVersion] DROP CONSTRAINT [fkPlatform_OSVersion]
GO
ALTER TABLE [dbo].[Module] DROP CONSTRAINT [R_42]
GO
ALTER TABLE [dbo].[Model] DROP CONSTRAINT [fkBrand_Model]
GO
ALTER TABLE [dbo].[LocationCoordinate] DROP CONSTRAINT [fkLocation_LocationCoordinate]
GO
ALTER TABLE [dbo].[Location] DROP CONSTRAINT [fkLocationType_Location]
GO
ALTER TABLE [dbo].[Location] DROP CONSTRAINT [fkCity_Location]
GO
ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [fkInvoiceDetail_InvoiceId]
GO
ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [fkInvoiceDetail_ConceptId]
GO
ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [fkInvoceDetail_PaymentPeriodId]
GO
ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [fkInvoice_PaymentTypeId]
GO
ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [fkInvoice_EntityStatusId]
GO
ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [fkInvoice_ContractId]
GO
ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [fkInvoice_BusinessPartnerId]
GO
ALTER TABLE [dbo].[IncomeDetail] DROP CONSTRAINT [R_72]
GO
ALTER TABLE [dbo].[IncomeDetail] DROP CONSTRAINT [R_54]
GO
ALTER TABLE [dbo].[IncomeDetail] DROP CONSTRAINT [R_53]
GO
ALTER TABLE [dbo].[IncomeDetail] DROP CONSTRAINT [R_52]
GO
ALTER TABLE [dbo].[Income] DROP CONSTRAINT [R_69]
GO
ALTER TABLE [dbo].[Income] DROP CONSTRAINT [R_56]
GO
ALTER TABLE [dbo].[Income] DROP CONSTRAINT [R_55]
GO
ALTER TABLE [dbo].[Income] DROP CONSTRAINT [R_51]
GO
ALTER TABLE [dbo].[HouseServicePeriod] DROP CONSTRAINT [fkHouseServicePeriod_HouseServiceId]
GO
ALTER TABLE [dbo].[HouseService] DROP CONSTRAINT [fkHouseService_ServiceId]
GO
ALTER TABLE [dbo].[HouseService] DROP CONSTRAINT [fkHouseService_HouseId]
GO
ALTER TABLE [dbo].[HouseFeature] DROP CONSTRAINT [R_38]
GO
ALTER TABLE [dbo].[HouseFeature] DROP CONSTRAINT [R_15]
GO
ALTER TABLE [dbo].[HouseFeature] DROP CONSTRAINT [R_14]
GO
ALTER TABLE [dbo].[House] DROP CONSTRAINT [R_37]
GO
ALTER TABLE [dbo].[House] DROP CONSTRAINT [R_30]
GO
ALTER TABLE [dbo].[House] DROP CONSTRAINT [fkHouse_CityId]
GO
ALTER TABLE [dbo].[FinePayment] DROP CONSTRAINT [fkFinePayment_FineId]
GO
ALTER TABLE [dbo].[Fine] DROP CONSTRAINT [fkFine_TenantId]
GO
ALTER TABLE [dbo].[Fine] DROP CONSTRAINT [fkFine_PeriodId]
GO
ALTER TABLE [dbo].[Fine] DROP CONSTRAINT [fkFine_EntityStatusId]
GO
ALTER TABLE [dbo].[Fine] DROP CONSTRAINT [fkFine_ContractId]
GO
ALTER TABLE [dbo].[Fine] DROP CONSTRAINT [fkFine_ConceptId]
GO
ALTER TABLE [dbo].[FeatureImage] DROP CONSTRAINT [R_16]
GO
ALTER TABLE [dbo].[FeatureAccesory] DROP CONSTRAINT [R_40]
GO
ALTER TABLE [dbo].[FeatureAccesory] DROP CONSTRAINT [R_28]
GO
ALTER TABLE [dbo].[Feature] DROP CONSTRAINT [FK_Feature_HouseTypeId]
GO
ALTER TABLE [dbo].[ExpenseDetail] DROP CONSTRAINT [R_70]
GO
ALTER TABLE [dbo].[ExpenseDetail] DROP CONSTRAINT [R_68]
GO
ALTER TABLE [dbo].[ExpenseDetail] DROP CONSTRAINT [R_27]
GO
ALTER TABLE [dbo].[EquipmentSize] DROP CONSTRAINT [fkEquipmentType_EquipmentSize]
GO
ALTER TABLE [dbo].[Equipment] DROP CONSTRAINT [fkLocation_Equipment]
GO
ALTER TABLE [dbo].[Equipment] DROP CONSTRAINT [fkEquipmentStatus_Equipment]
GO
ALTER TABLE [dbo].[Equipment] DROP CONSTRAINT [fkEquipmentSize_Equipment]
GO
ALTER TABLE [dbo].[DriverReport] DROP CONSTRAINT [fkSystemUser_DriverReportDriverUserId]
GO
ALTER TABLE [dbo].[DriverReport] DROP CONSTRAINT [fkSystemUser_DriverReportApproverUserId]
GO
ALTER TABLE [dbo].[Device] DROP CONSTRAINT [fkOSVersion_Device]
GO
ALTER TABLE [dbo].[Device] DROP CONSTRAINT [fkModel_Device]
GO
ALTER TABLE [dbo].[Device] DROP CONSTRAINT [fkAppVersion_Device]
GO
ALTER TABLE [dbo].[Device] DROP CONSTRAINT [fkAmigoTenantTUser_Device]
GO
ALTER TABLE [dbo].[ContractHouseDetail] DROP CONSTRAINT [R_24]
GO
ALTER TABLE [dbo].[ContractHouseDetail] DROP CONSTRAINT [R_23]
GO
ALTER TABLE [dbo].[ContractDetailObligationPay] DROP CONSTRAINT [R_76]
GO
ALTER TABLE [dbo].[ContractDetailObligationPay] DROP CONSTRAINT [R_73]
GO
ALTER TABLE [dbo].[ContractDetailObligation] DROP CONSTRAINT [R_75]
GO
ALTER TABLE [dbo].[ContractDetailObligation] DROP CONSTRAINT [R_74]
GO
ALTER TABLE [dbo].[ContractDetailObligation] DROP CONSTRAINT [R_71]
GO
ALTER TABLE [dbo].[ContractDetailObligation] DROP CONSTRAINT [R_62]
GO
ALTER TABLE [dbo].[ContractDetailObligation] DROP CONSTRAINT [R_61]
GO
ALTER TABLE [dbo].[ContractDetailObligation] DROP CONSTRAINT [R_60]
GO
ALTER TABLE [dbo].[ContractDetail] DROP CONSTRAINT [R_77]
GO
ALTER TABLE [dbo].[ContractDetail] DROP CONSTRAINT [R_63]
GO
ALTER TABLE [dbo].[ContractDetail] DROP CONSTRAINT [R_4]
GO
ALTER TABLE [dbo].[ContractDetail] DROP CONSTRAINT [R_32]
GO
ALTER TABLE [dbo].[Contract] DROP CONSTRAINT [R_57]
GO
ALTER TABLE [dbo].[Contract] DROP CONSTRAINT [R_31]
GO
ALTER TABLE [dbo].[Contract] DROP CONSTRAINT [R_26]
GO
ALTER TABLE [dbo].[Contract] DROP CONSTRAINT [FK_Contract_PeriodId]
GO
ALTER TABLE [dbo].[Concept] DROP CONSTRAINT [R_36]
GO
ALTER TABLE [dbo].[Concept] DROP CONSTRAINT [R_34]
GO
ALTER TABLE [dbo].[City] DROP CONSTRAINT [fkState_City]
GO
ALTER TABLE [dbo].[BusinessPartner] DROP CONSTRAINT [R_47]
GO
ALTER TABLE [dbo].[BusinessPartner] DROP CONSTRAINT [R_46]
GO
ALTER TABLE [dbo].[BusinessPartner] DROP CONSTRAINT [fkBusinessPartner_CityId]
GO
ALTER TABLE [dbo].[AppUser] DROP CONSTRAINT [R_41]
GO
ALTER TABLE [dbo].[AmigoTenantTUser] DROP CONSTRAINT [fkLocation_AmigoTenantTUser]
GO
ALTER TABLE [dbo].[AmigoTenantTUser] DROP CONSTRAINT [fkAmigoTenantTRole_AmigoTenantTUser]
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge] DROP CONSTRAINT [fkRate_AmigoTenantTServiceCharge]
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge] DROP CONSTRAINT [fkDriverReport_AmigoTenantTServiceCharge]
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge] DROP CONSTRAINT [fkAmigoTenantTService_AmigoTenantTServiceCharge]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkService_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkProduct_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkLocation_AmigoTenantMoveOriginLocationId]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkLocation_AmigoTenantMoveDestinationLocationId]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkEquipmentType_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkEquipmentStatus_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkEquipmentSize_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkDispatchingParty_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService] DROP CONSTRAINT [fkAmigoTenantTUser_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTEventLog] DROP CONSTRAINT [fkActivityType_AmigoTenantTEventLog]
GO
ALTER TABLE [dbo].[Action] DROP CONSTRAINT [R_12]
GO
ALTER TABLE [dbo].[Action] DROP CONSTRAINT [fkModule_Action]
GO
/****** Object:  Table [sec].[UserRole]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[UserRole]
GO
/****** Object:  Table [sec].[UserLogin]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[UserLogin]
GO
/****** Object:  Table [sec].[UserClaim]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[UserClaim]
GO
/****** Object:  Table [sec].[User]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[User]
GO
/****** Object:  Table [sec].[Tokens]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[Tokens]
GO
/****** Object:  Table [sec].[ScopeSecrets]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ScopeSecrets]
GO
/****** Object:  Table [sec].[Scopes]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[Scopes]
GO
/****** Object:  Table [sec].[ScopeClaims]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ScopeClaims]
GO
/****** Object:  Table [sec].[Role]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[Role]
GO
/****** Object:  Table [sec].[Consents]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[Consents]
GO
/****** Object:  Table [sec].[ClientSecrets]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ClientSecrets]
GO
/****** Object:  Table [sec].[ClientScopes]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ClientScopes]
GO
/****** Object:  Table [sec].[Clients]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[Clients]
GO
/****** Object:  Table [sec].[ClientRedirectUris]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ClientRedirectUris]
GO
/****** Object:  Table [sec].[ClientPostLogoutRedirectUris]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ClientPostLogoutRedirectUris]
GO
/****** Object:  Table [sec].[ClientIdPRestrictions]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ClientIdPRestrictions]
GO
/****** Object:  Table [sec].[ClientCustomGrantTypes]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ClientCustomGrantTypes]
GO
/****** Object:  Table [sec].[ClientCorsOrigins]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ClientCorsOrigins]
GO
/****** Object:  Table [sec].[ClientClaims]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [sec].[ClientClaims]
GO
/****** Object:  Table [dbo].[ServicePeriodStatus]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ServicePeriodStatus]
GO
/****** Object:  Table [dbo].[ServicePeriod]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ServicePeriod]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Role]
GO
/****** Object:  Table [dbo].[RequestLog]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[RequestLog]
GO
/****** Object:  Table [dbo].[RentalApplicationFeature]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[RentalApplicationFeature]
GO
/****** Object:  Table [dbo].[RentalApplicationCity]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[RentalApplicationCity]
GO
/****** Object:  Table [dbo].[InvoiceDetail]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[InvoiceDetail]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Invoice]
GO
/****** Object:  Table [dbo].[IncomeDetail]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[IncomeDetail]
GO
/****** Object:  Table [dbo].[Income]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Income]
GO
/****** Object:  Table [dbo].[FinePayment]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[FinePayment]
GO
/****** Object:  Table [dbo].[Fine]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Fine]
GO
/****** Object:  Table [dbo].[FeatureImage]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[FeatureImage]
GO
/****** Object:  Table [dbo].[FeatureAccesory]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[FeatureAccesory]
GO
/****** Object:  Table [dbo].[ExpenseDetail]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ExpenseDetail]
GO
/****** Object:  Table [dbo].[CostCenter]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[CostCenter]
GO
/****** Object:  Table [dbo].[ContractDetailObligationPay]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ContractDetailObligationPay]
GO
/****** Object:  Table [dbo].[ContractDetailObligation]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ContractDetailObligation]
GO
/****** Object:  Table [dbo].[AuditLog]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AuditLog]
GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AppUser]
GO
/****** Object:  Table [dbo].[AmigoTenantParameter]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AmigoTenantParameter]
GO

/****** Object:  Table [dbo].[ServiceHousePeriod]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ServiceHousePeriod]
GO
/****** Object:  Table [dbo].[RentalApplication]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[RentalApplication]
GO
/****** Object:  Table [dbo].[AppSetting]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AppSetting]
GO
/****** Object:  Table [dbo].[PaymentPeriod]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[PaymentPeriod]
GO
/****** Object:  Table [dbo].[OtherTenant]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[OtherTenant]
GO
/****** Object:  Table [dbo].[LocationCoordinate]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[LocationCoordinate]
GO
/****** Object:  Table [dbo].[LocationType]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[LocationType]
GO
/****** Object:  Table [dbo].[BusinessPartner]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[BusinessPartner]
GO
/****** Object:  Table [dbo].[ServiceHouse]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ServiceHouse]
GO
/****** Object:  Table [dbo].[HouseServicePeriod]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[HouseServicePeriod]
GO
/****** Object:  Table [dbo].[HouseService]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[HouseService]
GO
/****** Object:  Table [dbo].[Equipment]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Equipment]
GO
/****** Object:  Table [dbo].[Brand]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Brand]
GO
/****** Object:  Table [dbo].[Platform]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Platform]
GO
/****** Object:  Table [dbo].[OSVersion]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[OSVersion]
GO
/****** Object:  Table [dbo].[Model]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Model]
GO
/****** Object:  Table [dbo].[ContractDetail]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ContractDetail]
GO
/****** Object:  Table [dbo].[ContractHouseDetail]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ContractHouseDetail]
GO
/****** Object:  Table [dbo].[HouseFeature]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[HouseFeature]
GO
/****** Object:  Table [dbo].[Feature]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Feature]
GO
/****** Object:  Table [dbo].[EntityStatus]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[EntityStatus]
GO
/****** Object:  Table [dbo].[Contract]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Contract]
GO
/****** Object:  Table [dbo].[Tenant]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Tenant]
GO
/****** Object:  Table [dbo].[Period]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Period]
GO
/****** Object:  Table [dbo].[House]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[House]
GO
/****** Object:  Table [dbo].[Concept]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Concept]
GO
/****** Object:  Table [dbo].[GeneralTable]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[GeneralTable]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Country]
GO
/****** Object:  Table [dbo].[City]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[City]
GO
/****** Object:  Table [dbo].[State]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[State]
GO
/****** Object:  Table [dbo].[AppVersion]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AppVersion]
GO
/****** Object:  Table [dbo].[Device]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Device]
GO
/****** Object:  Table [dbo].[AmigoTenantTRole]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AmigoTenantTRole]
GO
/****** Object:  Table [dbo].[DriverReport]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[DriverReport]
GO
/****** Object:  Table [dbo].[AmigoTenantTServiceCharge]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AmigoTenantTServiceCharge]
GO
/****** Object:  Table [dbo].[Rate]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Rate]
GO
/****** Object:  Table [dbo].[EquipmentStatus]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[EquipmentStatus]
GO
/****** Object:  Table [dbo].[EquipmentSize]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[EquipmentSize]
GO
/****** Object:  Table [dbo].[DispatchingParty]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[DispatchingParty]
GO
/****** Object:  Table [dbo].[AmigoTenantTUser]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AmigoTenantTUser]
GO
/****** Object:  Table [dbo].[ServiceType]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ServiceType]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Service]
GO
/****** Object:  Table [dbo].[EquipmentType]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[EquipmentType]
GO
/****** Object:  Table [dbo].[AmigoTenantTService]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AmigoTenantTService]
GO
/****** Object:  Table [dbo].[AmigoTenantTEventLog]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[AmigoTenantTEventLog]
GO
/****** Object:  Table [dbo].[ActivityType]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[ActivityType]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Product]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Location]
GO
/****** Object:  Table [dbo].[Action]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Action]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TABLE [dbo].[Module]
GO
/****** Object:  UserDefinedDataType [dbo].[UserCode]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP TYPE [dbo].[UserCode]
GO
/****** Object:  Schema [sec]    Script Date: 11/20/2017 9:33:34 PM ******/
DROP SCHEMA [sec]
GO
/****** Object:  Schema [sec]    Script Date: 11/20/2017 9:33:34 PM ******/
CREATE SCHEMA [sec]
GO
/****** Object:  UserDefinedDataType [dbo].[UserCode]    Script Date: 11/20/2017 9:33:34 PM ******/
CREATE TYPE [dbo].[UserCode] FROM [varchar](30) NOT NULL
GO
/****** Object:  Table [dbo].[Module]    Script Date: 11/20/2017 9:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[ModuleId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[URL] [varchar](200) NULL,
	[ParentModuleId] [int] NULL,
	[SortOrder] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkModule] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Action]    Script Date: 11/20/2017 9:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Action](
	[ActionId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](200) NULL,
	[Type] [varchar](20) NULL,
	[ModuleId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkAction] PRIMARY KEY CLUSTERED 
(
	[ActionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/****** Object:  Table [dbo].[Location]    Script Date: 11/20/2017 9:33:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[LocationId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[LocationTypeId] [int] NULL,
	[ParentLocationId] [int] NULL,
	[Latitude] [decimal](16, 9) NULL,
	[Longitude] [decimal](16, 9) NULL,
	[Coordinate] [geometry] NULL,
	[Address1] [varchar](100) NULL,
	[Address2] [varchar](100) NULL,
	[ZipCode] [varchar](20) NULL,
	[HasGeofence] [bit] NULL,
	[CityId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkLocation] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/20/2017 9:33:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](255) NULL,
	[ShortName] [varchar](50) NULL,
	[IsHazardous] [varchar](1) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkProduct] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ActivityType]    Script Date: 11/20/2017 9:33:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityType](
	[ActivityTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkActivityType] PRIMARY KEY CLUSTERED 
(
	[ActivityTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AmigoTenantTEventLog]    Script Date: 11/20/2017 9:33:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmigoTenantTEventLog](
	[AmigoTenantTEventLogId] [int] IDENTITY(1,1) NOT NULL,
	[ActivityTypeId] [int] NULL,
	[Username] [varchar](64) NULL,
	[ReportedActivityDate] [datetimeoffset](7) NULL,
	[ReportedActivityTimeZone] [varchar](20) NULL,
	[ConvertedActivityUTC] [datetime2](7) NULL,
	[LogType] [varchar](20) NULL,
	[Parameters] [varchar](4096) NULL,
	[AmigoTenantTServiceId] [int] NULL,
	[EquipmentNumber] [varchar](20) NULL,
	[EquipmentId] [int] NULL,
	[IsAutoDateTime] [bit] NULL,
	[IsSpoofingGPS] [bit] NULL,
	[IsRootedJailbreaked] [bit] NULL,
	[Platform] [varchar](50) NULL,
	[OSVersion] [varchar](50) NULL,
	[AppVersion] [varchar](20) NULL,
	[Latitude] [decimal](16, 9) NULL,
	[Longitude] [decimal](16, 9) NULL,
	[Accuracy] [int] NULL,
	[LocationProvider] [varchar](20) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[ChargeNo] [varchar](20) NULL,
	[ReportedActivityDateLocal]  AS (CONVERT([datetime2],[ReportedActivityDate])),
 CONSTRAINT [pkAmigoTenantTEventLog] PRIMARY KEY CLUSTERED 
(
	[AmigoTenantTEventLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AmigoTenantTService]    Script Date: 11/20/2017 9:33:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmigoTenantTService](
	[AmigoTenantTServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceOrderNo] [uniqueidentifier] NULL,
	[ServiceStartDate] [datetimeoffset](7) NULL,
	[ServiceStartDateTZ] [varchar](20) NULL,
	[ServiceFinishDate] [datetimeoffset](7) NULL,
	[ServiceFinishDateTZ] [varchar](20) NULL,
	[ServiceStartDateUTC] [datetime2](7) NULL,
	[ServiceFinishDateUTC] [datetime2](7) NULL,
	[EquipmentNumber] [varchar](20) NULL,
	[EquipmentTestDate25Year] [datetime2](7) NULL,
	[EquipmentTestDate5Year] [datetime2](7) NULL,
	[ChassisNumber] [varchar](20) NULL,
	[ChargeType] [varchar](1) NULL,
	[PayBy] [varchar](1) NULL,
	[AuthorizationCode] [varchar](20) NULL,
	[HasH34] [bit] NULL,
	[DetentionInMinutesReal] [int] NULL,
	[DetentionInMinutesRounded] [int] NULL,
	[AcknowledgeBy] [varchar](50) NULL,
	[ServiceAcknowledgeDate] [datetimeoffset](7) NULL,
	[ServiceAcknowledgeDateTZ] [varchar](20) NULL,
	[ServiceAcknowledgeDateUTC] [datetime2](7) NULL,
	[IsAknowledged] [bit] NULL,
	[ApprovedBy] [varchar](50) NULL,
	[ApprovalDate] [datetime2](7) NULL,
	[ServiceStatus] [bit] NULL,
	[ApprovalModified] [varchar](1) NULL,
	[OriginLocationId] [int] NULL,
	[DestinationLocationId] [int] NULL,
	[DispatchingPartyId] [int] NULL,
	[EquipmentSizeId] [int] NULL,
	[EquipmentTypeId] [int] NULL,
	[EquipmentStatusId] [int] NULL,
	[ProductId] [int] NULL,
	[ProductDescription] [varchar](200) NULL,
	[ServiceId] [int] NULL,
	[AmigoTenantTUserId] [int] NULL,
	[OriginLocationCode] [varchar](20) NULL,
	[DestinationLocationCode] [varchar](20) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[ApprovalComments] [varchar](max) NULL,
	[ChargeNo] [varchar](20) NULL,
	[DriverComments] [varchar](max) NULL,
	[ServiceStartDateLocal]  AS (CONVERT([datetime2],[servicestartdate])),
 CONSTRAINT [pkAmigoTenantTService] PRIMARY KEY CLUSTERED 
(
	[AmigoTenantTServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/****** Object:  Table [dbo].[EquipmentType]    Script Date: 11/20/2017 9:33:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentType](
	[EquipmentTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[ProductRequiredCode] [char](1) NULL,
	[ChassisRequiredCode] [char](1) NULL,
	[EquipmentNumberRequiredCode] [char](1) NULL,
 CONSTRAINT [pkEquipmentType] PRIMARY KEY CLUSTERED 
(
	[EquipmentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Service]    Script Date: 11/20/2017 9:33:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[IsPerMove] [varchar](1) NULL,
	[IsPerHour] [varchar](1) NULL,
	[ServiceTypeId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[BlockRequiredCode] [char](1) NULL,
	[ProductRequiredCode] [char](1) NULL,
	[EquipmentRequiredCode] [char](1) NULL,
	[ChassisRequiredCode] [char](1) NULL,
	[DispatchingPartyRequiredCode] [char](1) NULL,
	[EquipmentStatusRequiredCode] [char](1) NULL,
	[BobtailAuthRequiredCode] [char](1) NULL,
	[HasH34RequiredCode] [char](1) NULL,
 CONSTRAINT [pkService] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ServiceType]    Script Date: 11/20/2017 9:33:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceType](
	[ServiceTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkServiceType] PRIMARY KEY CLUSTERED 
(
	[ServiceTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AmigoTenantTUser]    Script Date: 11/20/2017 9:33:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmigoTenantTUser](
	[AmigoTenantTUserId] [int] NOT NULL,
	[Username] [varchar](64) NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[AmigoTenantTRoleId] [int] NULL,
	[PayBy] [varchar](1) NULL,
	[UserType] [varchar](1) NULL,
	[DedicatedLocationId] [int] NULL,
	[BypassDeviceValidation] [bit] NULL,
	[UnitNumber] [varchar](20) NULL,
	[TractorNumber] [varchar](20) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkAmigoTenantTUser] PRIMARY KEY CLUSTERED 
(
	[AmigoTenantTUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[DispatchingParty]    Script Date: 11/20/2017 9:33:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DispatchingParty](
	[DispatchingPartyId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkDispatchingParty] PRIMARY KEY CLUSTERED 
(
	[DispatchingPartyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[EquipmentSize]    Script Date: 11/20/2017 9:33:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentSize](
	[EquipmentSizeId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[EquipmentTypeId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkEquipmentSize] PRIMARY KEY CLUSTERED 
(
	[EquipmentSizeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[EquipmentStatus]    Script Date: 11/20/2017 9:33:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentStatus](
	[EquipmentStatusId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkEquipmentStatus] PRIMARY KEY CLUSTERED 
(
	[EquipmentStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/****** Object:  Table [dbo].[Rate]    Script Date: 11/20/2017 9:33:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rate](
	[RateId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](200) NULL,
	[PaidBy] [varchar](1) NULL,
	[ServiceId] [int] NULL,
	[BillCustomer] [decimal](16, 9) NULL,
	[PayDriver] [decimal](16, 9) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkRate] PRIMARY KEY CLUSTERED 
(
	[RateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/****** Object:  Table [dbo].[AmigoTenantTServiceCharge]    Script Date: 11/20/2017 9:33:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmigoTenantTServiceCharge](
	[AmigoTenantTServiceChargeId] [int] IDENTITY(1,1) NOT NULL,
	[DriverPay] [decimal](16, 9) NULL,
	[CustomerBill] [decimal](16, 9) NULL,
	[AmigoTenantTServiceId] [int] NULL,
	[RateId] [int] NULL,
	[DriverReportId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkAmigoTenantTServiceCharge] PRIMARY KEY CLUSTERED 
(
	[AmigoTenantTServiceChargeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[DriverReport]    Script Date: 11/20/2017 9:33:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DriverReport](
	[DriverReportId] [int] IDENTITY(1,1) NOT NULL,
	[ReportDate] [datetime2](7) NULL,
	[Year] [int] NULL,
	[WeekNumber] [int] NULL,
	[DriverUserId] [int] NULL,
	[ApproverUserId] [int] NULL,
	[ApproverSignature] [varchar](50) NULL,
	[DayPayDriverTotal] [decimal](16, 9) NULL,
	[DayBillCustomerTotal] [decimal](16, 9) NULL,
	[TotalHours] [decimal](5, 2) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[StartTime] [datetime2](7) NULL,
	[FinishTime] [datetime2](7) NULL,
 CONSTRAINT [pkDriverReport] PRIMARY KEY CLUSTERED 
(
	[DriverReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/****** Object:  Table [dbo].[AmigoTenantTRole]    Script Date: 11/20/2017 9:33:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmigoTenantTRole](
	[AmigoTenantTRoleId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[IsAdmin] [bit] NOT NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkAmigoTenantTRole] PRIMARY KEY CLUSTERED 
(
	[AmigoTenantTRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Device]    Script Date: 11/20/2017 9:33:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Device](
	[DeviceId] [int] IDENTITY(1,1) NOT NULL,
	[CellphoneNumber] [varchar](20) NULL,
	[Identifier] [varchar](100) NULL,
	[WIFIMAC] [varchar](100) NULL,
	[OSVersionId] [int] NULL,
	[ModelId] [int] NULL,
	[IsAutoDateTime] [bit] NULL,
	[IsSpoofingGPS] [bit] NULL,
	[IsRootedJailbreaked] [bit] NULL,
	[AppVersionId] [int] NULL,
	[AssignedAmigoTenantTUserId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkDevice] PRIMARY KEY CLUSTERED 
(
	[DeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AppVersion]    Script Date: 11/20/2017 9:33:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppVersion](
	[AppVersionId] [int] IDENTITY(1,1) NOT NULL,
	[Version] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[ReleaseDate] [datetime] NULL,
	[ReleaseNotes] [varchar](4096) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkAppVersion] PRIMARY KEY CLUSTERED 
(
	[AppVersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)


/****** Object:  Table [dbo].[State]    Script Date: 11/20/2017 9:33:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[State](
	[StateId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[CountryId] [int] NOT NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkState] PRIMARY KEY CLUSTERED 
(
	[StateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[City]    Script Date: 11/20/2017 9:33:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[CityId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[StateId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkCity] PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Country]    Script Date: 11/20/2017 9:33:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[ISOCode] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkCountry] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/****** Object:  Table [dbo].[GeneralTable]    Script Date: 11/20/2017 9:33:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GeneralTable](
	[GeneralTableId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](10) NULL,
	[TableName] [varchar](30) NOT NULL,
	[Value] [varchar](50) NOT NULL,
	[Sequence] [int] NOT NULL,
	[ByDefault] [bit] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKGeneralTable] PRIMARY KEY CLUSTERED 
(
	[GeneralTableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Concept]    Script Date: 11/20/2017 9:33:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Concept](
	[ConceptId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](12) NULL,
	[Description] [varchar](200) NULL,
	[TypeId] [int] NOT NULL,
	[RowStatus] [bit] NULL,
	[Remark] [varchar](500) NULL,
	[PayTypeId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[ConceptAmount] [decimal](8, 2) NULL,
 CONSTRAINT [XPKConcept] PRIMARY KEY CLUSTERED 
(
	[ConceptId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

/****** Object:  Table [dbo].[House]    Script Date: 11/20/2017 9:33:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[House](
	[HouseId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](8) NOT NULL,
	[Name] [varchar](100) NULL,
	[ShortName] [varchar](100) NULL,
	[LocationId] [int] NOT NULL,
	[Address] [varchar](150) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[RentPrice] [decimal](8, 2) NOT NULL,
	[RentDeposit] [decimal](8, 2) NOT NULL,
	[HouseTypeId] [int] NOT NULL,
	[HouseStatusId] [int] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Latitude] [decimal](18, 15) NULL,
	[Longitude] [decimal](18, 15) NULL,
	[CityId] [int] NULL,
 CONSTRAINT [XPKHouse] PRIMARY KEY CLUSTERED 
(
	[HouseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Period]    Script Date: 11/20/2017 9:33:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Period](
	[PeriodId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](6) NULL,
	[BeginDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[Sequence] [int] NULL,
 CONSTRAINT [XPKPeriod] PRIMARY KEY CLUSTERED 
(
	[PeriodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Tenant]    Script Date: 11/20/2017 9:33:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenant](
	[TenantId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](6) NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[CountryId] [int] NOT NULL,
	[PassportNo] [varchar](50) NULL,
	[PhoneN01] [varchar](20) NULL,
	[Email] [varchar](50) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[TypeId] [int] NULL,
	[Address] [varchar](250) NULL,
	[Reference] [varchar](250) NULL,
	[PhoneNo2] [varchar](20) NULL,
	[ContactName] [varchar](50) NULL,
	[ContactPhone] [varchar](20) NULL,
	[ContactEmail] [varchar](50) NULL,
	[ContactRelation] [varchar](50) NULL,
	[IdRef] [varchar](30) NULL,
 CONSTRAINT [XPKCustomer] PRIMARY KEY CLUSTERED 
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Contract]    Script Date: 11/20/2017 9:33:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contract](
	[ContractId] [int] IDENTITY(1,1) NOT NULL,
	[BeginDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NULL,
	[RentDeposit] [decimal](8, 2) NOT NULL,
	[RentPrice] [decimal](8, 2) NOT NULL,
	[ContractDate] [datetime2](7) NOT NULL,
	[PaymentModeId] [int] NOT NULL,
	[ContractStatusId] [int] NOT NULL,
	[PeriodId] [int] NOT NULL,
	[ContractCode] [varchar](11) NULL,
	[ReferencedBy] [varchar](30) NULL,
	[HouseId] [int] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[FrecuencyTypeId] [int] NULL,
	[TenantId] [int] NULL,
 CONSTRAINT [XPKContract] PRIMARY KEY CLUSTERED 
(
	[ContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[EntityStatus]    Script Date: 11/20/2017 9:33:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityStatus](
	[EntityStatusId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](10) NULL,
	[Name] [varchar](100) NOT NULL,
	[EntityCode] [varchar](3) NOT NULL,
	[Sequence] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKEntityStatus] PRIMARY KEY CLUSTERED 
(
	[EntityStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Feature]    Script Date: 11/20/2017 9:33:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feature](
	[FeatureId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Description] [varchar](150) NULL,
	[Measure] [decimal](8, 2) NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Sequence] [int] NULL,
	[IsAllHouse] [bit] NULL,
	[HouseTypeId] [int] NULL,
 CONSTRAINT [XPKFeature] PRIMARY KEY CLUSTERED 
(
	[FeatureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[HouseFeature]    Script Date: 11/20/2017 9:33:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HouseFeature](
	[HouseFeatureId] [int] IDENTITY(1,1) NOT NULL,
	[HouseId] [int] NOT NULL,
	[FeatureId] [int] NOT NULL,
	[HouseFeatureStatusId] [int] NOT NULL,
	[IsRentable] [bit] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[AdditionalAddressInfo] [varchar](50) NULL,
	[RentPrice] [decimal](8, 2) NULL,
 CONSTRAINT [XPKHouseFeature] PRIMARY KEY CLUSTERED 
(
	[HouseFeatureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ContractHouseDetail]    Script Date: 11/20/2017 9:33:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractHouseDetail](
	[ContractHouseDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NOT NULL,
	[HouseFeatureId] [int] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKContractHouseDetail] PRIMARY KEY CLUSTERED 
(
	[ContractHouseDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ContractDetail]    Script Date: 11/20/2017 9:33:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractDetail](
	[ContractDetailId] [int] IDENTITY(1,1) NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[ItemNo] [int] NOT NULL,
	[Description] [varchar](100) NOT NULL,
	[Comment] [varchar](100) NULL,
	[Rent] [decimal](8, 2) NOT NULL,
	[ContractId] [int] NOT NULL,
	[ContractDetailStatusId] [int] NOT NULL,
	[PaymentDate] [datetime2](7) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[PeriodId] [int] NULL,
	[DelayDays] [int] NULL,
	[FinePerDay] [decimal](8, 2) NULL,
	[FineAmount] [decimal](8, 2) NULL,
	[TotalPayment] [decimal](8, 2) NULL,
	[PayTypeId] [int] NULL,
	[PaymentReferenceNo] [varchar](50) NULL,
 CONSTRAINT [XPKContractDetail] PRIMARY KEY CLUSTERED 
(
	[ContractDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Model]    Script Date: 11/20/2017 9:33:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Model](
	[ModelId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[BrandId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkModel] PRIMARY KEY CLUSTERED 
(
	[ModelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[OSVersion]    Script Date: 11/20/2017 9:33:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OSVersion](
	[OSVersionId] [int] IDENTITY(1,1) NOT NULL,
	[Version] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[PlatformId] [int] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkOSVersion] PRIMARY KEY CLUSTERED 
(
	[OSVersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Platform]    Script Date: 11/20/2017 9:34:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Platform](
	[PlatformId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkPlatform] PRIMARY KEY CLUSTERED 
(
	[PlatformId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Brand]    Script Date: 11/20/2017 9:34:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[BrandId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkBrand] PRIMARY KEY CLUSTERED 
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Equipment]    Script Date: 11/20/2017 9:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipment](
	[EquipmentId] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentNo] [varchar](20) NULL,
	[TestDate25Year] [datetime] NULL,
	[TestDate5Year] [datetime] NULL,
	[EquipmentSizeId] [int] NULL,
	[EquipmentStatusId] [int] NULL,
	[LocationId] [int] NULL,
	[IsMasterRecord] [bit] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkEquipment] PRIMARY KEY CLUSTERED 
(
	[EquipmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[HouseService]    Script Date: 11/20/2017 9:34:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HouseService](
	[HouseServiceId] [int] IDENTITY(1,1) NOT NULL,
	[HouseId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkHouseService] PRIMARY KEY CLUSTERED 
(
	[HouseServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[HouseServicePeriod]    Script Date: 11/20/2017 9:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HouseServicePeriod](
	[HouseServicePeriodId] [int] IDENTITY(1,1) NOT NULL,
	[HouseServiceId] [int] NOT NULL,
	[MonthId] [int] NOT NULL,
	[DueDateMonth] [int] NOT NULL,
	[DueDateDay] [int] NOT NULL,
	[CutOffMonth] [int] NOT NULL,
	[CutOffDay] [int] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkHouseServicePeriod] PRIMARY KEY CLUSTERED 
(
	[HouseServicePeriodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ServiceHouse]    Script Date: 11/20/2017 9:34:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceHouse](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[ConceptId] [int] NULL,
	[BusinessPartnerId] [int] NULL,
	[ServiceTypeId] [int] NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKService] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[BusinessPartner]    Script Date: 11/20/2017 9:34:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessPartner](
	[BusinessPartnerId] [int] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](100) NULL,
	[RUC] [varchar](20) NULL,
	[BirthDate] [datetime2](7) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[Remark] [varchar](200) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[Email] [varchar](30) NULL,
	[CountryId] [int] NOT NULL,
	[EntityStatusId] [int] NULL,
	[CityId] [int] NULL,
 CONSTRAINT [XPKRentalOwner] PRIMARY KEY CLUSTERED 
(
	[BusinessPartnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[LocationType]    Script Date: 11/20/2017 9:34:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationType](
	[LocationTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkLocationType] PRIMARY KEY CLUSTERED 
(
	[LocationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[LocationCoordinate]    Script Date: 11/20/2017 9:34:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationCoordinate](
	[LocationCoordinateId] [int] IDENTITY(1,1) NOT NULL,
	[Latitude] [decimal](16, 9) NULL,
	[Longitude] [decimal](16, 9) NULL,
	[Coordinate] [geometry] NULL,
	[LocationId] [int] NULL,
 CONSTRAINT [pkLocationCoordinate] PRIMARY KEY CLUSTERED 
(
	[LocationCoordinateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[OtherTenant]    Script Date: 11/20/2017 9:34:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OtherTenant](
	[OtherTenantId] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NULL,
	[TenantId] [int] NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKContractTenant] PRIMARY KEY CLUSTERED 
(
	[OtherTenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[PaymentPeriod]    Script Date: 11/20/2017 9:34:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentPeriod](
	[PaymentPeriodId] [int] IDENTITY(1,1) NOT NULL,
	[ConceptId] [int] NULL,
	[ContractId] [int] NULL,
	[TenantId] [int] NULL,
	[PeriodId] [int] NULL,
	[PaymentAmount] [decimal](8, 2) NULL,
	[DueDate] [datetime2](7) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[PaymentPeriodStatusId] [int] NULL,
	[PaymentTypeId] [int] NULL,
	[Comment] [varchar](250) NULL,
	[ReferenceNo] [varchar](80) NULL,
	[PaymentDate] [datetime2](7) NULL,
 CONSTRAINT [XPKPaymentPeriod] PRIMARY KEY CLUSTERED 
(
	[PaymentPeriodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AppSetting]    Script Date: 11/20/2017 9:34:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppSetting](
	[AppSettingId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](10) NULL,
	[Name] [varchar](50) NULL,
	[AppSettingValue] [varchar](20) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKAppSetting] PRIMARY KEY CLUSTERED 
(
	[AppSettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[RentalApplication]    Script Date: 11/20/2017 9:34:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentalApplication](
	[RentalApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[PeriodId] [int] NULL,
	[PropertyTypeId] [int] NULL,
	[ApplicationDate] [datetime2](7) NOT NULL,
	[FullName] [varchar](80) NULL,
	[Email] [varchar](100) NULL,
	[HousePhone] [varchar](30) NULL,
	[CellPhone] [varchar](30) NULL,
	[CheckIn] [datetime2](7) NULL,
	[CheckOut] [datetime2](7) NULL,
	[ResidenseCountryId] [int] NULL,
	[BudgetId] [int] NULL,
	[Comment] [varchar](500) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[CityOfInterestId] [int] NULL,
	[HousePartId] [int] NULL,
	[PersonNo] [int] NULL,
	[OutInDownId] [int] NULL,
	[ReferredById] [int] NULL,
	[ReferredByOther] [varchar](50) NULL,
	[AlertBeforeThat] [int] NULL,
 CONSTRAINT [XPKRentalApplication] PRIMARY KEY CLUSTERED 
(
	[RentalApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
GO
/****** Object:  Table [dbo].[ServiceHousePeriod]    Script Date: 11/20/2017 9:34:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceHousePeriod](
	[ServiceHousePeriodId] [int] IDENTITY(1,1) NOT NULL,
	[MonthId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[DueDateMonth] [int] NOT NULL,
	[DueDateDay] [int] NOT NULL,
	[CutOffMonth] [int] NOT NULL,
	[CutOffDay] [int] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkServiceHousePeriod] PRIMARY KEY CLUSTERED 
(
	[ServiceHousePeriodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AmigoTenantParameter]    Script Date: 11/20/2017 9:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmigoTenantParameter](
	[AmigoTenantParameterId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Code] [varchar](20) NULL,
	[Value] [varchar](50) NULL,
	[Description] [varchar](200) NULL,
	[IsForMobile] [varchar](1) NULL,
	[IsForWeb] [varchar](1) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkAmigoTenantParameter] PRIMARY KEY CLUSTERED 
(
	[AmigoTenantParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 11/20/2017 9:34:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser](
	[AppUserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [dbo].[UserCode] NOT NULL,
	[Password] [varchar](512) NOT NULL,
	[FirstName] [varchar](20) NOT NULL,
	[LastName] [varchar](20) NOT NULL,
	[BirthDate] [date] NULL,
	[CargoId] [int] NULL,
	[ExternalInternal] [varchar](1) NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[RoleId] [int] NULL,
	[CreatedBy] [dbo].[UserCode] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [dbo].[UserCode] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKAppUser] PRIMARY KEY CLUSTERED 
(
	[AppUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[AuditLog]    Script Date: 11/20/2017 9:34:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Command] [nvarchar](1000) NULL,
	[PostTime] [nvarchar](24) NULL,
	[HostName] [nvarchar](100) NULL,
	[LoginName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ContractDetailObligation]    Script Date: 11/20/2017 9:34:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractDetailObligation](
	[ContractDetailObligationId] [int] IDENTITY(1,1) NOT NULL,
	[ContractDetailId] [int] NULL,
	[ObligationDate] [datetime2](7) NULL,
	[ConceptId] [int] NULL,
	[Comment] [varchar](150) NULL,
	[InfractionAmount] [decimal](8, 2) NULL,
	[TenantId] [int] NULL,
	[PeriodId] [int] NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[TenantInfractorId] [int] NULL,
	[EntityStatusId] [int] NULL,
 CONSTRAINT [XPKInfraction] PRIMARY KEY CLUSTERED 
(
	[ContractDetailObligationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ContractDetailObligationPay]    Script Date: 11/20/2017 9:34:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractDetailObligationPay](
	[ContractDetailInfractionPayId] [int] IDENTITY(1,1) NOT NULL,
	[ItemNo] [int] NULL,
	[ContractDetailObligationId] [int] NULL,
	[PayAmount] [decimal](8, 2) NULL,
	[PayDate] [datetime2](7) NULL,
	[TenantId] [int] NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKContractDetailInfraction] PRIMARY KEY CLUSTERED 
(
	[ContractDetailInfractionPayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[CostCenter]    Script Date: 11/20/2017 9:34:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CostCenter](
	[CostCenterId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NULL,
	[Name] [varchar](50) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [pkCostCenter] PRIMARY KEY CLUSTERED 
(
	[CostCenterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ExpenseDetail]    Script Date: 11/20/2017 9:34:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseDetail](
	[ExpenseDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ConceptId] [int] NOT NULL,
	[ExpenseDate] [datetime2](7) NULL,
	[Remark] [varchar](250) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[ContractDetailId] [int] NULL,
	[PeriodId] [int] NULL,
 CONSTRAINT [XPKExpenseDetail] PRIMARY KEY CLUSTERED 
(
	[ExpenseDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[FeatureAccesory]    Script Date: 11/20/2017 9:34:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeatureAccesory](
	[FeatureAccesoryId] [int] IDENTITY(1,1) NOT NULL,
	[AccesoryId] [int] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[HouseFeatureId] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKFeatureAccesory] PRIMARY KEY CLUSTERED 
(
	[FeatureAccesoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[FeatureImage]    Script Date: 11/20/2017 9:34:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeatureImage](
	[FeatureImageId] [int] IDENTITY(1,1) NOT NULL,
	[HouseFeatureId] [int] NOT NULL,
	[ImagePath] [varchar](250) NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKFeatureImage] PRIMARY KEY CLUSTERED 
(
	[FeatureImageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Fine]    Script Date: 11/20/2017 9:34:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fine](
	[FineId] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NULL,
	[FineDate] [datetime2](7) NULL,
	[PeriodId] [int] NULL,
	[ConceptId] [int] NULL,
	[Comment] [varchar](150) NULL,
	[FineAmount] [decimal](8, 2) NULL,
	[BalanceAmount] [decimal](8, 2) NULL,
	[TenantId] [int] NULL,
	[TenantOfenderName] [char](18) NULL,
	[EntityStatusId] [int] NULL,
	[DueDate] [datetime2](7) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKFine] PRIMARY KEY CLUSTERED 
(
	[FineId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[FinePayment]    Script Date: 11/20/2017 9:34:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinePayment](
	[FinePaymentId] [int] IDENTITY(1,1) NOT NULL,
	[ItemNo] [int] NULL,
	[FineId] [int] NULL,
	[PaymentAmount] [decimal](8, 2) NULL,
	[PaymentDate] [datetime2](7) NULL,
 CONSTRAINT [XPKFinePayment] PRIMARY KEY CLUSTERED 
(
	[FinePaymentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Income]    Script Date: 11/20/2017 9:34:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Income](
	[IncomeId] [int] IDENTITY(1,1) NOT NULL,
	[IncomeDate] [datetime2](7) NULL,
	[InvoiceNo] [varchar](20) NULL,
	[Remark] [varchar](200) NULL,
	[ContractId] [int] NULL,
	[TotalAmount] [decimal](8, 2) NULL,
	[Tax] [decimal](8, 2) NULL,
	[SubTotalAmount] [decimal](8, 2) NULL,
	[TenantId] [int] NULL,
	[EntityStatusId] [int] NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[PeriodId] [int] NULL,
 CONSTRAINT [XPKContractAdditionalCharges] PRIMARY KEY CLUSTERED 
(
	[IncomeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[IncomeDetail]    Script Date: 11/20/2017 9:34:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IncomeDetail](
	[IncomeDetailId] [int] IDENTITY(1,1) NOT NULL,
	[IncomeId] [int] NULL,
	[ContractDetailId] [int] NULL,
	[ConceptId] [int] NULL,
	[Description] [varchar](200) NULL,
	[Qty] [int] NULL,
	[TotalAmount] [decimal](8, 2) NULL,
	[ItemNo] [int] NULL,
	[UnitPrice] [decimal](8, 2) NULL,
	[ContractDetailObligationId] [int] NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKIncomeDetail] PRIMARY KEY CLUSTERED 
(
	[IncomeDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 11/20/2017 9:34:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NULL,
	[InvoiceDate] [datetime2](7) NULL,
	[TotalAmount] [decimal](8, 2) NULL,
	[Comment] [varchar](500) NULL,
	[PaymentTypeId] [int] NULL,
	[InvoiceNo] [varchar](10) NULL,
	[Taxes] [decimal](6, 2) NULL,
	[BusinessPartnerId] [int] NULL,
	[CustomerName] [varchar](100) NULL,
	[PaymentOperationNo] [varchar](20) NULL,
	[BankName] [varchar](50) NULL,
	[PaymentOperationDate] [datetime2](7) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[InvoiceStatusId] [int] NULL,
 CONSTRAINT [XPKInvoice] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[InvoiceDetail]    Script Date: 11/20/2017 9:34:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceDetail](
	[InvoiceDetailId] [char](18) NOT NULL,
	[InvoiceId] [int] NULL,
	[PaymentPeriodId] [int] NULL,
	[ConceptId] [int] NULL,
	[Qty] [decimal](6, 2) NULL,
	[UnitPrice] [decimal](8, 2) NULL,
	[TotalAmount] [decimal](8, 2) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKInvoiceDetail] PRIMARY KEY CLUSTERED 
(
	[InvoiceDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[RentalApplicationCity]    Script Date: 11/20/2017 9:34:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentalApplicationCity](
	[RentalApplicationCityId] [int] IDENTITY(1,1) NOT NULL,
	[RentalApplicationId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
 CONSTRAINT [XPKRentalApplicationCity] PRIMARY KEY CLUSTERED 
(
	[RentalApplicationCityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[RentalApplicationFeature]    Script Date: 11/20/2017 9:34:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RentalApplicationFeature](
	[RentalApplicationFeatureId] [int] IDENTITY(1,1) NOT NULL,
	[RentalApplicationId] [int] NOT NULL,
	[FeatureId] [int] NOT NULL,
 CONSTRAINT [XPKRentalApplicationFeature] PRIMARY KEY CLUSTERED 
(
	[RentalApplicationFeatureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[RequestLog]    Script Date: 11/20/2017 9:34:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestLog](
	[RequestLogId] [int] IDENTITY(1,1) NOT NULL,
	[URL] [varchar](200) NULL,
	[ServiceName] [varchar](200) NULL,
	[Request] [xml] NULL,
	[Response] [xml] NULL,
	[RequestedBy] [int] NULL,
	[RequestDate] [datetime2](7) NULL,
 CONSTRAINT [pkRequestLog] PRIMARY KEY CLUSTERED 
(
	[RequestLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Role]    Script Date: 11/20/2017 9:34:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](20) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [dbo].[UserCode] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [dbo].[UserCode] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKRole] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ServicePeriod]    Script Date: 11/20/2017 9:34:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServicePeriod](
	[ServicePeriodId] [int] IDENTITY(1,1) NOT NULL,
	[HouseServicePeriodId] [int] NULL,
	[PeriodId] [int] NULL,
	[CompanyId] [int] NULL,
	[Amount] [decimal](8, 2) NULL,
	[Adjust] [decimal](8, 2) NULL,
	[UM] [int] NULL,
	[Consumption] [decimal](8, 2) NULL,
	[ServicePeriodStatusId] [int] NULL,
	[ServiceStatusId] [int] NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKServicePeriod] PRIMARY KEY CLUSTERED 
(
	[ServicePeriodId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ServicePeriodStatus]    Script Date: 11/20/2017 9:34:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServicePeriodStatus](
	[ServicePeriodStatusId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceStatusId] [int] NULL,
	[ExecutionDate] [datetime2](7) NULL,
	[Comment] [varchar](255) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKEntityStatusDetail] PRIMARY KEY CLUSTERED 
(
	[ServicePeriodStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ClientClaims]    Script Date: 11/20/2017 9:34:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ClientClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](250) NOT NULL,
	[Value] [varchar](250) NOT NULL,
	[Client_Id] [int] NOT NULL,
 CONSTRAINT [pkClientClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ClientCorsOrigins]    Script Date: 11/20/2017 9:34:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ClientCorsOrigins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Origin] [varchar](150) NOT NULL,
	[Client_Id] [int] NOT NULL,
 CONSTRAINT [pkClientCorsOrigins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ClientCustomGrantTypes]    Script Date: 11/20/2017 9:34:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ClientCustomGrantTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GrantType] [varchar](250) NOT NULL,
	[Client_Id] [int] NOT NULL,
 CONSTRAINT [pkClientCustomGrantTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ClientIdPRestrictions]    Script Date: 11/20/2017 9:34:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ClientIdPRestrictions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Provider] [varchar](200) NOT NULL,
	[Client_Id] [int] NOT NULL,
 CONSTRAINT [pkClientIdPRestrictions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ClientPostLogoutRedirectUris]    Script Date: 11/20/2017 9:34:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ClientPostLogoutRedirectUris](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uri] [varchar](2000) NOT NULL,
	[Client_Id] [int] NOT NULL,
 CONSTRAINT [pkClientPostLogoutRedirectUris] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ClientRedirectUris]    Script Date: 11/20/2017 9:34:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ClientRedirectUris](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uri] [varchar](2000) NOT NULL,
	[Client_Id] [int] NOT NULL,
 CONSTRAINT [pkClientRedirectUris] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[Clients]    Script Date: 11/20/2017 9:34:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[ClientId] [varchar](200) NOT NULL,
	[ClientName] [varchar](200) NOT NULL,
	[ClientUri] [varchar](2000) NULL,
	[LogoUri] [varchar](max) NULL,
	[RequireConsent] [bit] NOT NULL,
	[AllowRememberConsent] [bit] NOT NULL,
	[AllowAccessTokensViaBrowser] [bit] NOT NULL,
	[Flow] [int] NOT NULL,
	[AllowClientCredentialsOnly] [bit] NOT NULL,
	[LogoutUri] [varchar](500) NULL,
	[LogoutSessionRequired] [bit] NOT NULL,
	[RequireSignOutPrompt] [bit] NOT NULL,
	[AllowAccessToAllScopes] [bit] NOT NULL,
	[IdentityTokenLifetime] [int] NOT NULL,
	[AccessTokenLifetime] [int] NOT NULL,
	[AuthorizationCodeLifetime] [int] NOT NULL,
	[AbsoluteRefreshTokenLifetime] [int] NOT NULL,
	[SlidingRefreshTokenLifetime] [int] NOT NULL,
	[RefreshTokenUsage] [int] NOT NULL,
	[UpdateAccessTokenOnRefresh] [bit] NOT NULL,
	[RefreshTokenExpiration] [int] NOT NULL,
	[AccessTokenType] [int] NOT NULL,
	[EnableLocalLogin] [bit] NOT NULL,
	[IncludeJwtId] [bit] NOT NULL,
	[AlwaysSendClientClaims] [bit] NOT NULL,
	[PrefixClientClaims] [bit] NOT NULL,
	[AllowAccessToAllGrantTypes] [bit] NOT NULL,
 CONSTRAINT [pkClients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ClientScopes]    Script Date: 11/20/2017 9:34:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ClientScopes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Scope] [varchar](200) NOT NULL,
	[Client_Id] [int] NOT NULL,
 CONSTRAINT [pkClientScopes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ClientSecrets]    Script Date: 11/20/2017 9:34:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ClientSecrets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](250) NOT NULL,
	[Type] [varchar](250) NULL,
	[Description] [varchar](2000) NULL,
	[Expiration] [datetimeoffset](7) NULL,
	[Client_Id] [int] NOT NULL,
 CONSTRAINT [pkClientSecrets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[Consents]    Script Date: 11/20/2017 9:34:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Consents](
	[Subject] [varchar](200) NOT NULL,
	[ClientId] [varchar](200) NOT NULL,
	[Scopes] [varchar](2000) NOT NULL,
 CONSTRAINT [pkConsents] PRIMARY KEY CLUSTERED 
(
	[Subject] ASC,
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[Role]    Script Date: 11/20/2017 9:34:32 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
 CONSTRAINT [pkRole] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ScopeClaims]    Script Date: 11/20/2017 9:34:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ScopeClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](1000) NULL,
	[AlwaysIncludeInIdToken] [bit] NOT NULL,
	[Scope_Id] [int] NOT NULL,
 CONSTRAINT [pkScopeClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[Scopes]    Script Date: 11/20/2017 9:34:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Scopes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[DisplayName] [varchar](200) NULL,
	[Description] [varchar](1000) NULL,
	[Required] [bit] NOT NULL,
	[Emphasize] [bit] NOT NULL,
	[Type] [int] NOT NULL,
	[IncludeAllClaimsForUser] [bit] NOT NULL,
	[ClaimsRule] [varchar](200) NULL,
	[ShowInDiscoveryDocument] [bit] NOT NULL,
	[AllowUnrestrictedIntrospection] [bit] NOT NULL,
 CONSTRAINT [pkScopes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[ScopeSecrets]    Script Date: 11/20/2017 9:34:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[ScopeSecrets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](1000) NULL,
	[Expiration] [datetimeoffset](7) NULL,
	[Type] [varchar](250) NULL,
	[Value] [varchar](250) NOT NULL,
	[Scope_Id] [int] NOT NULL,
 CONSTRAINT [pkScopeSecrets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[Tokens]    Script Date: 11/20/2017 9:34:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[Tokens](
	[Key] [varchar](128) NOT NULL,
	[TokenType] [smallint] NOT NULL,
	[SubjectId] [varchar](200) NULL,
	[ClientId] [varchar](200) NOT NULL,
	[JsonCode] [varchar](2000) NOT NULL,
	[Expiry] [datetimeoffset](7) NOT NULL,
 CONSTRAINT [pkTokens] PRIMARY KEY CLUSTERED 
(
	[Key] ASC,
	[TokenType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[User]    Script Date: 11/20/2017 9:34:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](64) NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[ProfilePictureUrl] [varchar](200) NULL,
	[Email] [varchar](100) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [varchar](100) NULL,
	[SecurityStamp] [varchar](100) NULL,
	[PhoneNumber] [varchar](20) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[RowStatus] [bit] NULL,
 CONSTRAINT [pkUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[UserClaim]    Script Date: 11/20/2017 9:34:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[UserClaim](
	[UserClaimId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [varchar](200) NULL,
	[ClaimValue] [varchar](200) NULL,
 CONSTRAINT [pkUserClaim] PRIMARY KEY CLUSTERED 
(
	[UserClaimId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[UserLogin]    Script Date: 11/20/2017 9:34:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[UserLogin](
	[LoginProvider] [varchar](128) NOT NULL,
	[ProviderKey] [varchar](128) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [pkUserLogin] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [sec].[UserRole]    Script Date: 11/20/2017 9:34:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [sec].[UserRole](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [pkUserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

---------------
--ALTER TABLES
---------------


ALTER TABLE [dbo].[Action]  WITH CHECK ADD  CONSTRAINT [fkModule_Action] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
GO
ALTER TABLE [dbo].[Action] CHECK CONSTRAINT [fkModule_Action]
GO
ALTER TABLE [dbo].[Action]  WITH CHECK ADD  CONSTRAINT [R_12] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
GO
ALTER TABLE [dbo].[Action] CHECK CONSTRAINT [R_12]
GO
ALTER TABLE [dbo].[AmigoTenantTEventLog]  WITH CHECK ADD  CONSTRAINT [fkActivityType_AmigoTenantTEventLog] FOREIGN KEY([ActivityTypeId])
REFERENCES [dbo].[ActivityType] ([ActivityTypeId])
GO
ALTER TABLE [dbo].[AmigoTenantTEventLog] CHECK CONSTRAINT [fkActivityType_AmigoTenantTEventLog]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkAmigoTenantTUser_AmigoTenantTService] FOREIGN KEY([AmigoTenantTUserId])
REFERENCES [dbo].[AmigoTenantTUser] ([AmigoTenantTUserId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkAmigoTenantTUser_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkDispatchingParty_AmigoTenantTService] FOREIGN KEY([DispatchingPartyId])
REFERENCES [dbo].[DispatchingParty] ([DispatchingPartyId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkDispatchingParty_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkEquipmentSize_AmigoTenantTService] FOREIGN KEY([EquipmentSizeId])
REFERENCES [dbo].[EquipmentSize] ([EquipmentSizeId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkEquipmentSize_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkEquipmentStatus_AmigoTenantTService] FOREIGN KEY([EquipmentStatusId])
REFERENCES [dbo].[EquipmentStatus] ([EquipmentStatusId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkEquipmentStatus_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkEquipmentType_AmigoTenantTService] FOREIGN KEY([EquipmentTypeId])
REFERENCES [dbo].[EquipmentType] ([EquipmentTypeId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkEquipmentType_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkLocation_AmigoTenantMoveDestinationLocationId] FOREIGN KEY([DestinationLocationId])
REFERENCES [dbo].[Location] ([LocationId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkLocation_AmigoTenantMoveDestinationLocationId]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkLocation_AmigoTenantMoveOriginLocationId] FOREIGN KEY([OriginLocationId])
REFERENCES [dbo].[Location] ([LocationId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkLocation_AmigoTenantMoveOriginLocationId]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkProduct_AmigoTenantTService] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkProduct_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTService]  WITH CHECK ADD  CONSTRAINT [fkService_AmigoTenantTService] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([ServiceId])
GO
ALTER TABLE [dbo].[AmigoTenantTService] CHECK CONSTRAINT [fkService_AmigoTenantTService]
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge]  WITH CHECK ADD  CONSTRAINT [fkAmigoTenantTService_AmigoTenantTServiceCharge] FOREIGN KEY([AmigoTenantTServiceId])
REFERENCES [dbo].[AmigoTenantTService] ([AmigoTenantTServiceId])
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge] CHECK CONSTRAINT [fkAmigoTenantTService_AmigoTenantTServiceCharge]
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge]  WITH CHECK ADD  CONSTRAINT [fkDriverReport_AmigoTenantTServiceCharge] FOREIGN KEY([DriverReportId])
REFERENCES [dbo].[DriverReport] ([DriverReportId])
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge] CHECK CONSTRAINT [fkDriverReport_AmigoTenantTServiceCharge]
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge]  WITH CHECK ADD  CONSTRAINT [fkRate_AmigoTenantTServiceCharge] FOREIGN KEY([RateId])
REFERENCES [dbo].[Rate] ([RateId])
GO
ALTER TABLE [dbo].[AmigoTenantTServiceCharge] CHECK CONSTRAINT [fkRate_AmigoTenantTServiceCharge]
GO
ALTER TABLE [dbo].[AmigoTenantTUser]  WITH CHECK ADD  CONSTRAINT [fkAmigoTenantTRole_AmigoTenantTUser] FOREIGN KEY([AmigoTenantTRoleId])
REFERENCES [dbo].[AmigoTenantTRole] ([AmigoTenantTRoleId])
GO
ALTER TABLE [dbo].[AmigoTenantTUser] CHECK CONSTRAINT [fkAmigoTenantTRole_AmigoTenantTUser]
GO
ALTER TABLE [dbo].[AmigoTenantTUser]  WITH CHECK ADD  CONSTRAINT [fkLocation_AmigoTenantTUser] FOREIGN KEY([DedicatedLocationId])
REFERENCES [dbo].[Location] ([LocationId])
GO
ALTER TABLE [dbo].[AmigoTenantTUser] CHECK CONSTRAINT [fkLocation_AmigoTenantTUser]
GO
ALTER TABLE [dbo].[AppUser]  WITH CHECK ADD  CONSTRAINT [R_41] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[AppUser] CHECK CONSTRAINT [R_41]
GO
ALTER TABLE [dbo].[BusinessPartner]  WITH NOCHECK ADD  CONSTRAINT [fkBusinessPartner_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([CityId])
GO
ALTER TABLE [dbo].[BusinessPartner] CHECK CONSTRAINT [fkBusinessPartner_CityId]
GO
ALTER TABLE [dbo].[BusinessPartner]  WITH CHECK ADD  CONSTRAINT [R_46] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[BusinessPartner] CHECK CONSTRAINT [R_46]
GO
ALTER TABLE [dbo].[BusinessPartner]  WITH CHECK ADD  CONSTRAINT [R_47] FOREIGN KEY([TypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[BusinessPartner] CHECK CONSTRAINT [R_47]
GO
ALTER TABLE [dbo].[City]  WITH CHECK ADD  CONSTRAINT [fkState_City] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([StateId])
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [fkState_City]
GO
ALTER TABLE [dbo].[Concept]  WITH CHECK ADD  CONSTRAINT [R_34] FOREIGN KEY([PayTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[Concept] CHECK CONSTRAINT [R_34]
GO
ALTER TABLE [dbo].[Concept]  WITH CHECK ADD  CONSTRAINT [R_36] FOREIGN KEY([TypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[Concept] CHECK CONSTRAINT [R_36]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [FK_Contract_PeriodId] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [FK_Contract_PeriodId]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [R_26] FOREIGN KEY([HouseId])
REFERENCES [dbo].[House] ([HouseId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [R_26]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [R_31] FOREIGN KEY([ContractStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [R_31]
GO
ALTER TABLE [dbo].[Contract]  WITH CHECK ADD  CONSTRAINT [R_57] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[Contract] CHECK CONSTRAINT [R_57]
GO
ALTER TABLE [dbo].[ContractDetail]  WITH CHECK ADD  CONSTRAINT [R_32] FOREIGN KEY([ContractDetailStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[ContractDetail] CHECK CONSTRAINT [R_32]
GO
ALTER TABLE [dbo].[ContractDetail]  WITH CHECK ADD  CONSTRAINT [R_4] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[ContractDetail] CHECK CONSTRAINT [R_4]
GO
ALTER TABLE [dbo].[ContractDetail]  WITH CHECK ADD  CONSTRAINT [R_63] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[ContractDetail] CHECK CONSTRAINT [R_63]
GO
ALTER TABLE [dbo].[ContractDetail]  WITH CHECK ADD  CONSTRAINT [R_77] FOREIGN KEY([PayTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[ContractDetail] CHECK CONSTRAINT [R_77]
GO
ALTER TABLE [dbo].[ContractDetailObligation]  WITH CHECK ADD  CONSTRAINT [R_60] FOREIGN KEY([ContractDetailId])
REFERENCES [dbo].[ContractDetail] ([ContractDetailId])
GO
ALTER TABLE [dbo].[ContractDetailObligation] CHECK CONSTRAINT [R_60]
GO
ALTER TABLE [dbo].[ContractDetailObligation]  WITH CHECK ADD  CONSTRAINT [R_61] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO
ALTER TABLE [dbo].[ContractDetailObligation] CHECK CONSTRAINT [R_61]
GO
ALTER TABLE [dbo].[ContractDetailObligation]  WITH CHECK ADD  CONSTRAINT [R_62] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[ContractDetailObligation] CHECK CONSTRAINT [R_62]
GO
ALTER TABLE [dbo].[ContractDetailObligation]  WITH CHECK ADD  CONSTRAINT [R_71] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[ContractDetailObligation] CHECK CONSTRAINT [R_71]
GO
ALTER TABLE [dbo].[ContractDetailObligation]  WITH CHECK ADD  CONSTRAINT [R_74] FOREIGN KEY([TenantInfractorId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[ContractDetailObligation] CHECK CONSTRAINT [R_74]
GO
ALTER TABLE [dbo].[ContractDetailObligation]  WITH CHECK ADD  CONSTRAINT [R_75] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[ContractDetailObligation] CHECK CONSTRAINT [R_75]
GO
ALTER TABLE [dbo].[ContractDetailObligationPay]  WITH CHECK ADD  CONSTRAINT [R_73] FOREIGN KEY([ContractDetailObligationId])
REFERENCES [dbo].[ContractDetailObligation] ([ContractDetailObligationId])
GO
ALTER TABLE [dbo].[ContractDetailObligationPay] CHECK CONSTRAINT [R_73]
GO
ALTER TABLE [dbo].[ContractDetailObligationPay]  WITH CHECK ADD  CONSTRAINT [R_76] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[ContractDetailObligationPay] CHECK CONSTRAINT [R_76]
GO
ALTER TABLE [dbo].[ContractHouseDetail]  WITH CHECK ADD  CONSTRAINT [R_23] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[ContractHouseDetail] CHECK CONSTRAINT [R_23]
GO
ALTER TABLE [dbo].[ContractHouseDetail]  WITH CHECK ADD  CONSTRAINT [R_24] FOREIGN KEY([HouseFeatureId])
REFERENCES [dbo].[HouseFeature] ([HouseFeatureId])
GO
ALTER TABLE [dbo].[ContractHouseDetail] CHECK CONSTRAINT [R_24]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [fkAmigoTenantTUser_Device] FOREIGN KEY([AssignedAmigoTenantTUserId])
REFERENCES [dbo].[AmigoTenantTUser] ([AmigoTenantTUserId])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [fkAmigoTenantTUser_Device]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [fkAppVersion_Device] FOREIGN KEY([AppVersionId])
REFERENCES [dbo].[AppVersion] ([AppVersionId])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [fkAppVersion_Device]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [fkModel_Device] FOREIGN KEY([ModelId])
REFERENCES [dbo].[Model] ([ModelId])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [fkModel_Device]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [fkOSVersion_Device] FOREIGN KEY([OSVersionId])
REFERENCES [dbo].[OSVersion] ([OSVersionId])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [fkOSVersion_Device]
GO
ALTER TABLE [dbo].[DriverReport]  WITH CHECK ADD  CONSTRAINT [fkSystemUser_DriverReportApproverUserId] FOREIGN KEY([ApproverUserId])
REFERENCES [dbo].[AmigoTenantTUser] ([AmigoTenantTUserId])
GO
ALTER TABLE [dbo].[DriverReport] CHECK CONSTRAINT [fkSystemUser_DriverReportApproverUserId]
GO
ALTER TABLE [dbo].[DriverReport]  WITH CHECK ADD  CONSTRAINT [fkSystemUser_DriverReportDriverUserId] FOREIGN KEY([DriverUserId])
REFERENCES [dbo].[AmigoTenantTUser] ([AmigoTenantTUserId])
GO
ALTER TABLE [dbo].[DriverReport] CHECK CONSTRAINT [fkSystemUser_DriverReportDriverUserId]
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [fkEquipmentSize_Equipment] FOREIGN KEY([EquipmentSizeId])
REFERENCES [dbo].[EquipmentSize] ([EquipmentSizeId])
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [fkEquipmentSize_Equipment]
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [fkEquipmentStatus_Equipment] FOREIGN KEY([EquipmentStatusId])
REFERENCES [dbo].[EquipmentStatus] ([EquipmentStatusId])
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [fkEquipmentStatus_Equipment]
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [fkLocation_Equipment] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([LocationId])
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [fkLocation_Equipment]
GO
ALTER TABLE [dbo].[EquipmentSize]  WITH CHECK ADD  CONSTRAINT [fkEquipmentType_EquipmentSize] FOREIGN KEY([EquipmentTypeId])
REFERENCES [dbo].[EquipmentType] ([EquipmentTypeId])
GO
ALTER TABLE [dbo].[EquipmentSize] CHECK CONSTRAINT [fkEquipmentType_EquipmentSize]
GO
ALTER TABLE [dbo].[ExpenseDetail]  WITH CHECK ADD  CONSTRAINT [R_27] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO
ALTER TABLE [dbo].[ExpenseDetail] CHECK CONSTRAINT [R_27]
GO
ALTER TABLE [dbo].[ExpenseDetail]  WITH CHECK ADD  CONSTRAINT [R_68] FOREIGN KEY([ContractDetailId])
REFERENCES [dbo].[ContractDetail] ([ContractDetailId])
GO
ALTER TABLE [dbo].[ExpenseDetail] CHECK CONSTRAINT [R_68]
GO
ALTER TABLE [dbo].[ExpenseDetail]  WITH CHECK ADD  CONSTRAINT [R_70] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[ExpenseDetail] CHECK CONSTRAINT [R_70]
GO
ALTER TABLE [dbo].[Feature]  WITH CHECK ADD  CONSTRAINT [FK_Feature_HouseTypeId] FOREIGN KEY([HouseTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[Feature] CHECK CONSTRAINT [FK_Feature_HouseTypeId]
GO
ALTER TABLE [dbo].[FeatureAccesory]  WITH CHECK ADD  CONSTRAINT [R_28] FOREIGN KEY([AccesoryId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[FeatureAccesory] CHECK CONSTRAINT [R_28]
GO
ALTER TABLE [dbo].[FeatureAccesory]  WITH CHECK ADD  CONSTRAINT [R_40] FOREIGN KEY([HouseFeatureId])
REFERENCES [dbo].[HouseFeature] ([HouseFeatureId])
GO
ALTER TABLE [dbo].[FeatureAccesory] CHECK CONSTRAINT [R_40]
GO
ALTER TABLE [dbo].[FeatureImage]  WITH CHECK ADD  CONSTRAINT [R_16] FOREIGN KEY([HouseFeatureId])
REFERENCES [dbo].[HouseFeature] ([HouseFeatureId])
GO
ALTER TABLE [dbo].[FeatureImage] CHECK CONSTRAINT [R_16]
GO
ALTER TABLE [dbo].[Fine]  WITH NOCHECK ADD  CONSTRAINT [fkFine_ConceptId] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO
ALTER TABLE [dbo].[Fine] CHECK CONSTRAINT [fkFine_ConceptId]
GO
ALTER TABLE [dbo].[Fine]  WITH NOCHECK ADD  CONSTRAINT [fkFine_ContractId] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[Fine] CHECK CONSTRAINT [fkFine_ContractId]
GO
ALTER TABLE [dbo].[Fine]  WITH NOCHECK ADD  CONSTRAINT [fkFine_EntityStatusId] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[Fine] CHECK CONSTRAINT [fkFine_EntityStatusId]
GO
ALTER TABLE [dbo].[Fine]  WITH NOCHECK ADD  CONSTRAINT [fkFine_PeriodId] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[Fine] CHECK CONSTRAINT [fkFine_PeriodId]
GO
ALTER TABLE [dbo].[Fine]  WITH NOCHECK ADD  CONSTRAINT [fkFine_TenantId] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[Fine] CHECK CONSTRAINT [fkFine_TenantId]
GO
ALTER TABLE [dbo].[FinePayment]  WITH NOCHECK ADD  CONSTRAINT [fkFinePayment_FineId] FOREIGN KEY([FineId])
REFERENCES [dbo].[Fine] ([FineId])
GO
ALTER TABLE [dbo].[FinePayment] CHECK CONSTRAINT [fkFinePayment_FineId]
GO
ALTER TABLE [dbo].[House]  WITH CHECK ADD  CONSTRAINT [fkHouse_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([CityId])
GO
ALTER TABLE [dbo].[House] CHECK CONSTRAINT [fkHouse_CityId]
GO
ALTER TABLE [dbo].[House]  WITH CHECK ADD  CONSTRAINT [R_30] FOREIGN KEY([HouseStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[House] CHECK CONSTRAINT [R_30]
GO
ALTER TABLE [dbo].[House]  WITH CHECK ADD  CONSTRAINT [R_37] FOREIGN KEY([HouseTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[House] CHECK CONSTRAINT [R_37]
GO
ALTER TABLE [dbo].[HouseFeature]  WITH CHECK ADD  CONSTRAINT [R_14] FOREIGN KEY([HouseId])
REFERENCES [dbo].[House] ([HouseId])
GO
ALTER TABLE [dbo].[HouseFeature] CHECK CONSTRAINT [R_14]
GO
ALTER TABLE [dbo].[HouseFeature]  WITH CHECK ADD  CONSTRAINT [R_15] FOREIGN KEY([FeatureId])
REFERENCES [dbo].[Feature] ([FeatureId])
GO
ALTER TABLE [dbo].[HouseFeature] CHECK CONSTRAINT [R_15]
GO
ALTER TABLE [dbo].[HouseFeature]  WITH CHECK ADD  CONSTRAINT [R_38] FOREIGN KEY([HouseFeatureStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[HouseFeature] CHECK CONSTRAINT [R_38]
GO
ALTER TABLE [dbo].[HouseService]  WITH NOCHECK ADD  CONSTRAINT [fkHouseService_HouseId] FOREIGN KEY([HouseId])
REFERENCES [dbo].[House] ([HouseId])
GO
ALTER TABLE [dbo].[HouseService] CHECK CONSTRAINT [fkHouseService_HouseId]
GO
ALTER TABLE [dbo].[HouseService]  WITH NOCHECK ADD  CONSTRAINT [fkHouseService_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[ServiceHouse] ([ServiceId])
GO
ALTER TABLE [dbo].[HouseService] CHECK CONSTRAINT [fkHouseService_ServiceId]
GO
ALTER TABLE [dbo].[HouseServicePeriod]  WITH NOCHECK ADD  CONSTRAINT [fkHouseServicePeriod_HouseServiceId] FOREIGN KEY([HouseServiceId])
REFERENCES [dbo].[HouseService] ([HouseServiceId])
GO
ALTER TABLE [dbo].[HouseServicePeriod] CHECK CONSTRAINT [fkHouseServicePeriod_HouseServiceId]
GO
ALTER TABLE [dbo].[Income]  WITH CHECK ADD  CONSTRAINT [R_51] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[Income] CHECK CONSTRAINT [R_51]
GO
ALTER TABLE [dbo].[Income]  WITH CHECK ADD  CONSTRAINT [R_55] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[Income] CHECK CONSTRAINT [R_55]
GO
ALTER TABLE [dbo].[Income]  WITH CHECK ADD  CONSTRAINT [R_56] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[Income] CHECK CONSTRAINT [R_56]
GO
ALTER TABLE [dbo].[Income]  WITH CHECK ADD  CONSTRAINT [R_69] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[Income] CHECK CONSTRAINT [R_69]
GO
ALTER TABLE [dbo].[IncomeDetail]  WITH CHECK ADD  CONSTRAINT [R_52] FOREIGN KEY([IncomeId])
REFERENCES [dbo].[Income] ([IncomeId])
GO
ALTER TABLE [dbo].[IncomeDetail] CHECK CONSTRAINT [R_52]
GO
ALTER TABLE [dbo].[IncomeDetail]  WITH CHECK ADD  CONSTRAINT [R_53] FOREIGN KEY([ContractDetailId])
REFERENCES [dbo].[ContractDetail] ([ContractDetailId])
GO
ALTER TABLE [dbo].[IncomeDetail] CHECK CONSTRAINT [R_53]
GO
ALTER TABLE [dbo].[IncomeDetail]  WITH CHECK ADD  CONSTRAINT [R_54] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO
ALTER TABLE [dbo].[IncomeDetail] CHECK CONSTRAINT [R_54]
GO
ALTER TABLE [dbo].[IncomeDetail]  WITH CHECK ADD  CONSTRAINT [R_72] FOREIGN KEY([ContractDetailObligationId])
REFERENCES [dbo].[ContractDetailObligation] ([ContractDetailObligationId])
GO
ALTER TABLE [dbo].[IncomeDetail] CHECK CONSTRAINT [R_72]
GO
ALTER TABLE [dbo].[Invoice]  WITH NOCHECK ADD  CONSTRAINT [fkInvoice_BusinessPartnerId] FOREIGN KEY([BusinessPartnerId])
REFERENCES [dbo].[BusinessPartner] ([BusinessPartnerId])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [fkInvoice_BusinessPartnerId]
GO
ALTER TABLE [dbo].[Invoice]  WITH NOCHECK ADD  CONSTRAINT [fkInvoice_ContractId] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [fkInvoice_ContractId]
GO
ALTER TABLE [dbo].[Invoice]  WITH NOCHECK ADD  CONSTRAINT [fkInvoice_EntityStatusId] FOREIGN KEY([InvoiceStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [fkInvoice_EntityStatusId]
GO
ALTER TABLE [dbo].[Invoice]  WITH NOCHECK ADD  CONSTRAINT [fkInvoice_PaymentTypeId] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [fkInvoice_PaymentTypeId]
GO
ALTER TABLE [dbo].[InvoiceDetail]  WITH NOCHECK ADD  CONSTRAINT [fkInvoceDetail_PaymentPeriodId] FOREIGN KEY([PaymentPeriodId])
REFERENCES [dbo].[PaymentPeriod] ([PaymentPeriodId])
GO
ALTER TABLE [dbo].[InvoiceDetail] CHECK CONSTRAINT [fkInvoceDetail_PaymentPeriodId]
GO
ALTER TABLE [dbo].[InvoiceDetail]  WITH NOCHECK ADD  CONSTRAINT [fkInvoiceDetail_ConceptId] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO
ALTER TABLE [dbo].[InvoiceDetail] CHECK CONSTRAINT [fkInvoiceDetail_ConceptId]
GO
ALTER TABLE [dbo].[InvoiceDetail]  WITH NOCHECK ADD  CONSTRAINT [fkInvoiceDetail_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([InvoiceId])
GO
ALTER TABLE [dbo].[InvoiceDetail] CHECK CONSTRAINT [fkInvoiceDetail_InvoiceId]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [fkCity_Location] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([CityId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [fkCity_Location]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [fkLocationType_Location] FOREIGN KEY([LocationTypeId])
REFERENCES [dbo].[LocationType] ([LocationTypeId])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [fkLocationType_Location]
GO
ALTER TABLE [dbo].[LocationCoordinate]  WITH CHECK ADD  CONSTRAINT [fkLocation_LocationCoordinate] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([LocationId])
GO
ALTER TABLE [dbo].[LocationCoordinate] CHECK CONSTRAINT [fkLocation_LocationCoordinate]
GO
ALTER TABLE [dbo].[Model]  WITH CHECK ADD  CONSTRAINT [fkBrand_Model] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Brand] ([BrandId])
GO
ALTER TABLE [dbo].[Model] CHECK CONSTRAINT [fkBrand_Model]
GO
ALTER TABLE [dbo].[Module]  WITH CHECK ADD  CONSTRAINT [R_42] FOREIGN KEY([ParentModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
GO
ALTER TABLE [dbo].[Module] CHECK CONSTRAINT [R_42]
GO
ALTER TABLE [dbo].[OSVersion]  WITH CHECK ADD  CONSTRAINT [fkPlatform_OSVersion] FOREIGN KEY([PlatformId])
REFERENCES [dbo].[Platform] ([PlatformId])
GO
ALTER TABLE [dbo].[OSVersion] CHECK CONSTRAINT [fkPlatform_OSVersion]
GO
ALTER TABLE [dbo].[OtherTenant]  WITH CHECK ADD  CONSTRAINT [R_43] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[OtherTenant] CHECK CONSTRAINT [R_43]
GO
ALTER TABLE [dbo].[OtherTenant]  WITH CHECK ADD  CONSTRAINT [R_44] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[OtherTenant] CHECK CONSTRAINT [R_44]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH NOCHECK ADD  CONSTRAINT [fkPaymentPeriod_ConceptId] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [fkPaymentPeriod_ConceptId]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH NOCHECK ADD  CONSTRAINT [fkPaymentPeriod_ContractId] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [fkPaymentPeriod_ContractId]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH NOCHECK ADD  CONSTRAINT [fkPaymentPeriod_PaymentPeriodStatusId] FOREIGN KEY([PaymentPeriodStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [fkPaymentPeriod_PaymentPeriodStatusId]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH NOCHECK ADD  CONSTRAINT [fkPaymentPeriod_PaymentTypeId] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [fkPaymentPeriod_PaymentTypeId]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH NOCHECK ADD  CONSTRAINT [fkPaymentPeriod_PeriodId] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [fkPaymentPeriod_PeriodId]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH NOCHECK ADD  CONSTRAINT [fkPaymentPeriod_TenantId] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [fkPaymentPeriod_TenantId]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH CHECK ADD  CONSTRAINT [R_111] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [R_111]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH CHECK ADD  CONSTRAINT [R_112] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [R_112]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH CHECK ADD  CONSTRAINT [R_113] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [R_113]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH CHECK ADD  CONSTRAINT [R_114] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [R_114]
GO
ALTER TABLE [dbo].[PaymentPeriod]  WITH CHECK ADD  CONSTRAINT [R_117] FOREIGN KEY([PaymentPeriodStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[PaymentPeriod] CHECK CONSTRAINT [R_117]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [fkAction_Permission] FOREIGN KEY([ActionId])
REFERENCES [dbo].[Action] ([ActionId])
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [fkAction_Permission]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [fkAmigoTenantTRole_Permission] FOREIGN KEY([AmigoTenantTRoleId])
REFERENCES [dbo].[AmigoTenantTRole] ([AmigoTenantTRoleId])
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [fkAmigoTenantTRole_Permission]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [R_13] FOREIGN KEY([ActionId])
REFERENCES [dbo].[Action] ([ActionId])
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [R_13]
GO
ALTER TABLE [dbo].[Rate]  WITH CHECK ADD  CONSTRAINT [fkService_Rate] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([ServiceId])
GO
ALTER TABLE [dbo].[Rate] CHECK CONSTRAINT [fkService_Rate]
GO
ALTER TABLE [dbo].[RentalApplication]  WITH CHECK ADD  CONSTRAINT [FK_RentalApplication_BudgetId] FOREIGN KEY([BudgetId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[RentalApplication] CHECK CONSTRAINT [FK_RentalApplication_BudgetId]
GO
ALTER TABLE [dbo].[RentalApplication]  WITH CHECK ADD  CONSTRAINT [FK_RentalApplication_PeriodId] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[RentalApplication] CHECK CONSTRAINT [FK_RentalApplication_PeriodId]
GO
ALTER TABLE [dbo].[RentalApplication]  WITH CHECK ADD  CONSTRAINT [FK_RentalApplication_PropertyTypeId] FOREIGN KEY([PropertyTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[RentalApplication] CHECK CONSTRAINT [FK_RentalApplication_PropertyTypeId]
GO
ALTER TABLE [dbo].[RentalApplication]  WITH CHECK ADD  CONSTRAINT [FK_RentalApplication_ResidenseCountryId] FOREIGN KEY([ResidenseCountryId])
REFERENCES [dbo].[Country] ([CountryId])
GO
ALTER TABLE [dbo].[RentalApplication] CHECK CONSTRAINT [FK_RentalApplication_ResidenseCountryId]
GO
ALTER TABLE [dbo].[RentalApplication]  WITH CHECK ADD  CONSTRAINT [fkRentalApplication_CityOfInterestId] FOREIGN KEY([CityOfInterestId])
REFERENCES [dbo].[City] ([CityId])
GO
ALTER TABLE [dbo].[RentalApplication] CHECK CONSTRAINT [fkRentalApplication_CityOfInterestId]
GO
ALTER TABLE [dbo].[RentalApplication]  WITH CHECK ADD  CONSTRAINT [fkRentalApplication_HousePartId] FOREIGN KEY([HousePartId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[RentalApplication] CHECK CONSTRAINT [fkRentalApplication_HousePartId]
GO
ALTER TABLE [dbo].[RentalApplication]  WITH CHECK ADD  CONSTRAINT [fkRentalApplication_OutInDownId] FOREIGN KEY([OutInDownId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[RentalApplication] CHECK CONSTRAINT [fkRentalApplication_OutInDownId]
GO
ALTER TABLE [dbo].[RentalApplication]  WITH CHECK ADD  CONSTRAINT [fkRentalApplication_ReferredById] FOREIGN KEY([ReferredById])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[RentalApplication] CHECK CONSTRAINT [fkRentalApplication_ReferredById]
GO
ALTER TABLE [dbo].[RentalApplicationCity]  WITH CHECK ADD  CONSTRAINT [FK_RentalApplicationCity_CityId] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([CityId])
GO
ALTER TABLE [dbo].[RentalApplicationCity] CHECK CONSTRAINT [FK_RentalApplicationCity_CityId]
GO
ALTER TABLE [dbo].[RentalApplicationCity]  WITH CHECK ADD  CONSTRAINT [FK_RentalApplicationCity_RentalApplicationId] FOREIGN KEY([RentalApplicationId])
REFERENCES [dbo].[RentalApplication] ([RentalApplicationId])
GO
ALTER TABLE [dbo].[RentalApplicationCity] CHECK CONSTRAINT [FK_RentalApplicationCity_RentalApplicationId]
GO
ALTER TABLE [dbo].[RentalApplicationFeature]  WITH CHECK ADD  CONSTRAINT [FK_RentalApplicationFeature_FeatureId] FOREIGN KEY([FeatureId])
REFERENCES [dbo].[Feature] ([FeatureId])
GO
ALTER TABLE [dbo].[RentalApplicationFeature] CHECK CONSTRAINT [FK_RentalApplicationFeature_FeatureId]
GO
ALTER TABLE [dbo].[RentalApplicationFeature]  WITH CHECK ADD  CONSTRAINT [FK_RentalApplicationFeature_RentalApplicationId] FOREIGN KEY([RentalApplicationId])
REFERENCES [dbo].[RentalApplication] ([RentalApplicationId])
GO
ALTER TABLE [dbo].[RentalApplicationFeature] CHECK CONSTRAINT [FK_RentalApplicationFeature_RentalApplicationId]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [fkServiceType_Service] FOREIGN KEY([ServiceTypeId])
REFERENCES [dbo].[ServiceType] ([ServiceTypeId])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [fkServiceType_Service]
GO
ALTER TABLE [dbo].[ServiceHouse]  WITH CHECK ADD  CONSTRAINT [fkService_BusinessPartnerId] FOREIGN KEY([BusinessPartnerId])
REFERENCES [dbo].[BusinessPartner] ([BusinessPartnerId])
GO
ALTER TABLE [dbo].[ServiceHouse] CHECK CONSTRAINT [fkService_BusinessPartnerId]
GO
ALTER TABLE [dbo].[ServiceHouse]  WITH CHECK ADD  CONSTRAINT [fkService_ConceptId] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO
ALTER TABLE [dbo].[ServiceHouse] CHECK CONSTRAINT [fkService_ConceptId]
GO
ALTER TABLE [dbo].[ServiceHouse]  WITH CHECK ADD  CONSTRAINT [fkService_ServiceTypeId] FOREIGN KEY([ServiceTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[ServiceHouse] CHECK CONSTRAINT [fkService_ServiceTypeId]
GO
ALTER TABLE [dbo].[ServiceHousePeriod]  WITH NOCHECK ADD  CONSTRAINT [fkServiceHousePeriod_ServiceId] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[ServiceHouse] ([ServiceId])
GO
ALTER TABLE [dbo].[ServiceHousePeriod] CHECK CONSTRAINT [fkServiceHousePeriod_ServiceId]
GO
ALTER TABLE [dbo].[ServicePeriod]  WITH NOCHECK ADD  CONSTRAINT [fkServicePeriod_HouseServicePeriodId] FOREIGN KEY([HouseServicePeriodId])
REFERENCES [dbo].[HouseServicePeriod] ([HouseServicePeriodId])
GO
ALTER TABLE [dbo].[ServicePeriod] CHECK CONSTRAINT [fkServicePeriod_HouseServicePeriodId]
GO
ALTER TABLE [dbo].[ServicePeriod]  WITH NOCHECK ADD  CONSTRAINT [R_123] FOREIGN KEY([ServicePeriodStatusId])
REFERENCES [dbo].[ServicePeriodStatus] ([ServicePeriodStatusId])
GO
ALTER TABLE [dbo].[ServicePeriod] CHECK CONSTRAINT [R_123]
GO
ALTER TABLE [dbo].[ServicePeriod]  WITH NOCHECK ADD  CONSTRAINT [R_124] FOREIGN KEY([ServiceStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[ServicePeriod] CHECK CONSTRAINT [R_124]
GO
ALTER TABLE [dbo].[ServicePeriod]  WITH NOCHECK ADD  CONSTRAINT [R_87] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
ALTER TABLE [dbo].[ServicePeriod] CHECK CONSTRAINT [R_87]
GO
ALTER TABLE [dbo].[ServicePeriod]  WITH NOCHECK ADD  CONSTRAINT [R_90] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[BusinessPartner] ([BusinessPartnerId])
GO
ALTER TABLE [dbo].[ServicePeriod] CHECK CONSTRAINT [R_90]
GO
ALTER TABLE [dbo].[ServicePeriod]  WITH NOCHECK ADD  CONSTRAINT [R_91] FOREIGN KEY([UM])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[ServicePeriod] CHECK CONSTRAINT [R_91]
GO
ALTER TABLE [dbo].[ServicePeriod]  WITH NOCHECK ADD  CONSTRAINT [R_92] FOREIGN KEY([ServicePeriodStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[ServicePeriod] CHECK CONSTRAINT [R_92]
GO
ALTER TABLE [dbo].[ServicePeriodStatus]  WITH NOCHECK ADD  CONSTRAINT [fkServicePeriodStatus_EntityStatusId] FOREIGN KEY([ServiceStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[ServicePeriodStatus] CHECK CONSTRAINT [fkServicePeriodStatus_EntityStatusId]
GO
ALTER TABLE [dbo].[State]  WITH CHECK ADD  CONSTRAINT [fkCountry_State] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([CountryId])
GO
ALTER TABLE [dbo].[State] CHECK CONSTRAINT [fkCountry_State]
GO
ALTER TABLE [dbo].[Tenant]  WITH CHECK ADD  CONSTRAINT [fk_Tenant_CountryId] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Country] ([CountryId])
GO
ALTER TABLE [dbo].[Tenant] CHECK CONSTRAINT [fk_Tenant_CountryId]
GO
ALTER TABLE [dbo].[Tenant]  WITH CHECK ADD  CONSTRAINT [R_58] FOREIGN KEY([TypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])
GO
ALTER TABLE [dbo].[Tenant] CHECK CONSTRAINT [R_58]
GO
ALTER TABLE [sec].[ClientClaims]  WITH CHECK ADD  CONSTRAINT [fkClients_ClientClaims] FOREIGN KEY([Client_Id])
REFERENCES [sec].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ClientClaims] CHECK CONSTRAINT [fkClients_ClientClaims]
GO
ALTER TABLE [sec].[ClientCorsOrigins]  WITH CHECK ADD  CONSTRAINT [fkClients_ClientCorsOrigins] FOREIGN KEY([Client_Id])
REFERENCES [sec].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ClientCorsOrigins] CHECK CONSTRAINT [fkClients_ClientCorsOrigins]
GO
ALTER TABLE [sec].[ClientCustomGrantTypes]  WITH CHECK ADD  CONSTRAINT [fkClients_ClientCustomGrantTypes] FOREIGN KEY([Client_Id])
REFERENCES [sec].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ClientCustomGrantTypes] CHECK CONSTRAINT [fkClients_ClientCustomGrantTypes]
GO
ALTER TABLE [sec].[ClientIdPRestrictions]  WITH CHECK ADD  CONSTRAINT [fkClients_ClientIdPRestrictions] FOREIGN KEY([Client_Id])
REFERENCES [sec].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ClientIdPRestrictions] CHECK CONSTRAINT [fkClients_ClientIdPRestrictions]
GO
ALTER TABLE [sec].[ClientPostLogoutRedirectUris]  WITH CHECK ADD  CONSTRAINT [fkClients_ClientPostLogoutRedirectUris] FOREIGN KEY([Client_Id])
REFERENCES [sec].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ClientPostLogoutRedirectUris] CHECK CONSTRAINT [fkClients_ClientPostLogoutRedirectUris]
GO
ALTER TABLE [sec].[ClientRedirectUris]  WITH CHECK ADD  CONSTRAINT [fkClients_ClientRedirectUris] FOREIGN KEY([Client_Id])
REFERENCES [sec].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ClientRedirectUris] CHECK CONSTRAINT [fkClients_ClientRedirectUris]
GO
ALTER TABLE [sec].[ClientScopes]  WITH CHECK ADD  CONSTRAINT [fkClients_ClientScopes] FOREIGN KEY([Client_Id])
REFERENCES [sec].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ClientScopes] CHECK CONSTRAINT [fkClients_ClientScopes]
GO
ALTER TABLE [sec].[ClientSecrets]  WITH CHECK ADD  CONSTRAINT [fkClients_ClientSecrets] FOREIGN KEY([Client_Id])
REFERENCES [sec].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ClientSecrets] CHECK CONSTRAINT [fkClients_ClientSecrets]
GO
ALTER TABLE [sec].[ScopeClaims]  WITH CHECK ADD  CONSTRAINT [fkScopes_ScopeClaims] FOREIGN KEY([Scope_Id])
REFERENCES [sec].[Scopes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ScopeClaims] CHECK CONSTRAINT [fkScopes_ScopeClaims]
GO
ALTER TABLE [sec].[ScopeSecrets]  WITH CHECK ADD  CONSTRAINT [fkScopes_ScopeSecrets] FOREIGN KEY([Scope_Id])
REFERENCES [sec].[Scopes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[ScopeSecrets] CHECK CONSTRAINT [fkScopes_ScopeSecrets]
GO
ALTER TABLE [sec].[UserClaim]  WITH CHECK ADD  CONSTRAINT [fkUser_UserClaim] FOREIGN KEY([UserId])
REFERENCES [sec].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[UserClaim] CHECK CONSTRAINT [fkUser_UserClaim]
GO
ALTER TABLE [sec].[UserLogin]  WITH CHECK ADD  CONSTRAINT [fkUser_UserLogin] FOREIGN KEY([UserId])
REFERENCES [sec].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[UserLogin] CHECK CONSTRAINT [fkUser_UserLogin]
GO
ALTER TABLE [sec].[UserRole]  WITH CHECK ADD  CONSTRAINT [fkRole_UserRole] FOREIGN KEY([RoleId])
REFERENCES [sec].[Role] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[UserRole] CHECK CONSTRAINT [fkRole_UserRole]
GO
ALTER TABLE [sec].[UserRole]  WITH CHECK ADD  CONSTRAINT [fkUser_UserRole] FOREIGN KEY([UserId])
REFERENCES [sec].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [sec].[UserRole] CHECK CONSTRAINT [fkUser_UserRole]
GO





