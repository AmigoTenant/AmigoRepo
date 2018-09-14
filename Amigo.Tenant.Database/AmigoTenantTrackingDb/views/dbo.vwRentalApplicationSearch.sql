IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwRentalApplicationSearch]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwRentalApplicationSearch]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwRentalApplicationSearch]

AS
SELECT       
  RA.RentalApplicationId,    
  RA.PeriodId,      
  P.Code AS PeriodCode,    
  RA.ApplicationDate,   
  RA.PropertyTypeId,  
  PT.Value as PropertyTypeName,  
  RA.FullName,  
  RA.Email,  
  RA.HousePhone,  
  RA.CellPhone,  
  RA.CheckIn,  
  RA.CheckOut,  
  RA.ResidenseCountryId,  
  CO.Name as ResidenseCountryName,  
  RA.BudgetId,  
  BU.Value as BudgetName,  
  0 AS AvailableProperties,  
  0 as RentedProperties,  
  RA.CityOfInterestId,  
  CI.Name as CityOfInterestName,  
  RA.HousePartId,  
  HP.Value as HousePartName,  
  RA.OutInDownId,  
  DW.Value as OutInDownName,  
  RA.PersonNo,  
  RA.ReferredById,  
  RR.Value as ReferredByName,  
  RA.ReferredByOther,  

  RA.PriorityId,
  PR.Value as PriorityName,
  RA.AlertDate,
  RA.AlertMessage,
  CASE	WHEN	RA.AlertDate IS NOT NULL AND 
				GETDATE()+10 >= RA.AlertDate
  THEN 1  
  ELSE 0 END HasNotification  
  
FROM RentalApplication RA  
  LEFT JOIN Period P ON RA.PeriodId = P.PeriodId        
  LEFT JOIN GeneralTable PT on PT.GeneralTableId = RA.PropertyTypeId  
  LEFT JOIN Country CO on CO.CountryId = RA.ResidenseCountryId  
  LEFT JOIN GeneralTable BU on BU.GeneralTableId = RA.BudgetId  
  LEFT JOIN City CI on CI.CityId = RA.CityOfInterestId  
  LEFT JOIN GeneralTable HP on HP.GeneralTableId = RA.HousePartId  
  LEFT JOIN GeneralTable DW on DW.GeneralTableId = RA.OutInDownId  
  LEFT JOIN GeneralTable RR on RR.GeneralTableId = RA.ReferredById  
  LEFT JOIN GeneralTable PR on PR.GeneralTableId = RA.PriorityId
WHERE RA.RowStatus = 1




go