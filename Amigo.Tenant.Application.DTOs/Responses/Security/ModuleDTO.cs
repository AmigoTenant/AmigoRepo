using System;


namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class ModuleDTO
    {

        public string Code
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Url
        {
            get; set;
        }

        public string ParentModuleCode
        {
            get; set;
        }

        public string ParentModuleName
        {
            get; set;
        }


        public int SortOrder
        {
            get; set;
        }

        public bool? OnlyParents
        {
            get; set;
        }
    }
}
