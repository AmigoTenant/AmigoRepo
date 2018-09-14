
set ansi_padding on
if not exists (select  sc.name
                from syscolumns sc, sysobjects so
                where
                  sc.name = 'PayDate'
                  and sc.id = so.id
                  and so.name = 'Period'
              )
BEGIN
	ALTER TABLE Period
		ADD PayDate datetime2 not null 
END