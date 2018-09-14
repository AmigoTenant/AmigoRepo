IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwAmigoTenantTEventLog]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwAmigoTenantTEventLog]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[vwAmigoTenantTEventLog]
AS

	SELECT el.AmigoTenantTEventLogId, 
			el.activitytypeid, 
			at.Code as 'ActivityTypeCode', 
			at.Name as 'ActivityTypeName', 
			el.Username, 
			el.ReportedActivityDate, 
			el.ReportedActivityTimeZone, 
			el.ConvertedActivityUTC, 
			el.LogType, 
			el.Parameters, 
			--el.ShipmentID, 
			--el.CostCenterCode, 
			--el.CostCenterId, 
			el.EquipmentNumber, 
			el.EquipmentId, 
			el.IsAutoDateTime, 
			el.IsSpoofingGPS, 
			el.IsRootedJailbreaked, 
			el.Platform, 
			el.OSVersion, 
			el.AppVersion, 
			el.Latitude, 
			el.Longitude, 
			el.Accuracy, 
			el.LocationProvider,
			el.CreatedBy,
			el.CreationDate,
			el.UpdatedBy,
			el.UpdatedDate,
			el.RowStatus,
			el.ChargeNo
	FROM dbo.AmigoTenantTEventLog el
		inner join dbo.ActivityType at on ( el.activitytypeid = at.activitytypeid and at.rowstatus = 1)

GO


