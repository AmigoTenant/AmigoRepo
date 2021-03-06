IF  NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwDevice]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwDevice]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwDevice]
AS

SELECT	
		d.DeviceId, 
		d.CellphoneNumber, 
		d.Identifier, 
		d.WIFIMAC, 
		
		d.OSVersionId,
		osv.[version] as 'OSVersion',
		osv.[name] as 'OSVersionName',
		
		osv.PlatformId,
		p.Name as 'PlatformName',
		
		d.ModelId, 
		m.Name as 'ModelName',
		
		m.brandid,
		b.name as 'BrandName',
		
		d.IsAutoDateTime, 
		d.IsSpoofingGPS, 
		d.IsRootedJailbreaked, 
		
		d.AppVersionId, 
		a.[version] as 'AppVersion', 
		a.name as 'AppVersionName', 
		
		d.AssignedAmigoTenantTUserId, 
		u.username as 'AssignedAmigoTenantTUserUsername',
		
		d.RowStatus, 
		d.CreatedBy, 
		d.CreationDate, 
		d.UpdatedBy, 
		d.UpdatedDate

FROM dbo.Device d
	left join dbo.OSVersion osv on (d.osversionid = osv.osversionid and osv.rowstatus = 1)
	left join dbo.[Platform] p on (osv.PlatformId = p.PlatformId and p.rowstatus = 1)
	left join dbo.Model m on (d.modelid = m.modelid and m.rowstatus = 1)
	left join dbo.Brand b on (m.brandid = b.brandid and b.rowstatus = 1)
	left join dbo.AppVersion a on (d.appversionid = a.appversionid and a.rowstatus = 1)
	left join dbo.AmigoTenantTUser u on (d.AssignedAmigoTenantTUserId = u.AmigoTenantTUserId and u.rowstatus = 1)
where d.rowstatus = 1	

GO
