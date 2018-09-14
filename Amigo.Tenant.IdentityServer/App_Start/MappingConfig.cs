using System.Diagnostics;
using ExpressMapper;
using Amigo.Tenant.IdentityServer.ApplicationServices.Mapping.Profiles;

namespace Amigo.Tenant.IdentityServer
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            Register<UsersProfile>();

            Mapper.Compile();
        }

        private static void Register<T>() where T : new()
        {
            var instance = new T();
            Debug.WriteLine(instance);
        }
    }
}