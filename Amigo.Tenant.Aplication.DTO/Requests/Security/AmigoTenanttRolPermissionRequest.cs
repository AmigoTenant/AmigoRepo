using System;
using Amigo.Tenant.Application.DTOs.Requests.Common;


namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
   public class AmigoTenanttRolPermissionRequest  : BaseStatusRequest
    {
       public string CodeRol { get; set; }
       public  string CodeAction { get; set; }
       public int ActionId { get; set; }

    }
}
