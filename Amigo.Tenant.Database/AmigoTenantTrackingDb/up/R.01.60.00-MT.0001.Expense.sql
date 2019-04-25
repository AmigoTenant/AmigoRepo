IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'ConceptId' 
			AND sc.id = so.id AND so.name = 'Expense') 
BEGIN
	ALTER TABLE [Expense]
	ADD [ConceptId] INT NULL

ALTER TABLE [dbo].[Expense]  WITH NOCHECK ADD  CONSTRAINT [fkExpense_ConceptId] FOREIGN KEY([ConceptId])
REFERENCES [dbo].[Concept] ([ConceptId])

END 
GO


IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'PrecedenceId' 
			AND sc.id = so.id AND so.name = 'GeneralTable') 
BEGIN
	ALTER TABLE [GeneralTable]
	ADD [PrecedenceId] INT NULL

ALTER TABLE [dbo].[GeneralTable]  WITH NOCHECK ADD  CONSTRAINT [fkGeneralTable_PrecedenceId] FOREIGN KEY([PrecedenceId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])

END
GO

IF not exists(Select * from GeneralTable Where Code='BALANCE' ) 
BEGIN
INSERT INTO generaltable VALUES('BALANCE', 'Balance', 'Balance', 1, 1, 1,1, getdate(), 1, getdate(), null)
UPDATE generaltable set PrecedenceId = 68 where GeneralTableId in (1,13,67)
END 
go

/*- AGREGAR LOS CONCEPTOS QUE EDGAR ENVIARA COMO TABLENAME 'CONCEPTYTYPE'
  - LOS QUE EXISTIAN COMO CONECPTYPE DEBERAN TENER OTRO NOMBRE COMO BALANCETYPE */


/*BUSINESS PARTNER*/
alter table invoice drop constraint fkInvoice_BusinessPartnerId
alter table servicehouse drop constraint fkService_BusinessPartnerId


--AGREGANDO BUSINESSPARTNER, BPSucursal, BPSucursalContact

GO
ALTER TABLE [dbo].[BPSucursalContact] DROP CONSTRAINT [fkBPSucursalContact_BusinessPartnerId]
GO
ALTER TABLE [dbo].[BPSucursalContact] DROP CONSTRAINT [fkBPSucursalContact_BPSucursalId]
GO
ALTER TABLE [dbo].[Expense] DROP CONSTRAINT [fkExpense_BusinessPartnerId]
GO
DROP TABLE [dbo].[BusinessPartner]
GO
DROP TABLE [dbo].[BPSucursalContact]
GO
DROP TABLE [dbo].[BPSucursal]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BPSucursal](
	[BPSucursalId] [int] IDENTITY(1,1) NOT NULL,
	[BusinessPartnerId] [int] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[Address] [nvarchar](150) NULL,
	[EMail] [nvarchar](100) NULL,
	[Phone1] [nvarchar](20) NULL,
	[Phone2] [nvarchar](20) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_BPSucursal] PRIMARY KEY CLUSTERED 
(
	[BPSucursalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BPSucursalContact](
	[BPSucursalContactId] [int] NOT NULL,
	[BusinessPartnerId] [int] NOT NULL,
	[BPSucursalId] [int] NOT NULL,
	[Name] [nvarchar](150) NULL,
	[EMail] [nvarchar](100) NULL,
	[Phone] [nvarchar](20) NULL,
	[IsPrimary] [bit] NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_BPSucursalContact] PRIMARY KEY CLUSTERED 
(
	[BPSucursalContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BusinessPartner](
	[BusinessPartnerId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [varchar](10) NULL,
	[Name] [varchar](150) NULL,
	[BPTypeId] [int] NOT NULL,
	[SIN] [nvarchar](12) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_BusinessPartner] PRIMARY KEY CLUSTERED 
(
	[BusinessPartnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[BPSucursalContact]  WITH NOCHECK ADD  CONSTRAINT [fkBPSucursalContact_BPSucursalId] FOREIGN KEY([BPSucursalId])
REFERENCES [dbo].[BPSucursal] ([BPSucursalId])
GO
ALTER TABLE [dbo].[BPSucursalContact] CHECK CONSTRAINT [fkBPSucursalContact_BPSucursalId]
GO
ALTER TABLE [dbo].[BPSucursalContact]  WITH NOCHECK ADD  CONSTRAINT [fkBPSucursalContact_BusinessPartnerId] FOREIGN KEY([BusinessPartnerId])
REFERENCES [dbo].[BusinessPartner] ([BusinessPartnerId])
GO
ALTER TABLE [dbo].[BPSucursalContact] CHECK CONSTRAINT [fkBPSucursalContact_BusinessPartnerId]
GO

ALTER TABLE [dbo].[BusinessPartner]  WITH NOCHECK ADD  CONSTRAINT [fkBusinessPartner_BPTypeId] FOREIGN KEY([BPTypeId])
REFERENCES [dbo].[GeneralTable] ([GeneralTableId])


--AGREGANDO COLUMNA BUSINESSPARTNERID AL EXPENSE
IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'BusinessPartnerId' 
			AND sc.id = so.id AND so.name = 'Expense') 
	ALTER TABLE [Expense]
	ADD [BusinessPartnerId] INT NULL

ALTER TABLE [dbo].[Expense]  WITH NOCHECK ADD  CONSTRAINT [fkExpense_BusinessPartnerId] FOREIGN KEY([BusinessPartnerId])
REFERENCES [dbo].[BusinessPartner] ([BusinessPartnerId])
GO

--AGREGANDO FILE REPOSITORY
ALTER TABLE [dbo].[FileRepository] DROP CONSTRAINT [fkFileRepository_EntityCode]
GO
DROP TABLE [dbo].[FileRepository]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FileRepository](
	[FileRepositoryId] [INT] IDENTITY(1,1) NOT NULL,
	[EntityCode] [varchar](3) NULL,
	[ParentId] [INT] NULL,
	[Name] [nvarchar](256) NULL,
	[ContentType] [nvarchar](256) NULL,
	[FileExtension] [nvarchar](50) NULL,
	[utMediaFile] [varbinary](max) NULL,
	[RowStatus] [bit] NULL,
	[CreatedBy] [nvarchar](64) NULL,
	[CreationDate] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](64) NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[AdditionalInfo] [nvarchar](255) NULL,
 CONSTRAINT [pkFileRepository] PRIMARY KEY CLUSTERED 
(
	[FileRepositoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[FileRepository]  WITH CHECK ADD  CONSTRAINT [fkFileRepository_EntityCode] FOREIGN KEY([EntityCode])
REFERENCES [dbo].[EntityStatusTable] ([Code])
GO
ALTER TABLE [dbo].[FileRepository] CHECK CONSTRAINT [fkFileRepository_EntityCode]
GO


/* %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
	GENERAL TABLE FIX DATA PARA AGREGAR EXPENSES Y CONCEPTOS
   %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% */
UPDATE GENERALTABLE SET VALUE = 'Servicios' where code = 'CHARGE'
if not exists(Select * from GeneralTable Where Code='EXPRENTA01' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('EXPRENTA01','ConceptType','Expense Renta','7','0','1','1', GETDATE() ,'1',GETDATE())
if not exists(Select * from GeneralTable Where Code='EXPDEVDEPO' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('EXPDEVDEPO','ConceptType','Expense Devolucion Depositos','8','0','1','1', GETDATE() ,'1',GETDATE())
if not exists(Select * from GeneralTable Where Code='EXPOFICINA' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('EXPOFICINA','ConceptType','Expense Oficina','9','0','1','1', GETDATE() ,'1',GETDATE())
if not exists(Select * from GeneralTable Where Code='EXPVEHICUL' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('EXPVEHICUL','ConceptType','Expense Vehiculos','10','0','1','1', GETDATE() ,'1',GETDATE())
if not exists(Select * from GeneralTable Where Code='EXPSALARIO' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('EXPSALARIO','ConceptType','Expense Salarios','11','0','1','1', GETDATE() ,'1',GETDATE())
if not exists(Select * from GeneralTable Where Code='EXPLIMPIEZ' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('EXPLIMPIEZ','ConceptType','Expense Limpieza y Mantenimiento','12','0','1','1', GETDATE() ,'1',GETDATE())
if not exists(Select * from GeneralTable Where Code='EXPOTROS01' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) values ('EXPOTROS01','ConceptType','Otros Expenses','13','0','1','1', GETDATE() ,'1',GETDATE())
delete from GeneralTable where code = 'PENALTY'

alter table Concept alter column PayTypeId int null


DECLARE @Id INT = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOFICINA' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOFRENTA1' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOFRENTA1','Renta', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOFICINA' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOFINTERN' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOFINTERN','Internet', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOFICINA' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOFUTOFIC' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOFUTOFIC','Utiles de Oficina', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOFICINA' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOFSWQUIC' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOFSWQUIC','Quicbooks', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOFICINA' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOFSWTREL' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOFSWTREL','Trello', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOFICINA' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOFSWTEAV' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOFSWTEAV','Teamviewer', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOFICINA' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOFLUNOFI' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOFLUNOFI','Lunch Oficina', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPVEHICUL' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXVHSGTOYO' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXVHSGTOYO','Seguro Toyota', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPVEHICUL' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXVHSGCAMI' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXVHSGCAMI','Seguro Camion', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPVEHICUL' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXVHSGHOND' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXVHSGHOND','Seguro Honda', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPVEHICUL' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXVHCMTOYO' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXVHCMTOYO','Combustible Toyota', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPVEHICUL' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXVHCMCAMI' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXVHCMCAMI','Combustible Camion', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPVEHICUL' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXVHCMHOND' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXVHCMHOND','Combustible Honda', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPVEHICUL' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXVHCUTOYO' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXVHCUTOYO','Cuota Mensual Toyota', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPVEHICUL' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXVHMANTEN' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXVHMANTEN','Mantenimiento', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPSALARIO' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXSAEDGAR1' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXSAEDGAR1','Edgar', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPSALARIO' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXSASORAYA' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXSASORAYA','Soraya', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPSALARIO' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXSAMARIAD' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXSAMARIAD','Maria David', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPSALARIO' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXSACLAUDI' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXSACLAUDI','Claudia', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPSALARIO' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXSAANDREA' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXSAANDREA','Andrea', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPSALARIO' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXSAMARGAR' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXSAMARGAR','Margarita', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPSALARIO' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXSARICARD' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXSARICARD','Ricardo', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPSALARIO' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXSACARLOS' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXSACARLOS','Carlos', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPLIMPIEZ' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXLMSVCLIM' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXLMSVCLIM','Servicios de Limpieza', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPLIMPIEZ' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXLMREPMEN' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXLMREPMEN','Reparaciones Menores', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPLIMPIEZ' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXLMREPPLO' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXLMREPPLO',' Reparaciones Plomeria', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPLIMPIEZ' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXLMREPELE' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXLMREPELE','Reparaciones Electricas', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPLIMPIEZ' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXLMREPUES' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXLMREPUES','Repuestos', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPLIMPIEZ' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXLMFURNIT' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXLMFURNIT','Furniture', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPLIMPIEZ' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXLMSFUMIG' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXLMSFUMIG','Fumigacion', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'CHARGE' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='SVCINTERNE' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('SVCINTERNE','Internet', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'CHARGE' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='SVCGAS0001' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('SVCGAS0001','Gas', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'CHARGE' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='SVCLUZ0001' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('SVCLUZ0001','Luz', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOTROS01' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOTGARCOL' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOTGARCOL','Garbage Collection', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOTROS01' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOTDONACI' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOTDONACI','Donación', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())

SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'EXPOTROS01' and tableName = 'ConceptType') 
if not exists(Select * from Concept Where Code         ='EXOTCOMISI' )  Insert Into Concept(Code         ,  Description  ,  TypeId       ,  RowStatus    ,  Remark       ,  PayTypeId    ,  ConceptAmount,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate  
) values ('EXOTCOMISI','Comisión', @Id, '1','', NULL, NULL, '1', GETDATE(), '1', GETDATE())


/*GENRAL TABLE: BUSINESS PARTNER TYPE*/

if not exists(Select * from GeneralTable Where Code='BPVENDOR01' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) 
values ('BPVENDOR01','BPType','Vendor','1','0','1','1', GETDATE() ,'1',GETDATE())


if not exists(Select * from GeneralTable Where Code='BPCUSTOMER' ) Insert into GeneralTable ( Code           ,TableName, Value, Sequence, ByDefault,  RowStatus, CreatedBy      ,CreationDate   ,UpdatedBy      ,UpdatedDate  ) 
values ('BPCUSTOMER','BPType','Customer','1','0','1','1', GETDATE() ,'1',GETDATE())

GO

/*BUSINESS PARTNER */

declare @Id INT
SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'BPVENDOR01' and tableName = 'BPType') 
if not exists(Select * from BusinessPartner Where Name         ='Proveedor Luz' )  Insert Into BusinessPartner(Name,  BPTypeId, SIN, RowStatus,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate) 
values ('Proveedor Luz', @Id , '123456789', 1, '1', GETDATE(), '1', GETDATE())

GO

declare @Id INT
SET @Id = (Select GeneralTableId FROM GeneralTable Where Code = 'BPCUSTOMER' and tableName = 'BPType') 
if not exists(Select * from BusinessPartner Where Name         ='ABC CUSTOMER' )  Insert Into BusinessPartner(Name,  BPTypeId, SIN, RowStatus,  CreatedBy    ,  CreationDate ,  UpdatedBy    ,  UpdatedDate) 
values ('ABC CUSTOMER', @Id , '123456798', 1, '1', GETDATE(), '1', GETDATE())

GO

/*agregando fEATURE EN RENTAL APLICATION*/

IF NOT EXISTS (select  sc.name from syscolumns sc, sysobjects so WHERE sc.name = 'Feature' 
			AND sc.id = so.id AND so.name = 'RentalApplication') 
BEGIN
	ALTER TABLE [RentalAPplication]
	ADD [Feature] NVARCHAR(80) NULL
END 
GO