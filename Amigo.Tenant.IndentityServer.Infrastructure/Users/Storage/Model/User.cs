using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model
{
    public sealed class User :IdentityUser<int,IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,IUser<int>
    {
        public User()
        {
            //this.Id = int.Newint();
        }

        public User(string userName)
          : this()
        {
            this.UserName = userName;
        }   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureUrl { get; set; }
        public bool? RowStatus { get; set; }
    }

    public class IdentityUserLogin : IdentityUserLogin<int>
    {
    }
    public class IdentityUserRole : IdentityUserRole<int>
    {
    }

    public class IdentityUserClaim : IdentityUserClaim<int>
    {
    }
}