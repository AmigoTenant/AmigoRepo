using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Linq;

using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class RoleMapping : Map<RoleDTO>
    {

        public RoleMapping()
        {
            PrimaryKey(x => x.AmigoTenantTRoleId);

            TableName("AmigoTenantTRole");

            Columns(x =>
            {

                x.Column(y => y.AmigoTenantTRoleId);
                x.Column(y => y.ModuleActions).Ignore();
                x.Column(y => y.TableStatus).Ignore();
                x.Column(y => y.RowStatus);
            });
        }

    }
}
