using System;


namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class MainMenuDTO
    {

        public int UserId
        {
            get; set;
        }

        public int RoleId
        {
            get; set;
        }

        public int PermissionId
        {
            get; set;
        }

        public int ActionId
        {
            get; set;
        }

        public string ActionCode
        {
            get; set;
        }

        public int ModuleId
        {
            get; set;
        }

        public string ModuleName
        {
            get; set;
        }

        public int ParentModuleId
        {
            get; set;
        }

        public string ParentModuleName
        {
            get; set;
        }

        public string Url
        {
            get; set;
        }

        public int SortOrder
        {
            get; set;
        }

        public int ParentSortOrder
        {
            get; set;
        }

        public string ModuleCode
        {
            get; set;
        }

        public string ParentModuleCode
        {
            get; set;
        }
    }
}
