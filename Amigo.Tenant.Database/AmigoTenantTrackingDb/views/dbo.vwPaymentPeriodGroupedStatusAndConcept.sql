IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwPaymentPeriodGroupedStatusAndConcept]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwPaymentPeriodGroupedStatusAndConcept]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwPaymentPeriodGroupedStatusAndConcept]    
AS    

Select	PeriodId, TenantId, 
		SUM(CASE WHEN ES.Code = 'PPPAYED' AND CPTO.Code = 'ONACCOUNT' THEN PaymentAmount ELSE 0 END) TotalOnAccountAmountPayed ,
		SUM(CASE WHEN ES.Code = 'PPPENDING' AND CPTO.Code = 'RENT' THEN PaymentAmount ELSE 0 END) TotalRentAmountPending ,
		SUM(CASE WHEN ES.Code = 'PPPENDING' AND CPTO.Code = 'DEPOSIT' THEN PaymentAmount ELSE 0 END) TotalDepositAmountPending, 
		SUM(CASE WHEN ES.Code = 'PPPENDING' AND CPTO.Code = 'SVCWATER' THEN PaymentAmount ELSE 0 END) TotalSvcWaterAmountPending ,
		SUM(CASE WHEN ES.Code = 'PPPENDING' AND CPTO.Code = 'LATEFEE' THEN PaymentAmount ELSE 0 END) TotalLateFeeAmountPending ,
		SUM(CASE WHEN ES.Code = 'PPPENDING' AND CPTO.Code = 'SVCENERGY' THEN PaymentAmount ELSE 0 END) TotalSvcEnergyAmountPending ,
		SUM(CASE WHEN ES.Code = 'PPPENDING' AND CPTO.Code = 'FINEINFRAC' THEN PaymentAmount ELSE 0 END) TotalFineInfracAmountPending 
From	PaymentPeriod PP
		INNER JOIN EntityStatus ES on ES.EntityStatusId = PP.PaymentPeriodStatusId
		INNER JOIN Concept CPTO on CPTO.ConceptId = PP.ConceptId AND CPTO.RowStatus = 1
Where	--PeriodId = 47 AND TENANTID = 304 and 
		PP.RowStatus = 1 
group by PeriodId, TenantId

