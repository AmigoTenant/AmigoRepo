IF NOT EXISTS(SELECT * FROM Module WHERE Code='UTILITYBILLS')
BEGIN
	INSERT INTO Module(Code, CreatedBy, CreationDate, Name, ParentModuleId, RowStatus, SortOrder, URL)
	VALUES ('UTILITYBILLS', 1, GETDATE(), 'Utility Bills', null, 1, 7, null)
END

DECLARE @ModuleId INT
set @ModuleId=0
SELECT @ModuleId=ModuleId FROM Module where Code='UTILITYBILLS'

IF NOT EXISTS(SELECT * FROM Module WHERE Code='AT-UTILITYBILLSRECOR')
BEGIN
	INSERT INTO Module(Code, CreatedBy, CreationDate, Name, ParentModuleId, RowStatus, SortOrder, URL)
	VALUES ('AT-UTILITYBILLSRECOR', 1, GETDATE(), 'Record Utility Bills', @ModuleId, 1, 26, '/utilitybill/record')
END

-- ACTION

SELECT @ModuleId=ModuleId FROM Module where Code='AT-UTILITYBILLSRECOR'

IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.BillRecord.Search')
BEGIN
	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
	VALUES ('AT.BillRecord.Search', 'Search', 'Search', 'Button', @ModuleId, 1, 1, GETDATE())
END

IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.BillRecord.Create')
BEGIN
	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
	VALUES ('AT.BillRecord.Create', 'Create', 'Create', 'Button', @ModuleId, 1, 1, GETDATE())
END

IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.BillRecord.Update')
BEGIN
	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
	VALUES ('AT.BillRecord.Update', 'Update', 'Update', 'Button', @ModuleId, 1, 1, GETDATE())
END

IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.BillRecord.Delete')
BEGIN
	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
	VALUES ('AT.BillRecord.Delete', 'Delete', 'Delete', 'Button', @ModuleId, 1, 1, GETDATE())
END

-- PERSMISSION

INSERT INTO Permission(AmigoTenantTRoleId, ActionId)
select r.AmigoTenantTRoleId, a.ActionId
from AmigoTenantTRole r
	cross join Action a 
where r.RowStatus = 1
	and a.Code like 'AT.Bill%' 
	AND ltrim(r.AmigoTenantTRoleId) + '-' + ltrim(a.ActionId) NOT IN (SELECT ltrim(AmigoTenantTRoleId) + '-' + ltrim(ActionId) FROM Permission)
