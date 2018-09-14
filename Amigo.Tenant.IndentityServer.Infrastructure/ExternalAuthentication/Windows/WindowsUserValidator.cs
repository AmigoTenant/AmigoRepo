using System;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;

namespace Amigo.Tenant.IdentityServer.Infrastructure.ExternalAuthentication.Windows
{
    public class WindowsUserValidator
    {
        private readonly string _dsstoreName;
        private readonly string _emaildomain;

        public WindowsUserValidator(string dsstoreName,string emaildomain)
        {
            _dsstoreName = dsstoreName;
            _emaildomain = emaildomain;
        }

        public bool ValidateUserCredentials(string user, string password)
        {
            {
                try
                {
                    using (var pc = new PrincipalContext(ContextType.Domain, _dsstoreName))
                    {
                        var isValid = pc.ValidateCredentials(user, password);

                        return isValid;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                }
            }
        }

        public bool Exists(string username)
        {
            using (var pc = new PrincipalContext(ContextType.Domain, _dsstoreName))
            {
                var up = UserPrincipal.FindByIdentity(pc, username);
                return up != null;
            }
        }

        public WindowsUserInfo GetProfileInfo(string username)
        {
            using (var pc = new PrincipalContext(ContextType.Domain,_dsstoreName))
            {
                var up = UserPrincipal.FindByIdentity(pc,username);
                if(up==null)throw new InvalidOperationException("User doesn't exist");

                var uinfo = new WindowsUserInfo()
                {
                    UserName = username,
                    FirstName = up.GivenName,
                    LastName = up.Surname,
                    Email = GetEmail(username)
                };

                return uinfo;                
            }
        }

        public string GetFullName(string user)
        {
            using (var context = new PrincipalContext(ContextType.Domain,_dsstoreName))
            {
                var usr = UserPrincipal.FindByIdentity(context,user);
                if (usr != null) return usr.DisplayName;
            }
            return null;
        }

        public string GetEmail(string user)
        {
            return $"{user}@{_emaildomain}".ToLowerInvariant();
        }
    }

    public class WindowsUserInfo
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
