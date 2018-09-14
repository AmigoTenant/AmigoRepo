using IdentityServer3.Core.Configuration;
using IdentityServer3.EntityFramework;
using Amigo.Tenant.IdentityServer.Infrastructure.Users;

namespace Amigo.Tenant.IdentityServer.Infrastructure.Storage.EntityFramework
{
    public static class Factory
    {
        public static IdentityServerServiceFactory Configure(string connString,string dsstorenam,string emaildomain)
        {
            var efConfig = new EntityFrameworkServiceOptions
            {
                ConnectionString = connString,
                Schema = Constants.Schema
            };
            
            var factory = new IdentityServerServiceFactory();           

            factory.RegisterConfigurationServices(efConfig);
            factory.RegisterOperationalServices(efConfig);            

            factory.ConfigureUserService(connString,dsstorenam,emaildomain);

            return factory;
        }        
    }
}