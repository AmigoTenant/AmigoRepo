using IdentityServer3.Admin.EntityFramework;
using IdentityServer3.Admin.EntityFramework.Entities;
using Amigo.Tenant.IdentityServer.Infrastructure.Storage;

namespace Amigo.Tenant.IdentityServer.Infrastructure.ClientManagement
{
    public class IdentityAdminManagerService : IdentityAdminCoreManager<IdentityClient, int, IdentityScope, int>
    {
        public IdentityAdminManagerService()
            : base(Constants.ConnectionName, Constants.Schema)
        {
        }
    }
}
