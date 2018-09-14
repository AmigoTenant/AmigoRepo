
IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'TotalRent' 
				AND sc.id = so.id AND so.name = 'Invoice') 
	ALTER TABLE [Invoice]
	ADD [TotalRent] [decimal](8, 2) NULL

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'TotalDeposit' 
			AND sc.id = so.id AND so.name = 'Invoice') 
	ALTER TABLE [Invoice]
	ADD [TotalDeposit] [decimal](8, 2) NULL

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'TotalLateFee' 
			AND sc.id = so.id AND so.name = 'Invoice') 
	ALTER TABLE [Invoice]
	ADD [TotalLateFee] [decimal](8, 2) NULL

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'TotalService' 
			AND sc.id = so.id AND so.name = 'Invoice') 
	ALTER TABLE [Invoice]
	ADD [TotalService] [decimal](8, 2) NULL

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'TotalFine' 
			AND sc.id = so.id AND so.name = 'Invoice') 
	ALTER TABLE [Invoice]
	ADD [TotalFine] [decimal](8, 2) NULL

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'TotalOnAcount' 
			AND sc.id = so.id AND so.name = 'Invoice') 
	ALTER TABLE [Invoice]
	ADD [TotalOnAcount] [decimal](8, 2) NULL


--INVOICE DETAIL

GO

ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [fkInvoiceDetail_InvoiceId]
GO

ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [fkInvoiceDetail_ConceptId]
GO

ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [fkInvoceDetail_PaymentPeriodId]
GO

/****** Object:  Table [dbo].[InvoiceDetail]    Script Date: 1/23/2018 11:44:02 PM ******/
DROP TABLE [dbo].[InvoiceDetail]
GO

/****** Object:  Table [dbo].[InvoiceDetail]    Script Date: 1/23/2018 11:44:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InvoiceDetail](
	[InvoiceDetailId] [int] Identity(1,1) NOT NULL,
	[InvoiceId] [int] NULL,
	[PaymentPeriodId] [int] NULL,
	[ConceptId] [int] NULL,
	[Qty] [decimal](6, 2) NULL,
	[UnitPrice] [decimal](8, 2) NULL,
	[TotalAmount] [decimal](8, 2) NULL,
	[RowStatus] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [XPKInvoiceDetail] PRIMARY KEY CLUSTERED 
(
	[InvoiceDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[InvoiceDetail]  WITH NOCHECK ADD  CONSTRAINT [fkInvoceDetail_PaymentPeriodId] FOREIGN KEY([PaymentPeriodId])
REFERENCES [dbo].[PaymentPeriod] ([PaymentPeriodId])
GO

ALTER TABLE [dbo].[InvoiceDetail] CHECK CONSTRAINT [fkInvoceDetail_PaymentPeriodId]
GO

ALTER TABLE [dbo].[InvoiceDetail]  WITH NOCHECK ADD  CONSTRAINT [fkInvoiceDetail_ConceptId] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])
GO

ALTER TABLE [dbo].[InvoiceDetail] CHECK CONSTRAINT [fkInvoiceDetail_ConceptId]
GO

ALTER TABLE [dbo].[InvoiceDetail]  WITH NOCHECK ADD  CONSTRAINT [fkInvoiceDetail_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([InvoiceId])
GO

ALTER TABLE [dbo].[InvoiceDetail] CHECK CONSTRAINT [fkInvoiceDetail_InvoiceId]
GO


