IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwHouseFeature]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwHouseFeature]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwHouseFeature]
AS
	SELECT 
	    HouseFeatureId ,
        hf.HouseId ,
        hf.FeatureId ,
        HouseFeatureStatusId ,
        IsRentable ,
        hf.RowStatus ,
        AdditionalAddressInfo ,
        hf.RentPrice ,
        e.Name as HouseFeatureStatusName ,
        f.Code as FeatureCode ,
        f.[Description] as FeatureDescription ,
        f.Measure as FeatureMeasure ,
        hf.CreatedBy ,
        hf.CreationDate ,
        hf.UpdatedBy ,
        hf.UpdatedDate ,
        f.[Sequence]

		FROM HouseFeature hf
			INNER JOIN EntityStatus e ON e.EntityStatusId = hf.HouseFeatureStatusId
			INNER JOIN House h ON h.HouseId = hf.HouseId
			INNER JOIN Feature f ON f.FeatureId = hf.FeatureId
		WHERE f.IsAllHouse = 0
GO
