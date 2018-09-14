using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Users
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser()
                {
                    Username = "ABC111",
                    Password = "xpo1234",
                    Enabled = true,
                    Subject = "Jesus Angulo",
                    Claims = new List<Claim>()
                    {
                        new Claim(Constants.ClaimTypes.Email, "jesus.angulo@outlook.com"),
                        new Claim(Constants.ClaimTypes.GivenName, "Jesus Angulo"),
                        new Claim(Constants.ClaimTypes.Role, "Admin"),
                        new Claim(Constants.ClaimTypes.Role, "User")
                    }
                }
            };
        }
    }
}