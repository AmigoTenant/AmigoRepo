IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwAmigoTenantTServiceLatest]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwAmigoTenantTServiceLatest]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwAmigoTenantTServiceLatest]
AS 
SELECT 
--           STSVC.AmigoTenantTServiceId                     ,         
--           STSVC.AmigoTenantTServiceId       ,
             STSVC.AmigoTenantTUserId ,
             STSVC.ServiceStartDate        ,
             STSVC.ServiceFinishDate       ,
             STSVC.OriginLocationId        ,
             STSVC.DestinationLocationId,
             LOCORI.Code as OriginLocationCode,
             LOCORI.Name as OriginLocationName,
             LOCDST.Code as DestinationLocationCode,
             LOCDST.Code as DestinationLocationName,
             DIS.Code as DispatchingPartyCode,
             --ISNULL(STSVC.CostCenterCode, '') AS CostCenterCode,
             --ISNULL(STSVC.ShipmentID, '') AS ShipmentID,
			 STSVC.ChargeNo,
			 STSVC.ChargeType
FROM   AmigoTenantTService STSVC
             LEFT JOIN Location LOCORI ON LOCORI.LocationId = STSVC.OriginLocationId
             LEFT JOIN Location LOCDST ON LOCDST.LocationId = STSVC.DestinationLocationId
             LEFT JOIN DispatchingParty DIS ON DIS.DispatchingPartyId = STSVC.DispatchingPartyId
             CROSS APPLY 
             (
                    SELECT TOP    1 STSVCLAST.AmigoTenantTServiceId
                    FROM   AmigoTenantTService STSVCLAST
                    WHERE  STSVC.AmigoTenantTUserId = STSVCLAST.AmigoTenantTUserId AND
                                  STSVCLAST.RowStatus = 1 
                    ORDER BY ServiceStartDate DESC
             ) AS LATESTSERV
WHERE  LATESTSERV.AmigoTenantTServiceId = STSVC.AmigoTenantTServiceId AND
             STSVC.RowStatus = 1 



GO
