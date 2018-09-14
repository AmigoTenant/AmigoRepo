using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Events.Security
{
    public enum RoleEditionEventType:byte
    {
        Register,
        Update,
        Delete
    }
}
