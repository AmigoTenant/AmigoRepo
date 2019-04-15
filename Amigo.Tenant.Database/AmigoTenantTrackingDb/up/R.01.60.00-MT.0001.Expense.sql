IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'ConceptId' 
			AND sc.id = so.id AND so.name = 'Expense') 
	ALTER TABLE [Expense]
	ADD [ConceptId] INT NULL

ALTER TABLE [dbo].[Expense]  WITH NOCHECK ADD  CONSTRAINT [fkExpense_ConceptId] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO


