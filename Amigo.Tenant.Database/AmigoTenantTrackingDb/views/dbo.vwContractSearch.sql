IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwContractSearch]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwContractSearch]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwContractSearch]    
AS    
SELECT   
  C.ContractId,
  P.PeriodId,  
  ContractCode,    
  C.ContractStatusId,  
  C.BeginDate,    
  C.EndDate,    
  T.FullName AS TenantFullName,  
  --'' AS TenantFullName,  
  C.HouseId,  
  ISNULL(UnpaidPeriod.UnpaidPeriods, 0) AS UnpaidPeriods,  
  NextDueDate.NextDaysToCollect,  
  NextDueDate.NextPeriodDate,    
  NextDueDate.NextDueDate,    
  C.CreationDate,  
  P.Code AS PeriodCode,  
  H.Name AS HouseName,
  C.RowStatus,
  ES.Code as ContractStatusCode,
  '' as Features,
  C.RentDeposit,
  C.RentPrice
FROM Contract C    
  LEFT JOIN Period P ON C.PeriodId = P.PeriodId    
  LEFT JOIN Tenant T ON T.TenantId = C.TenantId    
  LEFT JOIN House H ON H.HouseId = C.HouseId    
  --LEFT JOIN     
  OUTER APPLY     
   ( SELECT TOP 1 P.Code AS NextPeriodDate, cd.DueDate as NextDueDate, DATEDIFF(dd, GETDATE(), cd.DueDate) AS NextDaysToCollect  
    FROM ContractDetail CD    
      INNER JOIN Period P ON P.PeriodId = CD.PeriodId     
      INNER JOIN EntityStatus ES ON CD.ContractDetailStatusId = ES.EntityStatusId    
    WHERE CD.ContractId = C.ContractId     
      AND ES.EntityCode = 'CD' AND ES.Code = 'PENDING'    
    ORDER BY CD.ItemNo ASC    
   ) AS NextDueDate    
  OUTER APPLY    
   (    
    SELECT COUNT(*) AS UnpaidPeriods    
    FROM ContractDetail CD    
      INNER JOIN Period P ON P.PeriodId = CD.PeriodId    
      INNER JOIN EntityStatus ES ON CD.ContractDetailStatusId = ES.EntityStatusId    
    WHERE CD.ContractId = C.ContractId     
      AND ES.EntityCode = 'CD' AND ES.Code = 'PENDING' AND P.DueDate < GETDATE()   
    GROUP BY ContractId    
        
   ) AS UnpaidPeriod    
  LEFT JOIN EntityStatus ES ON ES.EntityStatusId = C.ContractStatusId
  
GO