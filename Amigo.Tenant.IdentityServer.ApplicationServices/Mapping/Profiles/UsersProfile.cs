using System.Security.Claims;
using ExpressMapper;
using Amigo.Tenant.IdentityServer.DTOs.Requests.Users;
using Amigo.Tenant.IdentityServer.DTOs.Responses.Users;
using Amigo.Tenant.IdentityServer.Infrastructure.ExternalAuthentication.Windows;
using Amigo.Tenant.IdentityServer.Infrastructure.Users;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;

namespace Amigo.Tenant.IdentityServer.ApplicationServices.Mapping.Profiles
{
    public class UsersProfile
    {
        public UsersProfile()
        {
            Mapper.Register<IdentityUserClaim, UserClaim>();

            Mapper.Register<AddClaimRequest, Claim>()
                .Instantiate(x => new Claim(x.ClaimType, x.ClaimValue));

            Mapper.Register<RemoveClaimRequest, Claim>()
                .Instantiate(x => new Claim(x.ClaimType, x.ClaimValue));

            Mapper.Register<User, UserResponse>();

            Mapper.Register<WindowsUserInfo,User>()
                .Member(x => x.UserName, y =>  y.UserName.ToUpper())
                .Member(x=> x.Email,y=> y.Email)
                .Value(x => x.EmailConfirmed,true)
                .Member(x => x.FirstName, y => y.FirstName)
                .Member(x => x.LastName, y => y.LastName);

            Mapper.Register<WindowsUserInfo, UserResponse>()
                .Member(x => x.UserName, y => y.UserName.ToUpper())
                .Member(x => x.Email, y => y.Email)                
                .Member(x => x.FirstName, y => y.FirstName)
                .Member(x => x.LastName, y => y.LastName);

            Mapper.Register<RegisterUserRequest,User>()
                .Member(x=> x.UserName, y=> y.UserName != null? y.UserName.ToUpper():null);
        }
    }
}
