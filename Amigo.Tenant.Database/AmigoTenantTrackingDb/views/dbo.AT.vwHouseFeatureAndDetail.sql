IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwHouseFeatureAndDetail]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwHouseFeatureAndDetail]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW vwHouseFeatureAndDetail                
AS                
                
SELECT             
  Feature.Sequence,          
  HF.HouseFeatureId,                
  Feature.Description,                
  0 as marked, 
  1 as ContractId,            
  HF.HouseId,              
  Feature.IsAllHouse,              
  HF.RentPrice,              
  HF.HouseFeatureStatusId,              
  ES.Code as HouseFetureStatusCode,            
  0 as TableStatus  ,            
  CASE WHEN CHD.HasContract =0 THEN 0 ELSE 1 END as couldBeDeleted,  
  GETDATE() AS BeginDate, 
  GETDATE() AS EndDate, 
  0 as ContractHouseDetailId,
  HF.AditionalAddressInfo
FROM HouseFeature HF                 
  INNER JOIN Feature ON Feature.FeatureId = HF.FeatureId                
  LEFT JOIN EntityStatus ES ON ES.EntityStatusId = HF.HouseFeatureStatusId                
  CROSS APPLY  
 (SELECT COUNT(*) AS HasContract 
 FROM ContractHouseDetail CHD 
 WHERE CHD.HouseFeatureId = HF.HouseFeatureId ) AS CHD  

WHERE HF.IsRentable = 1 and HF.RowStatus = 1 and Feature.RowStatus = 1
go