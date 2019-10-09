
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


if not exists(select * from entitystatus where code = 'RENEWED' and EntityCode = 'CO')
BEGIN
INSERT INTO EntityStatus (Code, Name, EntityCode, Sequence, CreatedBy, CreationDate)
VALUES('RENEWED', 'Renewed', 'CO', 7, 1, getdate())
END

go 
IF EXISTS (select  so.name from sysobjects so WHERE so.name = 'ContractChangeStatus') 
BEGIN
	DROP TABLE [ContractChangeStatus]
end

CREATE TABLE [dbo].[ContractChangeStatus](
	[ContractChangeStatusId] [int] IDENTITY(1,1) NOT NULL,
	[ContractId] [int] NOT NULL,
	[ContractStatusId] [int] NOT NULL,
	[TenantId] [int] NULL,
	[HouseId] [int] NULL,
	[Rent] DECIMAL(8,2) NULL,
	[Deposit] DECIMAL(8,2) NULL,
	[BeginPeriodId] [INT] NULL,
	[EndPeriodId] [INT] NULL,
	[ContractTermType] [nvarchar](20) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL
	
 CONSTRAINT [PK_ContractChangeStatusId] PRIMARY KEY CLUSTERED 
(
	[ContractChangeStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[ContractChangeStatus]  WITH NOCHECK ADD  CONSTRAINT [fkContractChangeStatus_ContractId] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contract] ([ContractId])

ALTER TABLE [dbo].[ContractChangeStatus]  WITH NOCHECK ADD  CONSTRAINT [fkContractChangeStatus_ContractStatusId] FOREIGN KEY([ContractStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])

ALTER TABLE [dbo].[ContractChangeStatus]  WITH NOCHECK ADD  CONSTRAINT [fkContractChangeStatus_TenantId] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])

ALTER TABLE [dbo].[ContractChangeStatus]  WITH NOCHECK ADD  CONSTRAINT [fkContractChangeStatus_HouseId] FOREIGN KEY([HouseId])
REFERENCES [dbo].[House] ([HouseId])

ALTER TABLE [dbo].[ContractChangeStatus]  WITH NOCHECK ADD  CONSTRAINT [fkContractChangeStatus_BeginPeriodId] FOREIGN KEY([BeginPeriodId])
REFERENCES [dbo].[Period] ([PeriodId])

ALTER TABLE [dbo].[ContractChangeStatus]  WITH NOCHECK ADD  CONSTRAINT [fkContractChangeStatus_EndPeriodId] FOREIGN KEY([EndPeriodId])
REFERENCES [dbo].[Period] ([PeriodId])

go

