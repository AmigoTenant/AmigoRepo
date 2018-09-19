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
	,I.InvoiceId 
   ,P.Code as PeriodCode    
   ,H.Name as HouseName    
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

GO
