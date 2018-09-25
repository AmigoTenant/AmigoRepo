IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'TenantId' 
			AND sc.id = so.id AND so.name = 'Invoice') 
	ALTER TABLE [Invoice]
	ADD [TenantId] INT NULL

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'PeriodId' 
			AND sc.id = so.id AND so.name = 'Invoice') 
	ALTER TABLE [Invoice]
	ADD [PeriodId] INT NULL
GO

ALTER TABLE [dbo].[Invoice]  WITH NOCHECK ADD  CONSTRAINT [fkInvoce_TenantId] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
GO

ALTER TABLE [dbo].[Invoice]  WITH NOCHECK ADD  CONSTRAINT [fkInvoce_PeriodId] FOREIGN KEY([PeriodId])
REFERENCES [dbo].[Period] ([PeriodId])
GO
