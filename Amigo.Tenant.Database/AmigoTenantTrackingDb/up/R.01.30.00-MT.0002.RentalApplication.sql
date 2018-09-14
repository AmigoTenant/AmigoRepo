IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'PriorityId' 
				AND sc.id = so.id AND so.name = 'RentalApplication') 
BEGIN
	ALTER TABLE [RentalApplication]
	ADD PriorityId int null

	ALTER TABLE [RentalApplication]
	ADD CONSTRAINT fkRentalApplication_PriorityId FOREIGN KEY (PriorityId)
		REFERENCES GeneralTable(GeneralTableId)
END

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'AlertDate' 
			AND sc.id = so.id AND so.name = 'RentalApplication') 
BEGIN
	ALTER TABLE [RentalApplication]
	ADD AlertDate Datetime2 NULL
END

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'AlertMessage' 
			AND sc.id = so.id AND so.name = 'RentalApplication') 
BEGIN
	ALTER TABLE [RentalApplication]
	ADD AlertMessage VARCHAR(500)
END

IF EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'AlertBeforeThat' 
			AND sc.id = so.id AND so.name = 'RentalApplication') 
BEGIN
	ALTER TABLE [RentalApplication]
	DROP COLUMN AlertBeforeThat
END