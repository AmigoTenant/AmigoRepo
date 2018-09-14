IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwHouse]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwHouse]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwHouse]
AS
SELECT	
	h.HouseId      ,
	h.Code		   ,
	h.Name		   ,
	h.ShortName	   ,
	h.LocationId   ,
	h.Address	   ,
	h.PhoneNumber  ,
	h.RentPrice	   ,
	h.RentDeposit  ,
	h.HouseTypeId  ,
	h.HouseStatusId,
	h.RowStatus	   ,
	h.CreatedBy	   ,
	h.CreationDate ,
	h.UpdatedBy	   ,
	h.UpdatedDate,
	e.Code as StatusCode,
	e.Name as StatusName,
	g.Value as HouseTypeName,
	g.Code as HouseTypeCode,
	h.Latitude,
	h.Longitude,
	h.CityId,
	c.Code as CityCode,
	c.Name as CityName
FROM House h
	INNER JOIN EntityStatus e ON e.EntityStatusId = h.HouseStatusId
	INNER JOIN GeneralTable g ON g.GeneralTableId = h.HouseTypeId
	LEFT OUTER JOIN City c ON c.CityId = h.CityId
GO
