using Microsoft.AspNet.Identity.EntityFramework;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model
{
    public class Role: IdentityRole<int, IdentityUserRole>
    {
        public Role()
        {
           // this.Id = Guid.NewGuid();
        }

        public Role(string roleName): this()
        {
            this.Name = roleName;
        }
    }
}