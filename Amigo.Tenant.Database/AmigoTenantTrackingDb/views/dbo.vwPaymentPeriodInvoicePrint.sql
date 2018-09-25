IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwPaymentPeriodInvoicePrint]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwPaymentPeriodInvoicePrint]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwPaymentPeriodInvoicePrint]    
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

GO