/*Rental Application*/

IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwRentalApplicationSearch]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwRentalApplicationSearch]  AS SELECT 1 AS X'
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
  ELSE 0 END HasNotification  ,
  Feature
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

/*Business Partner*/

IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwBusinessPartner]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwBusinessPartner]  AS SELECT 1 AS X'
GO


ALTER VIEW [dbo].[vwBusinessPartner]

AS

SELECT	BP.BusinessPartnerId
		,BP.Code
		,BP.Name
		,BP.BPTypeId
		,BP.[SIN]
		,BP.RowStatus
		,BP.CreatedBy
		,BP.CreationDate
		,BP.UpdatedBy
		,BP.UpdatedDate
		,GT.Code AS BPTypeCode
FROM	BusinessPartner BP
		INNER JOIN GeneralTable GT on GT.GeneralTableId = BP.BPTypeId
go


/*Expense*/

IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwExpenseSearch]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwExpenseSearch]  AS SELECT 1 AS X'
GO


ALTER VIEW vwExpenseSearch AS
  
SELECT   
   E.ExpenseId  
  ,E.ExpenseDate  
  ,E.PaymentTypeId  
  ,E.HouseId  
  ,E.PeriodId  
  ,E.ReferenceNo  
  ,E.Remark  
  ,E.SubTotalAmount  
  ,E.Tax  
  ,E.TotalAmount  
  ,E.RowStatus  
  ,E.CreatedBy  
  ,E.CreationDate  
  ,E.UpdatedBy  
  ,E.UpdatedDate   
  ,ED.ExpenseDetailStatusId  
  ,ED.TenantId
  ,T.FullName as TenantFullName  
  ,H.Name as HouseName  
  ,H.HouseTypeId
  ,ES.Name as ExpenseDetailStatusName  
  ,C.ConceptId  
  ,C.Description as ConceptName  
  ,PaymentType.Value as PaymentTypeName
  ,HouseType.Value as HouseTypeName
  ,Period.Code as PeriodCode
  ,BP.Name as BusinessPartnerName
FROM Expense E      
  LEFT JOIN ExpenseDetail ED on E.ExpenseId = ED.ExpenseId  
  LEFT JOIN Tenant T ON T.TenantId = ED.TenantId      
  LEFT JOIN House H ON H.HouseId = E.HouseId      
  LEFT JOIN EntityStatus ES ON ED.ExpenseDetailStatusId = ES.EntityStatusId      
  LEFT JOIN Concept C ON C.ConceptId = ED.ConceptId  
  LEFT JOIN GeneralTable PaymentType ON PaymentType.GeneralTableId = E.PaymentTypeId  
  LEFT JOIN GeneralTable HouseType ON HouseType.GeneralTableId = H.HouseTypeId
  LEFT JOIN Period ON Period.PeriodId = E.PeriodId
  LEFT JOIN BusinessPartner BP ON BP.BusinessPartnerId = E.BusinessPartnerId
go

