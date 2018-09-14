using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Responses.Common.Approve
{
    public class PagedListServices<T>
    {
        public int Page { get; set; }
        public int TotalApproved { get; set; }
        public int TotalRejected { get; set; }
        public int TotalPending { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
        public IList<T> Items { get; set; }

    }
}
