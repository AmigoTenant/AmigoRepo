using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using Microsoft.AspNet.Identity;
using Amigo.Tenant.IdentityServer.Infrastructure.Users;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Manager;
using Amigo.Tenant.IdentityServer.Infrastructure.Users.Storage.Model;

namespace Amigo.Tenant.IdentityServer.Infrastructure.ExternalAuthentication.Windows
{    
    public class WindowsUserService : UserService
    {
        private readonly WindowsUserValidator _windowsUserValidator;
        public const string AcrKey = "type:";
        public const string AcrWindowsValue = "type:windows";
        public const string Provider = "Windows";

        public WindowsUserService(UserManager userMgr, WindowsUserValidator windowsUserValidator) : base(userMgr)
        {
            _windowsUserValidator = windowsUserValidator;
        }

        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext ctx)
        {
            if (ctx.SignInMessage.AcrValues != null && ctx.SignInMessage.AcrValues.Any(x => x.StartsWith(AcrKey)))
            {
                var type = ctx.SignInMessage.AcrValues.First(x => x.StartsWith(AcrKey));
                switch (type)
                {
                    case AcrWindowsValue:
                        await AuthenticateWindowsCredentials(ctx);
                        return;                        
                    default:
                        await base.AuthenticateLocalAsync(ctx);
                        return;
                }
            }
            await base.AuthenticateLocalAsync(ctx);            
        }

        protected async Task AuthenticateWindowsCredentials(LocalAuthenticationContext ctx)
        {
            var isValid = _windowsUserValidator.ValidateUserCredentials(ctx.UserName, ctx.Password);
            if(!isValid)return;

            var exists = await userManager.FindByNameAsync(ctx.UserName);
            if (exists==null)
            {
                var userProfile = _windowsUserValidator.GetProfileInfo(ctx.UserName);

                ctx.AuthenticateResult = await ProcessNewWindowsUserAsync(userProfile);
                return;
            }
            ctx.AuthenticateResult = await ProcessExistingWindowsUserAsync(exists);
        }

        private async Task<AuthenticateResult> ProcessExistingWindowsUserAsync(User user)
        {
            return await SignInFromExternalProviderAsync(user.Id, Provider);
        }

        private async Task<AuthenticateResult> ProcessNewWindowsUserAsync(WindowsUserInfo userinfo)
        {
            var user = await CreateNewUserFormWindowsLoginAsync(userinfo);
            var createdResult = await userManager.CreateAsync(user);
            if(!createdResult.Succeeded) return new AuthenticateResult(createdResult.Errors.First());

            var externalLogin = new UserLoginInfo(Provider,user.UserName);
            
            var addExternalResult = await userManager.AddLoginAsync(user.Id,externalLogin);
            if (!addExternalResult.Succeeded)
            {
                return new AuthenticateResult(addExternalResult.Errors.First());
            }            

            return await SignInFromExternalProviderAsync(user.Id,Provider);
        }

        private Task<User> CreateNewUserFormWindowsLoginAsync(WindowsUserInfo userinfo)
        {
            var user = new User(userinfo.UserName)
            {
                Email = userinfo.Email,
                EmailConfirmed = true,
                FirstName = userinfo.FirstName,
                LastName = userinfo.LastName
            };
            return Task.FromResult(user);
        }
    }
}