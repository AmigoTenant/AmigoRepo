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
   ,PP.HouseId
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


   go

   ALTER VIEW [dbo].[vwPaymentPeriod]  

AS  

SELECT PaymentPeriodId,          
  PP.ContractId,        
  PP.TenantId,        
  PP.PeriodId,        
  P.Code as PeriodCode,        
  PaymentAmount,        
  PP.DueDate,        
  PP.PaymentDate,      
  PaymentPeriodStatusId,        
  C.ContractCode,        
  H.Name as HouseName,        
  C.HouseId,            
  T.FullName as TenantFullName,          
  ES.Code as PaymentPeriodStatusCode,        
  ES.Name as PaymentPeriodStatusName,   

  --PENDING ACCOUNTS  
  ISNULL(PaymentPending.ServicesPending, 0) AS ServicesPending,        
  ISNULL(PaymentPending.FinesPending, 0) AS FinesPending,        
  CASE         
  WHEN ISNULL(PaymentPending.LateFeesPending, 0) > 0 THEN PaymentPending.LateFeesPending          
  WHEN DATEDIFF(DD, P.DueDate, GETDATE()) > 0 AND ES.Code = 'PPPENDING' THEN  1 ELSE 0           
  END AS LateFeesPending,         
  ISNULL(PaymentPending.DepositPending, 0) AS DepositPending,        

  --PENDING AMOUNTS  
  PaymentAmountPending.ServicesAmountPending,        
  PaymentAmountPending.FinesAmountPending,        
  CASE     
   WHEN PaymentAmountPending.LateFeesAmountPending > 0    
    THEN  PaymentAmountPending.LateFeesAmountPending    
   WHEN ES.Code = 'PPPENDING' AND DATEDIFF(DD, P.DueDate, GETDATE()) > 0    
    THEN  DATEDIFF(DD, P.DueDate, GETDATE()) * LateFeeAmountPerDay.AppSettingValue    
   WHEN ES.Code = 'PPPAYED' AND DATEDIFF(DD, P.DueDate, PP.PaymentDate) > 0 AND ISNULL(LateFeePayed.LateFeePayedCount, 0) = 0    
    THEN DATEDIFF(DD, P.DueDate, GETDATE()) * LateFeeAmountPerDay.AppSettingValue    
   ELSE 0 END LateFeesAmountPending,    

  PaymentAmountPending.DepositAmountPending,        
  PaymentAmountPending.OnAccountAmountPending,

  --PAID AMOUNTS


  --FOR DASHBOARDS  
  vwDashBala.TotalExpenseAmount,
  vwDashBala.TotalIncomePaidAmount,
  vwDashBala.TotalIncomePendingAmount,
  vwDashBala.TotalIncomePaidAmount + 
  vwDashBala.TotalIncomePendingAmount AS TotalIncomeAmountByPeriod,

  ISNULL(PaymentAmountPaid.RentAmountPaid, 0) as RentAmountPaid, 
  ISNULL(PaymentAmountPaid.DepositAmountPaid, 0) as DepositAmountPaid, 
  ISNULL(PaymentAmountPaid.LateFeesAmountPaid, 0) as LateFeesAmountPaid, 
  ISNULL(PaymentAmountPaid.FinesAmountPaid, 0) as FinesAmountPaid, 
  ISNULL(PaymentAmountPaid.OnAccountAmountPaid, 0) as OnAccountAmountPaid

 FROM PaymentPeriod PP        
  INNER JOIN Contract C ON C.ContractId = PP.ContractId        
  INNER JOIN House H ON h.HouseId = PP.HouseId        
  INNER JOIN Tenant T ON T.TenantId = C.TenantId        
  INNER JOIN EntityStatus ES ON ES.EntityStatusId = PP.PaymentPeriodStatusId        
  INNER JOIN GeneralTable GT ON GT.GeneralTableId = PP.PaymentTypeId AND GT.Code = 'RENT'        
  INNER JOIN Period P ON P.PeriodId = PP.PeriodId         
        
  CROSS APPLY        
  (        
   SELECT   
     SUM(ISNULL(CASE WHEN GT1.Code = 'SERVICE' THEN 1 ELSE 0 END, 0)) AS ServicesPending,        
     SUM(ISNULL(CASE WHEN GT1.Code = 'FINE' THEN 1 ELSE 0 END, 0)) AS FinesPending,        

    SUM(ISNULL(CASE WHEN GT1.Code = 'LATEFEE' THEN 1 ELSE 0 END, 0)) AS LateFeesPending,        
     SUM(ISNULL(CASE WHEN GT1.Code = 'DEPOSIT' THEN 1 ELSE 0 END, 0)) AS DepositPending,      
     SUM(ISNULL(CASE WHEN GT1.Code = 'ONACCOUNT' THEN 1 ELSE 0 END, 0)) AS OnAccountPending      
   FROM PaymentPeriod PP1        
     INNER JOIN GENERALTABLE GT1 ON GT1.GeneralTableId = PP1.PaymentTypeId        
     INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = PP1.PaymentPeriodStatusId        
   WHERE PP1.ContractId = PP.ContractId  AND         
     PP1.PeriodId = PP.PeriodId  AND         
     GT1.Code not in ('RENT') AND        
     ES1.Code = 'PPPENDING'        
  ) AS PaymentPending        
        
  CROSS APPLY          
  (          
   SELECT COUNT(1) AS LateFeePayedCount    
   FROM PaymentPeriod PP1          
     INNER JOIN GENERALTABLE GT1 ON GT1.GeneralTableId = PP1.PaymentTypeId          
     INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = PP1.PaymentPeriodStatusId          
   WHERE PP1.ContractId = PP.ContractId  AND           
     PP1.PeriodId = PP.PeriodId  AND      
     GT1.Code = 'LATEFEE' AND          
     ES1.Code = 'PPPAYED'          
  ) AS LateFeePayed      
       
       
  CROSS APPLY  
  (            
   SELECT       
     SUM(ISNULL(CASE WHEN GT1.Code = 'SERVICE' THEN PaymentAmount ELSE 0 END, 0)) AS ServicesAmountPending,          
     SUM(ISNULL(CASE WHEN GT1.Code = 'FINE' THEN PaymentAmount ELSE 0 END, 0)) AS FinesAmountPending,          
     SUM(ISNULL(CASE WHEN GT1.Code = 'LATEFEE' THEN PaymentAmount ELSE 0 END, 0)) AS LateFeesAmountPending,          
     SUM(ISNULL(CASE WHEN GT1.Code = 'DEPOSIT' THEN PaymentAmount ELSE 0 END, 0)) AS DepositAmountPending,        
     SUM(ISNULL(CASE WHEN GT1.Code = 'ONACCOUNT' THEN PaymentAmount ELSE 0 END, 0)) AS OnAccountAmountPending        
   FROM PaymentPeriod PP1          
     INNER JOIN GENERALTABLE GT1 ON GT1.GeneralTableId = PP1.PaymentTypeId          
     INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = PP1.PaymentPeriodStatusId        
   WHERE PP1.ContractId = PP.ContractId  AND           
     PP1.PeriodId = PP.PeriodId  AND           
     GT1.Code not in ('RENT') AND          
     ES1.Code = 'PPPENDING'          
  ) AS PaymentAmountPending  
  
  CROSS APPLY  
  (            
   SELECT       
	 SUM(ISNULL(CASE WHEN CPTO1.Code = 'RENT' THEN PaymentAmount ELSE 0 END, 0)) AS RentAmountPaid,   
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'DEPOSIT' THEN PaymentAmount ELSE 0 END, 0)) AS DepositAmountPaid,          
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'FINE' THEN PaymentAmount ELSE 0 END, 0)) AS FinesAmountPaid,          
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'LATEFEE' THEN PaymentAmount ELSE 0 END, 0)) AS LateFeesAmountPaid,         
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'ONACCOUNT' THEN PaymentAmount ELSE 0 END, 0)) AS OnAccountAmountPaid        
   FROM PaymentPeriod PP1          
     INNER JOIN CONCEPT CPTO1 ON CPTO1.ConceptId = PP1.ConceptId          
     INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = PP1.PaymentPeriodStatusId        
   WHERE PP1.ContractId = PP.ContractId  AND           
     PP1.PeriodId = PP.PeriodId  AND           
     CPTO1.Code in ('RENT','DEPOSIT', 'LATEFEE', 'FINE', 'ONACCOUNT') AND          
     ES1.Code = 'PPPAYED'          
  ) AS PaymentAmountPaid  

  
  OUTER APPLY  
  (  
   SELECT AppSettingValue FROM AppSetting WHERE Code = 'LATEFEEXDY'  
  ) AS LateFeeAmountPerDay  
  

Left join vwDashboardBalance vwDashBala on vwDashBala.PeriodCode = P.Code


go


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
   ,T.Email
   ,FR.FileRepositoryId
 FROM Invoice I    
   INNER JOIN InvoiceDetail ID ON I.InvoiceId = ID.InvoiceId    
   INNER JOIN PaymentPeriod PP ON ID.PaymentPeriodId = PP.PaymentPeriodId    
   INNER JOIN Contract C ON C.ContractId = I.ContractId    
   INNER JOIN House H ON h.HouseId = PP.HouseId    
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
