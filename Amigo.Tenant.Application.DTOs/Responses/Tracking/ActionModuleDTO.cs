

using System;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
  public class ActionModuleDTO
    {
        public int ModuleId { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string ActionCode { get; set; }
        public bool ActionRowStatus { get; set; }

    }
}
