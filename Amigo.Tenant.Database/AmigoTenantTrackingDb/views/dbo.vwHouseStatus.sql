IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwHouseStatus]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwHouseStatus]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwHouseStatus]  
AS  
  
SELECT EntityStatusId as Id, Code, Name
FROM dbo.EntityStatus
WHERE EntityCode = 'HO' 
	--AND RowStatus = 1  
	
GO