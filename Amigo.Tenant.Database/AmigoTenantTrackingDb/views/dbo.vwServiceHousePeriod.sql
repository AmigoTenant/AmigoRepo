IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwServiceHousePeriod]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwServiceHousePeriod]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW vwServiceHousePeriod
AS
	SELECT
		p.ServiceHousePeriodId,
		p.MonthId,
		p.ServiceId,
		p.DueDateMonth,
		p.DueDateDay,
		p.CutOffMonth,
		p.CutOffDay,
		sh.ConceptId,
		sh.BusinessPartnerId,
		sh.ServiceTypeId,
		p.RowStatus,
		p.CreatedBy,
		p.CreationDate,
		p.UpdatedBy,
		p.UpdatedDate

	FROM ServiceHousePeriod p
		INNER JOIN ServiceHouse sh ON sh.ServiceId = p.ServiceId
	WHERE sh.RowStatus = 1
				
GO

