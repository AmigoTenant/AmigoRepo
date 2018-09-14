namespace Amigo.Tenant.IdentityServer.DTOs.Requests.Common
{
    public abstract class PagedRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
