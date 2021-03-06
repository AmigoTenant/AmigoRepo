IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwOtherTenant]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwOtherTenant]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER View vwOtherTenant    
AS    
SELECT OtherTenantId,    
  ContractId,    
  OT.TenantId,    
  T.FullName ,  
  OT.RowStatus,  
  OT.CreationDate,  
  OT.CreatedBy,  
  OT.UpdatedBy,  
  OT.UpdatedDate,
  OT.TableStatus
FROM OtherTenant OT    
  INNER JOIN Tenant T ON T.TenantId = OT.TenantId    
WHERE OT.RowStatus = 1   
  
  

GO