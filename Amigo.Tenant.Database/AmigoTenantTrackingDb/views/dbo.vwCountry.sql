IF  NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwCountry]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwCountry]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwCountry]
AS 
SELECT c.CountryId, c.ISOCode,
	c.Name, c.RowStatus,
	c.CreatedBy, c.CreationDate,
	c.UpdatedBy, c.UpdatedDate
FROM Country c

WHERE c.Rowstatus = 1


GO

