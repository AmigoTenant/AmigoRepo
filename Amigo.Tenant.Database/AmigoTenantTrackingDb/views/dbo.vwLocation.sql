IF not EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwLocation]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwLocation]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[vwLocation]
AS

SELECT      
            l.LocationId, 
            l.Name, 
            l.Code, 
            l.LocationTypeId, 
            l.ParentLocationId, 
            l.Latitude, 
            l.Longitude, 
            l.Address1, 
            l.Address2, 
            l.ZipCode, 
            l.RowStatus, 
            l.CreatedBy, 
            l.CreationDate, 
            l.UpdatedBy, 
            l.UpdatedDate, 
            l.CityId,
            p.Code as 'ParentLocationCode',
            p.Name as 'ParentLocationName',
            t.Name as 'LocationTypeName',
            t.Code as 'LocationTypeCode',
            c.Code as 'CityCode', 
            c.Name as 'CityName', 
            s.StateId, 
            s.Name as 'StateName', 
            s.Code as 'StateCode', 
            co.CountryId, 
            co.Name as 'CountryName', 
            co.ISOCode as 'CountryISOCode',
            l.HasGeofence

FROM dbo.Location l
      LEFT JOIN dbo.Location p ON (l.ParentLocationId = p.LocationId and p.rowstatus = 1)
      LEFT JOIN dbo.LocationType t ON (l.LocationTypeId = t.LocationTypeId and t.rowstatus = 1)
      LEFT JOIN dbo.City c ON (l.CityId = c.CityId and c.rowstatus = 1)
      LEFT JOIN dbo.[State] s ON (c.StateId = s. StateId and s.rowstatus = 1)
      LEFT JOIN dbo.Country co ON (s.CountryId = co.CountryId and co.rowstatus = 1)
where l.RowStatus = 1
GROUP BY 
            l.LocationId, 
            l.Name, 
            l.Code, 
            l.LocationTypeId, 
            l.ParentLocationId, 
            l.Latitude, 
            l.Longitude, 
            l.Address1, 
            l.Address2, 
            l.ZipCode, 
            l.RowStatus, 
            l.CreatedBy, 
            l.CreationDate, 
            l.UpdatedBy, 
            l.UpdatedDate, 
            l.CityId,
            p.Code,
            p.Name,           
            t.Name,
            t.Code,
            c.Code, 
            c.Name, 
            s.StateId, 
            s.Name, 
            s.Code, 
            co.CountryId, 
            co.Name, 
            co.ISOCode,
            l.HasGeofence 

GO
