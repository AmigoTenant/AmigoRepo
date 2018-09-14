using System.Collections.Generic;

namespace Amigo.Tenant.Query.Common
{
    public class PagedList<T> where T:class 
    {        
        public int Page { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public IList<T> Items { get; set; }
    }
}
