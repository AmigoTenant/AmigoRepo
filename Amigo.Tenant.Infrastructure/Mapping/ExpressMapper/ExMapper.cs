using ExpressMapper;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Infrastructure.Mapping.ExpressMapper
{
    public class ExMapper: IMapper
    {
        public TDest Map<TSour, TDest>(TSour source)
        {
            return Mapper.Map<TSour, TDest>(source);
        }

        public TDest Map<TSour, TDest>(TSour source, TDest dest)
        {
            return Mapper.Map<TSour, TDest>(source, dest);
        }
    }
}
