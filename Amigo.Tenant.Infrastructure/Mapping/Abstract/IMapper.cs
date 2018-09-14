using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Mapping.Abstract
{
    public interface IMapper
    {
        TDest Map<TSour, TDest>(TSour source);

        TDest Map<TSour,TDest>(TSour source, TDest dest);
    }
}
