IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwExpenseDetailSearch]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwExpenseDetailSearch]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwExpenseDetailSearch]    
AS    

Select	ED.ExpenseDetailId
		,ED.ExpenseId
		,ED.ConceptId
		,ED.Remark
		,ED.Amount
		,ED.Tax
		,ED.TotalAmount
		,ED.TenantId
		,ED.RowStatus
		,ED.CreatedBy
		,ED.CreationDate
		,ED.UpdatedBy
		,ED.UpdatedDate
		,CO.Description as ConceptDescription
		,TE.FullName
From	ExpenseDetail ED
		LEFT JOIN Concept CO ON CO.ConceptId = ED.ConceptId
		LEFT JOIN Tenant TE ON TE.TenantId = ED.TenantId