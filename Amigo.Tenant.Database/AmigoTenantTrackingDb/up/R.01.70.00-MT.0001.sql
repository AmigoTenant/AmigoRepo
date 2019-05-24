if not exists(Select * from GeneralTable Where Code='FREANUAL01' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('FREANUAL01','Frecuencia','Anual','1','0','1','1', GETDATE() ,'1',GETDATE())
if not exists(Select * from GeneralTable Where Code='FREPERIODO' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('FREPERIODO','Frecuencia','Periodo','2','0','1','1', GETDATE() ,'1',GETDATE())

--AGREGANDO CONCEPTOS PARA INGRESOS EN DASHBOARD

IF EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'AppSettingValue' 
			AND sc.id = so.id AND so.name = 'AppSetting') 
BEGIN
	alter table AppSetting alter column AppSettingValue nvarchar(255)

	insert into AppSetting (code, name , AppSettingValue, RowStatus, CreatedBy, CreationDate)
	values ('DBINCCPTOS', 'Conceptos de ingreso para DashBoard', 'ONACCOUNT,LATEFEE,DEPOSIT,RENT', 1, 1, getdate())



END 

GO