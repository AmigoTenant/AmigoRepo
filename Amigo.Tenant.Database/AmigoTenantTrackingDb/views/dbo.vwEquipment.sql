IF not  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vwEquipment]'))
    exec sp_executesql N'CREATE VIEW [dbo].[vwEquipment]  AS SELECT 1 AS X'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[vwEquipment]
AS

SELECT	Equipment.EquipmentId,
		Equipment.EquipmentNo,
		--Equipment.EquipmentTypeId,
		Equipment.TestDate25Year,
		Equipment.TestDate5Year,
		Equipment.EquipmentSizeId,
		Equipment.EquipmentStatusId,
		Equipment.LocationId,
		Equipment.IsMasterRecord,
		Equipment.RowStatus,
		Equipment.CreatedBy,
		Equipment.CreationDate,
		Equipment.UpdatedBy,
		Equipment.UpdatedDate,
		EquipmentType.EquipmentTypeId,
		EquipmentType.Code as EquipmentTypeCode,
		EquipmentType.Name as EquipmentTypeName,
		EquipmentSize.Code as EquipmentSizeCode,
		EquipmentSize.Name as EquipmentSizeName
FROM	Equipment 
		LEFT JOIN EquipmentSize on EquipmentSize.EquipmentSizeId = Equipment.EquipmentSizeId
		LEFT JOIN EquipmentType on EquipmentType.EquipmentTypeId = EquipmentSize.EquipmentTypeId

GO
