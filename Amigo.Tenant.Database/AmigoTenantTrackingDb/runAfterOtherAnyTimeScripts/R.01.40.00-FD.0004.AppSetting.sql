IF NOT EXISTS(SELECT 1 FROM appSetting WHERE Code='LATEFEEXDY' )
BEGIN
		 Insert Into appSetting(Code,Name,AppSettingValue,RowStatus,CreatedBy,CreationDate)
	 Values ('LATEFEEXDY', 'Late Fee Amount per Day', '25' ,1, '1', GETDATE() )
END
