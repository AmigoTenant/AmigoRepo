IF NOT EXISTS(SELECT 1 FROM appSetting WHERE Code='CPTTOFAVTN' )
BEGIN
		 Insert Into appSetting(Code,Name,AppSettingValue,RowStatus,CreatedBy,CreationDate)
	 Values ('CPTTOFAVTN', 'Concepts favorable to tenant ', 'ONACCOUNT' ,1, '1', GETDATE() )
END