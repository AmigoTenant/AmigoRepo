using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Responses.Security;


namespace Amigo.Tenant.Application.DTOs.Requests.Security
{
  public  class RegisterRolRequest : AmigoTenantTRoleDTO
    {

      public List<string> ListActions { get; set; }

    }
}
