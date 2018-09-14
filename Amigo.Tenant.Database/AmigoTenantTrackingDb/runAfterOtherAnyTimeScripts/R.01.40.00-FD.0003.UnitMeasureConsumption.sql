IF NOT EXISTS(SELECT 1 FROM GeneralTable WHERE Code='KW/h' AND TableName = 'UnitMeasure')
BEGIN	
	INSERT INTO GeneralTable(TableName, Code, Value, Sequence, ByDefault, RowStatus, CreatedBy, CreationDate)
	SELECT 'UnitMeasure','KW/h','KiloWatt/Hour',1,0,1,1, GETDATE();
END

IF NOT EXISTS(SELECT 1 FROM GeneralTable WHERE Code='M3' AND TableName = 'UnitMeasure')
BEGIN
	INSERT INTO GeneralTable(TableName, Code, Value, Sequence, ByDefault, RowStatus, CreatedBy, CreationDate)
	SELECT 'UnitMeasure','M3','Cubic Meter',1,0,1,1, GETDATE();
END
