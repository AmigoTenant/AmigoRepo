IF not  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwCity]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwCity]  AS  SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwCity]
AS

SELECT	
		c.CityId, 
		c.Name as 'CityName', 
		c.Code as 'CityCode', 

		c.StateId,
		s.Code as 'StateCode', 
		s.Name as 'StateName', 
		
		s.CountryId,
		co.ISOCode as 'CountryISOCode',
		co.Name 'CountryName'
		
		
FROM dbo.City c
	LEFT  JOIN dbo.[State] s ON (c.StateId = s.StateId and s.RowStatus = 1)
	LEFT  JOIN dbo.Country co ON (s.CountryId = co.CountryId and co.RowStatus = 1)
WHERE c.RowStatus = 1
GO
