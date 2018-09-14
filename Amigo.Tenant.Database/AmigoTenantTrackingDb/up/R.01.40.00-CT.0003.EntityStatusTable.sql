set ansi_padding on
if not exists (select 1 from sysobjects so 
                where so.name = 'EntityStatusTable' and Type  = 'U')
begin
	CREATE TABLE dbo.EntityStatusTable (
		Code varchar(3) NOT NULL,
		Name varchar(100) NOT NULL,
		RowStatus bit NOT NULL,
		CreatedBy int NOT NULL,
		CreationDate datetime2 NOT NULL,
		UpdatedBy int NULL,
		UpdatedDate datetime2 NULL
	)

	alter table dbo.EntityStatusTable
		add constraint pkCode Primary key (Code)
end
go

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'dbo.fkEntityStatus_EntityCode') 
			AND parent_object_id = OBJECT_ID(N'EntityStatus'))

	alter table dbo.EntityStatus
		add constraint fkEntityStatus_EntityCode foreign key (EntityCode)
			references dbo.EntityStatusTable(Code)
GO