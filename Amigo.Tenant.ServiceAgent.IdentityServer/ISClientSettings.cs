namespace Amigo.Tenant.ServiceAgent.IdentityServer
{
    /// <summary>
    /// Identity Server Client Settings
    /// </summary>
    public class ISClientSettings
    {
        public string SecurityAuthority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientScope { get; set; }
    }
}
