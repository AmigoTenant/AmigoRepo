
IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'HouseId' 
			AND sc.id = so.id AND so.name = 'PaymentPeriod') 
BEGIN
	ALTER TABLE [PaymentPeriod]
	ADD [HouseId] INT NULL

ALTER TABLE [dbo].[PaymentPeriod]  WITH NOCHECK ADD  CONSTRAINT [fkPaymentPeriod_HouseId] FOREIGN KEY([HouseId])
REFERENCES [dbo].[House] ([HouseId])

END 
GO

--ACTUALIZANDO EL HOUSEID

update PaymentPeriod set HouseId = C.HouseId
From PaymentPeriod PP 
	inner join Contract C on C.ContractId = PP.ContractId

go


INSERT INTO EntityStatus (Code, Name, EntityCode, Sequence, CreatedBy, CreationDate)
VALUES('RENEWED', 'Renewed', 'CO', 7, 1, getdate())