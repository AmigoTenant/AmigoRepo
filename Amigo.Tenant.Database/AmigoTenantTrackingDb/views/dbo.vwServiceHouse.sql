IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwServiceHouse]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwServiceHouse]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW vwServiceHouse
AS
	SELECT 
		hs.ServiceId,
		hs.ConceptId,
		c.Code as [ConceptCode],
		c.Description as [ConceptDescription],
		c.TypeId as [ConceptTypeId],
		hs.BusinessPartnerId,
		b.Name as [BusinessPartnerName],
		hs.ServiceTypeId,
		g.Code as [ServiceTypeCode],
		g.Value as [ServiceTypeValue],

		hs.RowStatus,
		hs.CreatedBy,
		hs.CreationDate,
		hs.UpdatedBy,
		hs.UpdatedDate,

		shp.MonthId,
		shp.DueDateMonth,
		shp.DueDateDay,
		shp.CutOffMonth,
		shp.CutOffDay

	FROM ServiceHouse hs
		INNER JOIN BusinessPartner b ON b.BusinessPartnerId = hs.BusinessPartnerId
		INNER JOIN GeneralTable g ON g.GeneralTableId = hs.ServiceTypeId
		INNER JOIN Concept c ON c.ConceptId = hs.ConceptId
		INNER JOIN ServiceHousePeriod shp ON shp.ServiceId = hs.ServiceId
	WHERE hs.RowStatus = 1
		and shp.RowStatus = 1
				
GO

