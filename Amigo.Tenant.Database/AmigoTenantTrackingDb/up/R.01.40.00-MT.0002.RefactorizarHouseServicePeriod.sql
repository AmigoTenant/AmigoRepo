set ansi_padding on


if not exists (select 1 from syscolumns sc inner join sysobjects so on sc.id = so.id
                where sc.name = 'PeriodId' and so.name = 'HouseServicePeriod')
begin
	alter TABLE dbo.HouseServicePeriod 
		add PeriodId int NULL

	alter table dbo.HouseServicePeriod 
		add constraint fkHouseServicePeriod_PeriodId foreign key (PeriodId)
			references dbo.Period(PeriodId)

end
go
if not exists (select 1 from syscolumns sc inner join sysobjects so on sc.id = so.id
                where sc.name = 'Amount' and so.name = 'HouseServicePeriod')
begin
	alter TABLE dbo.HouseServicePeriod 
		add Amount decimal(8,2) NULL 
end
go

sp_bindefault 'DefaultCero', 'dbo.HouseServicePeriod.Amount';
go

if not exists (select 1 from syscolumns sc inner join sysobjects so on sc.id = so.id
                where sc.name = 'Adjust' and so.name = 'HouseServicePeriod')
begin
	alter TABLE dbo.HouseServicePeriod 
		add Adjust decimal(8,2) NULL 
end
go
sp_bindefault 'DefaultCero', 'dbo.HouseServicePeriod.Adjust';
go

if not exists (select 1 from syscolumns sc inner join sysobjects so on sc.id = so.id
                where sc.name = 'Consumption' and so.name = 'HouseServicePeriod')
begin
	alter TABLE dbo.HouseServicePeriod 
		add Consumption decimal(8,2) NULL 
end
go
sp_bindefault 'DefaultCero', 'dbo.HouseServicePeriod.Consumption';
go

if not exists (select 1 from syscolumns sc inner join sysobjects so on sc.id = so.id
                where sc.name = 'ConsumptionUnmId' and so.name = 'HouseServicePeriod')
begin
	alter TABLE dbo.HouseServicePeriod 
		add ConsumptionUnmId int NULL

	alter table dbo.HouseServicePeriod 
		add constraint fkHouseServicePeriod_ConsumptionUnmId foreign key (ConsumptionUnmId)
			references dbo.GeneralTable(GeneralTableId)
end
go

if not exists (select 1 from syscolumns sc inner join sysobjects so on sc.id = so.id
                where sc.name = 'HouseServicePeriodStatusId' and so.name = 'HouseServicePeriod')
begin
	alter TABLE dbo.HouseServicePeriod 
		add HouseServicePeriodStatusId int NULL

	alter table dbo.HouseServicePeriod 
		add constraint fkHouseServicePeriod_HouseServicePeriodStatusId foreign key (HouseServicePeriodStatusId)
			references dbo.EntityStatus(EntityStatusId)
end
go

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.R_90') 
			AND parent_object_id = OBJECT_ID(N'ServicePeriod'))
	ALTER TABLE [ServicePeriod] DROP CONSTRAINT [R_90]
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.R_91') 
			AND parent_object_id = OBJECT_ID(N'dbo.ServicePeriod'))
	ALTER TABLE [ServicePeriod] DROP CONSTRAINT [R_91]
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.R_92') 
			AND parent_object_id = OBJECT_ID(N'dbo.ServicePeriod'))
	ALTER TABLE [ServicePeriod] DROP CONSTRAINT [R_92]
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.R_124') 
			AND parent_object_id = OBJECT_ID(N'dbo.ServicePeriod'))
	ALTER TABLE [ServicePeriod] DROP CONSTRAINT [R_124]
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.fkServicePeriod_HouseServicePeriodId') 
			AND parent_object_id = OBJECT_ID(N'dbo.ServicePeriod'))
	ALTER TABLE [ServicePeriod] DROP CONSTRAINT [fkServicePeriod_HouseServicePeriodId]
GO

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.R_87') 
			AND parent_object_id = OBJECT_ID(N'dbo.ServicePeriod'))
	ALTER TABLE [ServicePeriod] DROP CONSTRAINT [R_87]
GO
