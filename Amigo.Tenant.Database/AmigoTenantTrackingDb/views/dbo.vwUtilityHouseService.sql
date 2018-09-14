IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwUtilityHouseService]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwUtilityHouseService]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW vwUtilityHouseService
AS
	SELECT 
		hs.HouseId,
		hs.HouseServiceId,
		hs.ServiceId,
		hs.RowStatus,
		hs.CreatedBy,
		hs.CreationDate,

		s.ConceptId,
		s.ServiceTypeId,
		s.BusinessPartnerId,
		c.Code as [ConceptCode],
		c.Description as [ConceptDescription],
		c.TypeId as [ConceptTypeId],
		b.Name as [BusinessPartnerName],
		b.Code as [BusinessPartnerCode],
		g.Code as [ServiceTypeCode],
		g.Value as [ServiceTypeValue],

		hsp.MonthId,
		hsp.DueDateMonth,
		hsp.DueDateDay,
		hsp.CutOffMonth,
		hsp.CutOffDay,
		hsp.HouseServicePeriodId,
		hsp.PeriodId,
		hsp.Amount,
		hsp.Adjust,
		hsp.Consumption,
		hsp.ConsumptionUnmId,
		hsp.HouseServicePeriodStatusId,
		hsp.CreatedBy as HouseServicePeriodCreatedBy,
		hsp.CreationDate as HouseServicePeriodCreationDate,

		p.Code as PeriodCode,
		p.BeginDate,
		p.EndDate,
		p.DueDate,
		p.Sequence,
		p.CreatedBy as PeriodCreatedBy,
		p.CreationDate as PeriodCreationDate

	FROM HouseService hs
		INNER JOIN HouseServicePeriod hsp ON hsp.HouseServiceId = hs.HouseServiceId
		INNER JOIN ServiceHouse s ON s.ServiceId = hs.ServiceId
		INNER JOIN BusinessPartner b ON b.BusinessPartnerId = s.BusinessPartnerId
		INNER JOIN GeneralTable g ON g.GeneralTableId = s.ServiceTypeId
		INNER JOIN Concept c ON c.ConceptId = s.ConceptId
		LEFT OUTER JOIN [Period] p ON p.PeriodId = hsp.PeriodId

	WHERE hs.RowStatus = 1
		AND hsp.RowStatus = 1
		AND p.RowStatus = 1
				
GO