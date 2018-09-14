--IF NOT EXISTS(SELECT * FROM Module WHERE Code='LEASING')
--BEGIN
--	INSERT INTO Module(Code, CreatedBy, CreationDate, Name, ParentModuleId, RowStatus, SortOrder, URL)
--	VALUES ('LEASING', 1, GETDATE(), 'Leasing', null, 1, 6, null)
--END

--DECLARE @ModuleId INT
--set @ModuleId=0
--SELECT @ModuleId=ModuleId FROM Module where Code='LEASING'

--IF NOT EXISTS(SELECT * FROM Module WHERE Code='AT-RENTALAPP')
--BEGIN
--	INSERT INTO Module(Code, CreatedBy, CreationDate, Name, ParentModuleId, RowStatus, SortOrder, URL)
--	VALUES ('AT-RENTALAPP', 1, GETDATE(), 'Rental Application', @ModuleId, 1, 25, '/leasing/rentalApp')
--END

---- ACTION

--SELECT @ModuleId=ModuleId FROM Module where Code='AT-RENTALAPP'

--IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.RentalApp.Search')
--BEGIN
--	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
--	VALUES ('AT.RentalApp.Search', 'Search', 'Search', 'Button', @ModuleId, 1, 1, GETDATE())
--END

--IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.RentalApp.Create')
--BEGIN
--	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
--	VALUES ('AT.RentalApp.Create', 'Create', 'Create', 'Button', @ModuleId, 1, 1, GETDATE())
--END

--IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.RentalApp.Update')
--BEGIN
--	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
--	VALUES ('AT.RentalApp.Update', 'Update', 'Update', 'Button', @ModuleId, 1, 1, GETDATE())
--END

--IF NOT EXISTS(SELECT * FROM Action WHERE Code='AT.RentalApp.Delete')
--BEGIN
--	INSERT INTO Action (Code, Name, Description, Type, ModuleId, RowStatus, CreatedBy, CreationDate)
--	VALUES ('AT.RentalApp.Delete', 'Delete', 'Delete', 'Button', @ModuleId, 1, 1, GETDATE())
--END

---- PERSMISSION

--INSERT INTO Permission(AmigoTenantTRoleId, ActionId)
--select r.AmigoTenantTRoleId, a.ActionId
--from AmigoTenantTRole r
--	cross join Action a 
--where r.RowStatus = 1
--	and a.Code like 'AT.RentalApp%' 
--	AND ltrim(r.AmigoTenantTRoleId) + '-' + ltrim(a.ActionId) NOT IN (SELECT ltrim(AmigoTenantTRoleId) + '-' + ltrim(ActionId) FROM Permission)
