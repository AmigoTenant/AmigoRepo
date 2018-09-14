if exists (select name from sys.objects where type_desc = 'DEFAULT_CONSTRAINT' and name = 'DefaultCero')
	DROP DEFAULT DefaultCero
GO
CREATE DEFAULT DefaultCero AS 0;
GO