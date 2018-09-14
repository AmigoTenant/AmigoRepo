IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwHouseService]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwHouseService]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW vwHouseService
AS
	SELECT 
		hs.HouseId,
		hs.HouseServiceId,
		hs.ServiceId,

		s.ConceptId,
		c.Code as [ConceptCode],
		c.Description as [ConceptDescription],
		c.TypeId as [ConceptTypeId],
		s.BusinessPartnerId,
		b.Name as [BusinessPartnerName],
		b.Code as [BusinessPartnerCode],
		s.ServiceTypeId,
		g.Code as [ServiceTypeCode],
		g.Value as [ServiceTypeValue],

		hs.RowStatus,
		hs.CreatedBy,
		hs.CreationDate,
		hs.UpdatedBy,
		hs.UpdatedDate,

		hsp.MonthId,
		hsp.DueDateMonth,
		hsp.DueDateDay,
		hsp.CutOffMonth,
		hsp.CutOffDay,
		hsp.HouseServicePeriodId

	FROM HouseService hs
		INNER JOIN HouseServicePeriod hsp ON hsp.HouseServiceId = hs.HouseServiceId
		INNER JOIN ServiceHouse s ON s.ServiceId = hs.ServiceId
		INNER JOIN BusinessPartner b ON b.BusinessPartnerId = s.BusinessPartnerId
		INNER JOIN GeneralTable g ON g.GeneralTableId = s.ServiceTypeId
		INNER JOIN Concept c ON c.ConceptId = s.ConceptId
	WHERE hs.RowStatus = 1
		AND hsp.RowStatus = 1
				
GO