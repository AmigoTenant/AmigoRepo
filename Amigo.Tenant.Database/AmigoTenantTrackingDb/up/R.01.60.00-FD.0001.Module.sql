IF NOT EXISTS(SELECT * FROM Module WHERE Code='DASHBOARD')
BEGIN
	INSERT INTO Module(Code, CreatedBy, CreationDate, Name, ParentModuleId, RowStatus, SortOrder, URL)
	VALUES ('DASHBOARD', 1, GETDATE(), 'Dasboard', null, 1, 6, null)
END

DECLARE @ModuleId INT
set @ModuleId=0
SELECT @ModuleId=ModuleId FROM Module where Code='DASHBOARD'

IF NOT EXISTS(SELECT * FROM Module WHERE Code='AT-DASHBOARD')
BEGIN
	INSERT INTO Module(Code, CreatedBy, CreationDate, Name, ParentModuleId, RowStatus, SortOrder, URL)
	VALUES ('AT-DASHBOARD', 1, GETDATE(), 'Dashboard', @ModuleId, 1, 1, '/dashboard/dashboard')
END

-- ACTION

SELECT @ModuleId=ModuleId FROM Module where Code='AT-DASHBOARD'

IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.Dashboard.Search')
BEGIN
	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
	VALUES ('AT.Dashboard.Search', 'Search', 'Search', 'Button', @ModuleId, 1, 1, GETDATE())
END



-- PERSMISSION

INSERT INTO Permission(AmigoTenantTRoleId, ActionId)
select r.AmigoTenantTRoleId, a.ActionId
from AmigoTenantTRole r
	cross join Action a 
where r.RowStatus = 1
	and a.Code like 'AT.Dashboard%' 
	AND ltrim(r.AmigoTenantTRoleId) + '-' + ltrim(a.ActionId) NOT IN (SELECT ltrim(AmigoTenantTRoleId) + '-' + ltrim(ActionId) FROM Permission)

