IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwLast24Hours]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwLast24Hours]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER VIEW [dbo].[vwLast24Hours]
AS

      select      at.Name as 'ActivityTypeName',
                  at.Code as 'ActivityTypeCode',
                  el.Username,
                  u.AmigoTenantTUserId, 
                  u.TractorNumber,
                  el.reportedActivityDate,
                  el.ReportedActivityTimeZone,
                  el.Latitude,
                  el.Longitude,
                  s.ChargeType,
                  el.EquipmentNumber,
                  s.ChassisNumber,
				  el.ChargeNo,
				  pro.Name as Product, 
				  originloc.Name as Origin, 
				  destinationLoc.Name as Destination,
				  SER.Name AS 'ServiceName',
				  EQUTYP.Name AS 'EquipmentTypeName'
      from dbo.AmigoTenantTEventLog el
            inner join dbo.ActivityType at on ( el.activitytypeid = at.activitytypeid and at.rowstatus = 1)
            inner join dbo.AmigoTenantTUser u on (el.username = u.username and u.rowstatus = 1)
            left join dbo.AmigoTenantTService s on (el.AmigoTenantTServiceId = s.AmigoTenantTServiceId and s.rowstatus = 1)
			inner join product pro on s.ProductId = pro.ProductId 
			left join Location originloc on s.OriginLocationCode = originloc.Code
			left join Location destinationLoc on s.DestinationLocationCode = destinationLoc.Code     
		    INNER JOIN dbo.Service SER ON s.ServiceId = SER.ServiceId
	        LEFT JOIN dbo.EquipmentType EQUTYP ON s.EquipmentTypeId = EQUTYP.EquipmentTypeId       
      where el.rowstatus = 1
                  and el.Latitude <> 0 and el.Latitude <> -1
                  and el.Longitude <> 0 and el.Longitude <> -1


GO