/*DashBoard Balance Ingresos versus Gastos*/

IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwDashboardBalance]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwDashboardBalance]  AS SELECT 1 AS X'
GO

alter VIEW [dbo].[vwDashboardBalance]  
  
AS  
    
Select p.Code as PeriodCode, substring(p.Code, 1, 4) as Anio, 
		ISNULL(Income.TotalIncomePaidAmount, 0) AS TotalIncomePaidAmount, 
		ISNULL(Expense.TotalExpenseAmount, 0) AS TotalExpenseAmount,
		ISNULL(IncomePending.TotalIncomePendingAmount, 0) AS TotalIncomePendingAmount   
From Period p  
  left Join  
  (  
   ------------------------------------------------  
   -- QUERY TODOS LOS CONCEPTOS DE INGRESOS PAGADO 
   ------------------------------------------------  
   select p.Code as PeriodCode, sum(id.TotalAmount)  as TotalIncomePaidAmount  
   from Invoice I   
     inner join InvoiceDetail id on i.InvoiceId = id.InvoiceDetailId  
     inner join EntityStatus es on es.EntityStatusId = i.InvoiceStatusId  
     inner join Period p on p.PeriodId = i.PeriodId  
     inner join Concept c on id.ConceptId = c.ConceptId --and c.Code in ('RENT','DEPOSIT','SERVICE','FINE','LATEFEE')  
     outer Apply (SELECT Code, AppSettingValue FROM APPSETTING WHERE code= 'DBINCCPTOS') OA  
   where es.Code = 'INPAYED' and   
     CHARINDEX(c.Code, AppSettingValue)>0 --and code= 'DBINCCPTOS'   
   group by p.Code  
    
  ) Income on p.Code = Income.PeriodCode  
  
  left Join  
  (  
   -----------------------------------------  
   -- QUERY TODOS LOS EXPENSES REGISTRADOS SIN INCLUIR LOS MIGRADOS 
   -----------------------------------------  
   select p.Code as PeriodCode, sum(ed.TotalAmount) as TotalExpenseAmount  
   from Expense e   
     inner join ExpenseDetail ed on e.ExpenseId = ed.ExpenseId  
     inner join GeneralTable gt on ed.ExpenseDetailStatusId = gt.GeneralTableId  
     inner join Period p on p.PeriodId = e.PeriodId  
   where gt.Code NOT IN ('EXMIGRATED')  
   group by p.Code  
    
  ) Expense on p.code = expense.PeriodCode  
  
  left Join  
  (  
   -----------------------------------------  
   -- QUERY TODOS LOS INGRESOS PENDIENTES
   -----------------------------------------  
   select p.Code as PeriodCode, sum(PP.PaymentAmount)  as TotalIncomePendingAmount  
   from PaymentPeriod PP   
     inner join EntityStatus es on es.EntityStatusId = pp.PaymentPeriodStatusId  
     inner join Period p on p.PeriodId = pp.PeriodId  
   where es.Code = 'PPPENDING' 
   group by p.Code  
    
  ) IncomePending on p.Code = IncomePending.PeriodCode 
	go

/*Payment Period*/

IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwPaymentPeriod]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwPaymentPeriod]  AS SELECT 1 AS X'
GO


alter VIEW [dbo].[vwPaymentPeriod]  
  
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
   WHEN ES.Code = 'PPPENDING' AND DATEDIFF(DD, P.DueDate, GETDATE()) > 0    
    THEN  DATEDIFF(DD, P.DueDate, GETDATE()) * LateFeeAmountPerDay.AppSettingValue    
   WHEN ES.Code = 'PPPAYED' AND DATEDIFF(DD, P.DueDate, PP.PaymentDate) > 0 AND ISNULL(LateFeePayed.LateFeePayedCount, 0) = 0    
    THEN DATEDIFF(DD, P.DueDate, GETDATE()) * LateFeeAmountPerDay.AppSettingValue    
   WHEN ISNULL(PaymentPending.LateFeesPending, 0) > 0    
    THEN PaymentAmountPending.LateFeesAmountPending    
   ELSE 0 END LateFeesAmountPending,    
  PaymentAmountPending.DepositAmountPending,        
  PaymentAmountPending.OnAccountAmountPending,
  vwDashBala.TotalExpenseAmount,
  vwDashBala.TotalIncomePaidAmount,
  vwDashBala.TotalIncomePendingAmount,
  vwDashBala.TotalIncomePaidAmount + 
  vwDashBala.TotalIncomePendingAmount AS TotalIncomeAmountByPeriod
 FROM PaymentPeriod PP        
  INNER JOIN Contract C ON C.ContractId = PP.ContractId        
  INNER JOIN House H ON h.HouseId = C.HouseId        
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
  
  
  OUTER APPLY  
  (  
   SELECT AppSettingValue FROM AppSetting WHERE Code = 'LATEFEEXDY'  
  ) AS LateFeeAmountPerDay  
  

  Left join vwDashboardBalance vwDashBala on vwDashBala.PeriodCode = P.Code
  
   go


/*DashBoard Ingresos Pagados y Pendientes*/

IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwPaymentPeriodPendingPaid]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwPaymentPeriodPendingPaid]  AS SELECT 1 AS X'
GO

ALTER VIEW [dbo].[vwPaymentPeriodPendingPaid]

AS
  
       
SELECT	P.Code as PeriodCode,      
		PaymentAmount,      
		H.Name as HouseName,      
		T.FullName as TenantFullName,        
	
		--PENDING AMOUNTS
		PaymentAmountPending.RentAmountPending,
		PaymentAmountPending.ServicesAmountPending,      
		PaymentAmountPending.FinesAmountPending,  
		PaymentAmountPending.LateFeesAmountPending,    
		PaymentAmountPending.DepositAmountPending,      
		PaymentAmountPending.OnAccountAmountPending,

		PaymentAmountPending.RentAmountPending +
		PaymentAmountPending.ServicesAmountPending+      
		PaymentAmountPending.FinesAmountPending+ 
		PaymentAmountPending.LateFeesAmountPending+    
		PaymentAmountPending.DepositAmountPending+      
		PaymentAmountPending.OnAccountAmountPending AS TotalAmountPending,

		--PAID AMOUNTS
		PaymentAmountPaid.RentAmountPaid,   
		PaymentAmountPaid.ServicesAmountPaid,      
		PaymentAmountPaid.FinesAmountPaid,  
		PaymentAmountPaid.LateFeesAmountPaid,    
		PaymentAmountPaid.DepositAmountPaid,      
		PaymentAmountPaid.OnAccountAmountPaid,

		PaymentAmountPaid.RentAmountPaid+   
		PaymentAmountPaid.ServicesAmountPaid+      
		PaymentAmountPaid.FinesAmountPaid+  
		PaymentAmountPaid.LateFeesAmountPaid+    
		PaymentAmountPaid.DepositAmountPaid+      
		PaymentAmountPaid.OnAccountAmountPaid as TotalAmountPaid
 FROM	PaymentPeriod PP      
		INNER JOIN Contract C ON C.ContractId = PP.ContractId      
		INNER JOIN House H ON h.HouseId = C.HouseId      
		INNER JOIN Tenant T ON T.TenantId = C.TenantId      
		INNER JOIN EntityStatus ES ON ES.EntityStatusId = PP.PaymentPeriodStatusId      
		INNER JOIN GeneralTable GT ON GT.GeneralTableId = PP.PaymentTypeId AND GT.Code = 'RENT'      
		INNER JOIN Period P ON P.PeriodId = PP.PeriodId       

		-- PENDING
		CROSS APPLY          
		(          
			SELECT     
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'RENT' THEN PaymentAmount ELSE 0 END, 0)), 0) AS RentAmountPending,   
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'SERVICE' THEN PaymentAmount ELSE 0 END, 0)), 0) AS ServicesAmountPending,        
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'FINE' THEN PaymentAmount ELSE 0 END, 0)), 0) AS FinesAmountPending,        
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'LATEFEE' THEN PaymentAmount ELSE 0 END, 0)), 0) AS LateFeesAmountPending,        
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'DEPOSIT' THEN PaymentAmount ELSE 0 END, 0)), 0) AS DepositAmountPending,      
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'ONACCOUNT' THEN PaymentAmount ELSE 0 END, 0)), 0) AS OnAccountAmountPending      
			FROM	PaymentPeriod PP1        
					INNER JOIN GENERALTABLE GT1 ON GT1.GeneralTableId = PP1.PaymentTypeId        
					INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = PP1.PaymentPeriodStatusId      
			WHERE	PP1.ContractId = PP.ContractId  AND         
					PP1.PeriodId = PP.PeriodId  AND         
					ES1.Code = 'PPPENDING'        
		) AS PaymentAmountPending

		-- PAID

		CROSS APPLY          
		(          
			SELECT     
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'RENT' THEN PaymentAmount ELSE 0 END, 0)), 0) AS RentAmountPaid,   
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'SERVICE' THEN PaymentAmount ELSE 0 END, 0)), 0) AS ServicesAmountPaid,        
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'FINE' THEN PaymentAmount ELSE 0 END, 0)), 0) AS FinesAmountPaid,        
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'LATEFEE' THEN PaymentAmount ELSE 0 END, 0)), 0) AS LateFeesAmountPaid,        
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'DEPOSIT' THEN PaymentAmount ELSE 0 END, 0)), 0) AS DepositAmountPaid,      
					ISNULL(SUM(ISNULL(CASE WHEN GT1.Code = 'ONACCOUNT' THEN PaymentAmount ELSE 0 END, 0)), 0) AS OnAccountAmountPaid      
			FROM	PaymentPeriod PP1        
					INNER JOIN GENERALTABLE GT1 ON GT1.GeneralTableId = PP1.PaymentTypeId        
					INNER JOIN EntityStatus ES1 ON ES1.EntityStatusId = PP1.PaymentPeriodStatusId      
			WHERE	PP1.ContractId = PP.ContractId  AND         
					PP1.PeriodId = PP.PeriodId  AND         
					ES1.Code = 'PPPAYED'        
		) AS PaymentAmountPaid


   
   go


   
/* VISTA EXPENSE SEARCH */

IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwExpenseSearch]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwExpenseSearch]  AS SELECT 1 AS X'
GO

ALTER VIEW [dbo].[vwExpenseSearch]

AS
                                                                                    
SELECT   
                                                                             
   E.ExpenseId  
  ,E.ExpenseDate  
  ,E.PaymentTypeId  
  ,PaymentType.Value as PaymentTypeName
  ,E.HouseId  
  ,H.Name as HouseName  
  ,H.HouseTypeId
  ,HouseType.Value as HouseTypeName
  ,E.PeriodId  
  ,Period.Code as PeriodCode
  ,E.ReferenceNo  
  ,E.Remark  
                                                                         
  ,E.SubTotalAmount  
  ,E.Tax  
  ,E.TotalAmount  
  ,E.BusinessPartnerId
  ,BP.Name as BusinessPartnerName
  ,Concepts.ConceptName AS Concepts
  ,Tenants.TenantFullName AS Tenants
  ,FileNames.FileName AS FileNames
                                                     
                                                    
FROM Expense E      

OUTER APPLY
(
	SELECT 	STRING_AGG (C.Description, ', ') as ConceptName
	FROM 	ExpenseDetail ed
			INNER JOIN Concept C ON C.ConceptId = ed.ConceptId
	WHERE	ed.ExpenseId = e.Expenseid
) AS Concepts

OUTER APPLY
(
	SELECT 	STRING_AGG (T.FullName, ', ') as TenantFullName
	FROM 	ExpenseDetail ed
			INNER JOIN Tenant T ON T.TenantId = ed.TenantId
	WHERE	ed.ExpenseId = e.Expenseid
) AS Tenants


OUTER APPLY
(
	SELECT 	STRING_AGG (FR.Name, ', ') as FileName
	FROM 	FileRepository fr
	WHERE	FR.ParentId = e.Expenseid AND fr.EntityCode = 'EX'
) AS FileNames
                                
  LEFT JOIN House H ON H.HouseId = E.HouseId      
     
  LEFT JOIN GeneralTable PaymentType ON PaymentType.GeneralTableId = E.PaymentTypeId  

  LEFT JOIN GeneralTable HouseType ON HouseType.GeneralTableId = H.HouseTypeId
        
  LEFT JOIN Period ON Period.PeriodId = E.PeriodId
                                    
  LEFT JOIN BusinessPartner BP ON BP.BusinessPartnerId = E.BusinessPartnerId            
  
  GO

  /*UPDATE GENERALTABLE INCOME PARAQUE NO APAREZCA EN LA RELACION DE CONCEPTOS A INGRESAR EN EXPENSES*/

  UPDATE GENERALTABLE SET TableName = 'ConceptTODO'
WHERE cODE = 'INCOME'


GO

/*ACTUALIZACION DE PROVEEDORES DE RENTAS*/
/*

=+CONCATENATE("INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)", "
VALUES (NULL,", "'", A4, " - ", D4, "', 76, NULL, 1, 1, GETDATE()) ")

*/

INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'353806 BC LTD - 3683 East Hastings Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Treaty Development LTD - 2132 Dundas Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Boundary Management  - 40 Powell Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Gramercy Enterprises LTD - 1270 Granville Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Zoro Holdings - 1075 Jervis Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Shelvyn Kiran - 1002 1082 Seymour Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Shelvyn Kiran - 1701 1082 Seymour St Parking', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Shelvyn Kiran - 909 1082 Seymour Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Pacific Coast Placement & Development - 2563 Renfrew Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Moe  - 1270 Robson Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Karen Ho - 788 Hamilton Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Karen Ho - 38 Smithe Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'Dezta Azale - 44th Avenue ', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INSTANT OFFICE LTD - 510 West Hastings Street', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INVESMENT LPD - 601 1450 West Georgia St', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INVESMENT LPD - 606 1450 West Georgia St', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INVESMENT LPD - 506 1450 West Georgia St', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INVESMENT LPD - 404 1450 West Georgia St', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INVESMENT LPD - 203 1450 West Georgia St', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INVESMENT LPD - 905 1450 West Georgia St', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INVESMENT LPD - 1606 1450 West Georgia St', 76, NULL, 1, 1, GETDATE()) 
INSERT INTO dbo.BusinessPartner (Code, Name, BPTypeId, SIN, RowStatus, CreatedBy, CreationDate)
VALUES (NULL,'INVESMENT LPD - 803 1450 West Georgia St', 76, NULL, 1, 1, GETDATE()) 

GO