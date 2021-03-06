IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwAmigoTenantTUser]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwAmigoTenantTUser]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwAmigoTenantTUser]
AS

SELECT AmigoTenantTUser.AmigoTenantTUserId          ,
             AmigoTenantTUser.Username              ,
             AmigoTenantTUser.PayBy              ,
             AmigoTenantTUser.UserType              ,
             AmigoTenantTUser.DedicatedLocationId   ,
             AmigoTenantTUser.BypassDeviceValidation,
             AmigoTenantTUser.UnitNumber            ,
             AmigoTenantTUser.TractorNumber         ,
             AmigoTenantTUser.RowStatus                ,
             AmigoTenantTUser.CreatedBy             ,
             AmigoTenantTUser.CreationDate          ,
             AmigoTenantTUser.UpdatedBy             ,
             AmigoTenantTUser.UpdatedDate           ,
             AmigoTenantTRole.AmigoTenantTRoleId                    ,
             Location.Name as LocationName     ,
             Location.Code as LocationCode    ,
             AmigoTenantTRole.Name as AmigoTenantTRoleName,
             AmigoTenantTRole.Code as AmigoTenantTRoleCode,
             FirstName,
             LastName,
			 Device.DeviceId,
			 dbo.Device.CellphoneNumber,
			 dbo.AmigoTenantTRole.IsAdmin
FROM   AmigoTenantTUser 
             LEFT JOIN Location on Location.LocationId = AmigoTenantTUser.DedicatedLocationId
             LEFT JOIN AmigoTenantTRole on AmigoTenantTRole.AmigoTenantTRoleId = AmigoTenantTUser.AmigoTenantTRoleId
			 LEFT JOIN dbo.Device ON Device.AssignedAmigoTenantTUserId = AmigoTenantTUser.AmigoTenantTUserId

GO
