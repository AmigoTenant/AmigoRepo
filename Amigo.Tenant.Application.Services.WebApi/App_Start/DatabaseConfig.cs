using Amigo.Tenant.Infrastructure.Persistence.NPoco.Abstract;

namespace Amigo.Tenant.Application.Services.WebApi
{
    public static class DatabaseConfig
    {
        public static void Configure()
        {
            NPocoDatabaseFactory.Setup();
        }
    }
}