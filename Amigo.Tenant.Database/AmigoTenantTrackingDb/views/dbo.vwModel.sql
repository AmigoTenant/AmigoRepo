IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwModel]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwModel]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwModel]
AS

      select b.BrandId,
                  b.Name as 'BrandName',
                  m.ModelId,
                  m.Name as 'ModelName'
      from Brand b
            inner join Model m on ( b.BrandId = m.BrandId and  m.RowStatus = 1)
      where b.RowStatus = 1

GO
