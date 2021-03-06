IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwAmigoTenantTServiceApproveRatesPerHour]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwAmigoTenantTServiceApproveRatesPerHour]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwAmigoTenantTServiceApproveRatesPerHour]
AS   
SELECT AmigoTenantTEventLogId,    
  AT.Code AS ActivityTypeCode,    
  AT.Name AS ActivityTypeName,    
  EL.Username,    
  US.AmigoTenantTUserId,
  ReportedActivityDateLocal,    
  AmigoTenantTServiceId  
FROM AmigoTenantTEventLog EL    
  INNER JOIN ActivityType AT ON AT.ActivityTypeId = EL.ActivityTypeId    
  INNER JOIN AmigoTenantTUser US ON US.Username = EL.Username
WHERE AT.Code IN ('STW', 'FNW', 'ONB', 'OFB' )     

GO
