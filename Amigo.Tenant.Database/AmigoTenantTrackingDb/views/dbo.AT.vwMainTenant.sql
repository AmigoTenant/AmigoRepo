IF  NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwMainTenant]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwMainTenant]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwMainTenant]
AS 
SELECT t.*,    
 c.Name as CountryName,    
 g.Value as TypeName,    
 g.Code as TypeCode,
 CO.ContractId,
 ISNULL(ES.Code, '') as ContractStatusCode
FROM Tenant t    
 LEFT JOIN Country c ON c.CountryId = t.CountryId    
 LEFT JOIN GeneralTable g ON g.GeneralTableId = t.TypeId    
 LEFT JOIN Contract CO ON CO.TenantId = t.tenantId AND CO.RowStatus = 1 
 LEFT JOIN EntityStatus ES ON ES.EntityStatusId = CO.ContractStatusId 



GO

