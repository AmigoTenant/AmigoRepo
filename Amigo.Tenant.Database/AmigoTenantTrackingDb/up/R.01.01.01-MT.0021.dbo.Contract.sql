
set ansi_padding on
if not exists (select  sc.name
                from syscolumns sc, sysobjects so
                where
                  sc.name = 'FrecuencyTypeId'
                  and sc.id = so.id
                  and so.name = 'Contract'
              )
BEGIN
	ALTER TABLE [Contract]
		ALTER COLUMN FrecuencyTypeId int NULL
END