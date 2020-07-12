using System;
using System.Collections.Generic;


namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class ModuleActionsDTO
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


        public int SortOrder
        {
            get; set;
        }

        public List<ActionDTO> Actions { get; set; }

    }

    //public class ModuleTreeDTO: ModuleActionsDTO
    //{
    //    public ModuleTreeDTO()
    //    {            
    //    }
    //    public List<ModuleTreeDTO> Modules { get; set; }
    //}

    public class ModuleTreeDTO
    {
        public int ActionId { get; set; }
        public string Code{get; set;}
        public bool Enabled { get; set; }

        public string Name{get; set;}        

        public string ParentCode{get; set;}
        public ModuleTreeType ModuleTreeType { get; set; }

        public List<ModuleTreeDTO> Children { get; set; }
    }

    public enum ModuleTreeType:byte
    {
        Module=1,
        Action=2
    }
}
