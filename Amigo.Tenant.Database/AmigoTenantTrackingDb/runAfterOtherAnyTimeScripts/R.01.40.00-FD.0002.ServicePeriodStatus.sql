IF NOT EXISTS(SELECT 1 FROM EntityStatusTable WHERE Code='CO')
BEGIN
	INSERT INTO EntityStatusTable(Code, Name, RowStatus, CreatedBy, CreationDate)
	SELECT 'CO','CO',1,1, GETDATE();
END
IF NOT EXISTS(SELECT 1 FROM EntityStatusTable WHERE Code='HO')
BEGIN
	INSERT INTO EntityStatusTable(Code, Name, RowStatus, CreatedBy, CreationDate)
	SELECT 'HO','House Status',1,1, GETDATE();
END
IF NOT EXISTS(SELECT 1 FROM EntityStatusTable WHERE Code='CD')
BEGIN
	INSERT INTO EntityStatusTable(Code, Name, RowStatus, CreatedBy, CreationDate)
	SELECT 'CD','CD',1,1, GETDATE();
END
IF NOT EXISTS(SELECT 1 FROM EntityStatusTable WHERE Code='PP')
BEGIN
	INSERT INTO EntityStatusTable(Code, Name, RowStatus, CreatedBy, CreationDate)
	SELECT 'PP','P Pay',1,1, GETDATE();
END
IF NOT EXISTS(SELECT 1 FROM EntityStatusTable WHERE Code='HP')
BEGIN
	INSERT INTO EntityStatusTable(Code, Name, RowStatus, CreatedBy, CreationDate)
	SELECT 'HP','HouseServicePeriodStatus',1,1, GETDATE();
END

IF NOT EXISTS(SELECT 1 FROM EntityStatus WHERE Code='DRAFT' AND EntityCode = 'HP')
BEGIN
	INSERT INTO EntityStatus(Code, Name, EntityCode, Sequence, CreatedBy, CreationDate)
	SELECT 'DRAFT','Draft','HP', 1, 1, GETDATE();
END

IF NOT EXISTS(SELECT 1 FROM EntityStatus WHERE Code='REGISTERED' AND EntityCode = 'HP')
BEGIN
	INSERT INTO EntityStatus(Code, Name, EntityCode, Sequence, CreatedBy, CreationDate)
	SELECT 'REGISTERED','Registered','HP', 2, 1, GETDATE();
END

