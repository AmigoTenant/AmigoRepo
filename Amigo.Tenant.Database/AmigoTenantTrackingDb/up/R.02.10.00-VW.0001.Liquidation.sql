
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
   --,CES.Code AS ContractStatusCode
 FROM PaymentPeriod PP      
   INNER JOIN Contract C ON C.ContractId = PP.ContractId      
   INNER JOIN House H ON h.HouseId = PP.HouseId      
   INNER JOIN Tenant T ON T.TenantId = C.TenantId      
   INNER JOIN EntityStatus ES ON ES.EntityStatusId = PP.PaymentPeriodStatusId      
   INNER JOIN GeneralTable GT ON GT.GeneralTableId = PP.PaymentTypeId       
   INNER JOIN Period P ON P.PeriodId = PP.PeriodId       
   INNER JOIN Concept CPTO ON CPTO.ConceptId = PP.ConceptId       
   LEFT JOIN InvoiceDetail INVDET ON INVDET.PaymentPeriodId = PP.PaymentPeriodId  --AND INVDET.TotalAmount > 0  
   LEFT JOIN Invoice INV ON INV.InvoiceId = INVDET.InvoiceId   
   --LEFT JOIN EntityStatus CES ON C.ContractStatusId = CES.EntityStatusId 

   CROSS APPLY   
   (  
  SELECT SUM(ISNULL(TotalAmount, 0)) as TotalInvoice  
  FROM INVOICE I2  
  WHERE I2.ContractId = PP.ContractId and   
    --I2.TenantId = T.TenantId and   
    I2.PeriodId = P.PeriodId and   
    I2.RowStatus = 1  
   ) as TotalInvoice  
  
   CROSS APPLY   
   (  
  SELECT SUM(ISNULL(PP2.PaymentAmount,0)) as TotalIncome  
  FROM PaymentPeriod PP2  
    INNER JOIN Concept C2 ON C2.ConceptId = PP2.ConceptId  
  WHERE PP2.ContractId = PP.ContractId and   
    --PP2.TenantId = T.TenantId and   
    PP2.PeriodId = P.PeriodId and   
    PP2.RowStatus = 1 and   
    CHARINDEX(C2.Code, (SELECT AppSettingValue FROM AppSetting WHERE Code= 'CPTTOFAVTN' AND RowStatus = 1)) = 0  
  
   ) as IncomeConcept  
     
   LEFT JOIN FileRepository FR ON FR.ParentId = INV.InvoiceId  AND FR.EntityCode = 'IN'  
  

--INSERTANDO REGISTRO EN CONTRATO
INSERT INTO dbo.EntityStatus (Code, Name, EntityCode, Sequence, CreatedBy, CreationDate, UpdatedBy, UpdatedDate)
VALUES ('LIQUIDED', 'Liquided', 'CO', 8, 1, GETDATE(), NULL, NULL)


--PROCEDURE

ALTER VIEW [dbo].[vwPaymentPeriod]  
AS  


SELECT PP.PaymentPeriodId,          
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
  ISNULL(PaymentPending.RentPending, 0) AS RentPending,        
  ISNULL(PaymentPending.FinesPending, 0) AS FinesPending,        
  CASE         
  WHEN ISNULL(PaymentPending.LateFeesPending, 0) > 0 THEN PaymentPending.LateFeesPending          
  WHEN DATEDIFF(DD, P.DueDate, GETDATE()) > 0 AND ES.Code = 'PPPENDING' THEN  1 ELSE 0           
  END AS LateFeesPending,         
  ISNULL(PaymentPending.DepositPending, 0) AS DepositPending,        

  --PENDING AMOUNTS  
  ISNULL(PaymentAmountPending.RentAmountPending, 0) AS RentAmountPending,
  ISNULL(PaymentAmountPending.FinesAmountPending, 0) AS FinesAmountPending,
  CASE     
   WHEN PaymentAmountPending.LateFeesAmountPending > 0    
    THEN  PaymentAmountPending.LateFeesAmountPending    
   WHEN ES.Code = 'PPPENDING' AND DATEDIFF(DD, P.DueDate, GETDATE()) > 0    
    THEN  DATEDIFF(DD, P.DueDate, GETDATE()) * LateFeeAmountPerDay.AppSettingValue    
   WHEN ES.Code = 'PPPAYED' AND DATEDIFF(DD, P.DueDate, PP.PaymentDate) > 0 AND ISNULL(LateFeePayed.LateFeePayedCount, 0) = 0    
    THEN DATEDIFF(DD, P.DueDate, GETDATE()) * LateFeeAmountPerDay.AppSettingValue    
   ELSE 0 END LateFeesAmountPending,    
  ISNULL(PaymentAmountPending.DepositAmountPending, 0) AS DepositAmountPending,
  ISNULL(PaymentAmountPending.OnAccountAmountPending,
 0) AS OnAccountAmountPending,

  --FOR DASHBOARDS  
  vwDashBala.TotalExpenseAmount,
  vwDashBala.TotalIncomePaidAmount,
  vwDashBala.TotalIncomePendingAmount,
  vwDashBala.TotalIncomePaidAmount + 
  vwDashBala.TotalIncomePendingAmount AS TotalIncomeAmountByPeriod,

  --PAID AMOUNTS
  ISNULL(PaymentAmountPaid.RentAmountPaid, 0) as RentAmountPaid, 
  ISNULL(PaymentAmountPaid.DepositAmountPaid, 0) as DepositAmountPaid, 
  ISNULL(PaymentAmountPaid.LateFeesAmountPaid, 0) as LateFeesAmountPaid, 
  ISNULL(PaymentAmountPaid.FinesAmountPaid, 0) as FinesAmountPaid, 
  ISNULL(PaymentAmountPaid.OnAccountAmountPaid, 0) as OnAccountAmountPaid

  ,CES.Code AS ContractStatusCode
  ,ID.InvoiceId
 FROM PaymentPeriod PP        
	INNER JOIN Contract C ON C.ContractId = PP.ContractId        
	INNER JOIN House H ON h.HouseId = PP.HouseId        
	INNER JOIN Tenant T ON T.TenantId = C.TenantId        
	INNER JOIN EntityStatus ES ON ES.EntityStatusId = PP.PaymentPeriodStatusId        
	INNER JOIN Concept CPTO ON CPTO.ConceptId = PP.ConceptId AND CPTO.Code = 'RENT'        
	INNER JOIN Period P ON P.PeriodId = PP.PeriodId         
    LEFT JOIN EntityStatus CES ON C.ContractStatusId = CES.EntityStatusId 
	LEFT JOIN InvoiceDetail ID on ID.PaymentPeriodId = PP.PaymentPeriodId
  CROSS APPLY        
  (        
   SELECT   
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'RENT' THEN 1 ELSE 0 END, 0)) AS RentPending,        
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'FINE' THEN 1 ELSE 0 END, 0)) AS FinesPending,        
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'LATEFEE' THEN 1 ELSE 0 END, 0)) AS LateFeesPending,        
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'DEPOSIT' THEN 1 ELSE 0 END, 0)) AS DepositPending,      
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'ONACCOUNT' THEN 1 ELSE 0 END, 0)) AS OnAccountPending      
   FROM PaymentPeriod PP1        
     INNER JOIN CONCEPT CPTO1 ON CPTO1.ConceptId = PP1.ConceptId              
     INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = 
PP1.PaymentPeriodStatusId        
   WHERE PP1.ContractId = PP.ContractId  AND         
     PP1.PeriodId = PP.PeriodId  AND         
     CPTO1.Code in ('RENT','DEPOSIT', 'LATEFEE', 'FINE', 'ONACCOUNT') AND          
     ES1.Code = 'PPPENDING'        
 
 ) AS PaymentPending        
        
  CROSS APPLY          
  (          
   SELECT COUNT(1) AS LateFeePayedCount    
   FROM PaymentPeriod PP1          
     INNER JOIN CONCEPT CPTO1 ON CPTO1.ConceptId = PP1.ConceptId       
     INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = PP1.PaymentPeriodStatusId          
   WHERE PP1.ContractId = PP.ContractId  AND           
     PP1.PeriodId = PP.PeriodId  AND      
     CPTO1.Code = 'LATEFEE' AND          
     ES1.Code = 'PPPAYED'          
  ) AS LateFeePayed      
       
       
  CROSS APPLY  
  (            
   SELECT       
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'RENT' THEN PaymentAmount ELSE 0 END, 0)) AS RentAmountPending,          
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'FINE' THEN PaymentAmount ELSE 0 END, 0)) AS FinesAmountPending,          
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'LATEFEE' THEN PaymentAmount ELSE 0 END, 0)) AS LateFeesAmountPending,          
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'DEPOSIT' THEN PaymentAmount ELSE 0 END, 0)) 
AS DepositAmountPending,        
     SUM(ISNULL(CASE WHEN CPTO1.Code = 'ONACCOUNT' THEN PaymentAmount ELSE 0 END, 0)) AS OnAccountAmountPending        
   FROM PaymentPeriod PP1          
     INNER JOIN CONCEPT CPTO1 ON CPTO1.ConceptId = PP1.ConceptId 

     INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = PP1.PaymentPeriodStatusId        
   WHERE PP1.ContractId = PP.ContractId  AND           
     PP1.PeriodId = PP.PeriodId  AND           
     CPTO1.Code in ('RENT','DEPOSIT', 'LATEFEE', 'FINE', 'ONACCOUNT') AND
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
