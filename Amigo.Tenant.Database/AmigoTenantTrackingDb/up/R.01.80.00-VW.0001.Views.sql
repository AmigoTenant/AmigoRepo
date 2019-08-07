
IF OBJECT_ID ('dbo.vwPaymentPeriodInvoicePrint') IS NOT NULL
	DROP VIEW dbo.vwPaymentPeriodInvoicePrint
GO

CREATE VIEW [dbo].[vwPaymentPeriodInvoicePrint]    
AS    

SELECT  I.InvoiceNo 
   ,ISNULL(TotalInvoice.TotalInvoice, 0) AS TotalInvoice
   ,ISNULL(IncomeConcept.TotalIncome, 0) AS TotalIncome
	,I.InvoiceId 
   ,P.Code as PeriodCode    
   ,H.Name as HouseName    
   ,T.TenantId
   ,P.PeriodId
   ,T.FullName as TenantFullName    
   ,I.InvoiceDate  
   ,I.CustomerName  
	,I.PaymentOperationNo  
	,I.BankName  
	,I.PaymentOperationDate  
   ,I.Comment  
   ,C.ContractCode  
   ,I.TotalRent    
   ,I.TotalDeposit    
   ,I.TotalLateFee    
   ,I.TotalService    
   ,I.TotalFine    
   ,I.TotalOnAcount    
   ,I.TotalAmount    
   ,ID.PaymentPeriodId          
   ,I.ContractId    
   ,GT.Value as PaymentTypeValue    
   ,GT.Code as PaymentTypeCode      
   ,CPTO.Description as PaymentDescription    
   ,ID.TotalAmount as PaymentAmount  
   ,P.DueDate as periodDueDate    
   ,GT.Sequence as PaymentTypeSequence    
   ,CHARINDEX(CPTO.Code, (SELECT AppSettingValue FROM AppSetting WHERE Code= 'CPTTOFAVTN' AND RowStatus = 1)) as IsTenantFavorable
   ,US.UserName
   ,T.Email
   ,FR.FileRepositoryId
 FROM Invoice I    
   INNER JOIN InvoiceDetail ID ON I.InvoiceId = ID.InvoiceId    
   INNER JOIN PaymentPeriod PP ON ID.PaymentPeriodId = PP.PaymentPeriodId    
   INNER JOIN Contract C ON C.ContractId = I.ContractId    
   INNER JOIN House H ON h.HouseId = C.HouseId    
   INNER JOIN Tenant T ON T.TenantId = C.TenantId    
   LEFT JOIN GeneralTable GT ON GT.GeneralTableId = I.PaymentTypeId   
   INNER JOIN Period P ON P.PeriodId = PP.PeriodId     
   INNER JOIN Concept CPTO ON CPTO.ConceptId = ID.ConceptId            
   LEFT JOIN sec.[user] US ON US.userId = I.CreatedBy       

   CROSS APPLY 
   (
		SELECT	SUM(ISNULL(TotalAmount, 0)) as TotalInvoice
		FROM	INVOICE I2
		WHERE	I2.TenantId = T.TenantId and 
				I2.PeriodId = P.PeriodId and 
				I2.RowStatus = 1
   ) as TotalInvoice

   CROSS APPLY 
   (
		SELECT	SUM(ISNULL(PP2.PaymentAmount,0)) as TotalIncome
		FROM	PaymentPeriod PP2
				INNER JOIN Concept C2 ON C2.ConceptId = PP2.ConceptId
		WHERE	PP2.TenantId = T.TenantId and 
				PP2.PeriodId = P.PeriodId and 
				PP2.RowStatus = 1 and 
				CHARINDEX(C2.Code, (SELECT AppSettingValue FROM AppSetting WHERE Code= 'CPTTOFAVTN' AND RowStatus = 1)) = 0

   ) as IncomeConcept
   
   LEFT JOIN FileRepository FR ON FR.ParentId = I.InvoiceId  AND FR.EntityCode = 'IN'
GO


IF OBJECT_ID ('dbo.vwPaymentPeriodByContract') IS NOT NULL
	DROP VIEW dbo.vwPaymentPeriodByContract
GO

CREATE VIEW [dbo].[vwPaymentPeriodByContract]  
AS  
   
 
SELECT PP.PaymentPeriodId      ,  
   PP.PeriodId             ,    
   P.Code as PeriodCode,    
   H.Name as HouseName,    
   T.FullName as TenantFullName,    
   GT.Value as PaymentTypeValue,    
   GT.Code as PaymentTypeCode,      
   CPTO.Description as PaymentDescription,    
   PaymentAmount        ,    
   ES.Name as PaymentPeriodStatusName,    
   ES.Code as PaymentPeriodStatusCode,    
    
   PP.ConceptId,    
   PP.ContractId,    
   PP.TenantId,    
   PP.PaymentTypeId,    
   PP.DueDate,    
   PP.RowStatus,    
   PP.CreatedBy,    
   PP.CreationDate,    
   PP.UpdatedBy,    
   PP.UpdatedDate,    
   PP.PaymentPeriodStatusId,    
   PATINDEX('%'+CPTO.Code+'%', (SELECT AppSettingValue FROM AppSetting WHERE Code = 'CPTSREQPAY' AND RowStatus = 1)) AS IsRequired,    
   P.DueDate as periodDueDate,    
   GT.Sequence as PaymentTypeSequence,    
   PP.PaymentDate,    
   INVDET.InvoiceId,  
   INVDET.InvoiceDetailId,  
   INV.InvoiceNo,  
   INV.InvoiceDate , 
   T.Email,
   CPTO.Code as ConceptCode,
   CHARINDEX(CPTO.Code, (SELECT AppSettingValue FROM AppSetting WHERE Code= 'CPTTOFAVTN' AND RowStatus = 1)) as IsTenantFavorable
   ,ISNULL(TotalInvoice.TotalInvoice, 0) AS TotalInvoice
   ,ISNULL(IncomeConcept.TotalIncome, 0) AS TotalIncome
   ,PP.Comment
   ,PP.ReferenceNo
   ,FR.FileRepositoryId
 FROM PaymentPeriod PP    
   INNER JOIN Contract C ON C.ContractId = PP.ContractId    
   INNER JOIN House H ON h.HouseId = C.HouseId    
   INNER JOIN Tenant T ON T.TenantId = C.TenantId    
   INNER JOIN EntityStatus ES ON ES.EntityStatusId = PP.PaymentPeriodStatusId    
   INNER JOIN GeneralTable GT ON GT.GeneralTableId = PP.PaymentTypeId     
   INNER JOIN Period P ON P.PeriodId = PP.PeriodId     
   INNER JOIN Concept CPTO ON CPTO.ConceptId = PP.ConceptId     
   LEFT JOIN InvoiceDetail INVDET ON INVDET.PaymentPeriodId = PP.PaymentPeriodId  AND INVDET.TotalAmount > 0
   LEFT JOIN Invoice INV ON INV.InvoiceId = INVDET.InvoiceId 

   CROSS APPLY 
   (
		SELECT	SUM(ISNULL(TotalAmount, 0)) as TotalInvoice
		FROM	INVOICE I2
		WHERE	I2.TenantId = T.TenantId and 
				I2.PeriodId = P.PeriodId and 
				I2.RowStatus = 1
   ) as TotalInvoice

   CROSS APPLY 
   (
		SELECT	SUM(ISNULL(PP2.PaymentAmount,0)) as TotalIncome
		FROM	PaymentPeriod PP2
				INNER JOIN Concept C2 ON C2.ConceptId = PP2.ConceptId
		WHERE	PP2.TenantId = T.TenantId and 
				PP2.PeriodId = P.PeriodId and 
				PP2.RowStatus = 1 and 
				CHARINDEX(C2.Code, (SELECT AppSettingValue FROM AppSetting WHERE Code= 'CPTTOFAVTN' AND RowStatus = 1)) = 0

   ) as IncomeConcept
   
   LEFT JOIN FileRepository FR ON FR.ParentId = INV.InvoiceId  AND FR.EntityCode = 'IN'
GO
