ALTER VIEW [dbo].[vwPaymentPeriodByContract]  
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
   ,PP.HouseId
   ,P.Sequence as PeriodSequence
 FROM PaymentPeriod PP    
   INNER JOIN Contract C ON C.ContractId = PP.ContractId    
   INNER JOIN House H ON h.HouseId = PP.HouseId    
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