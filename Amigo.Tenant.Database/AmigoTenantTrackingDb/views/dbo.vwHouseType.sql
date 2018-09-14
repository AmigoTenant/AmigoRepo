IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwHouseType]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwHouseType]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwHouseType]  
AS  
  
SELECT GeneralTableId as Id, Code, Value as Name
FROM dbo.GeneralTable
WHERE TableName = 'HouseType' 
	AND RowStatus = 1  
	
GO
