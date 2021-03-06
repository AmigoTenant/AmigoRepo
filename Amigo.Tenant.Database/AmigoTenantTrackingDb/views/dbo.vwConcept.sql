IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwConcept]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwConcept]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwConcept]
AS

SELECT	ConceptId
		,cpto.Code
		,Description
		,TypeId
		,CPTO.RowStatus
		,Remark
		,PayTypeId
		,CPTO.CreatedBy
		,CPTO.CreationDate
		,CPTO.UpdatedBy
		,CPTO.UpdatedDate
		,ConceptAmount
		,GNRL.Code as TypeCode
FROM	Concept CPTO
        INNER JOIN  GeneralTable GNRL on (GNRL.GeneralTableId = CPTO.TypeId)

GO
