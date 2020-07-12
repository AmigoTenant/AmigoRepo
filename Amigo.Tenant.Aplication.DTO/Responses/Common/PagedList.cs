using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.Common
{
    public class PagedList<T>
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public IList<T> Items { get; set; }
    }
}
