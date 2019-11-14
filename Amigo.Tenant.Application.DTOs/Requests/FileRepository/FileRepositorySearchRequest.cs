using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.FileRepository
{
    public class FileRepositorySearchRequest
    {
        public string EntityCode { get; set; }
        public List<int> ParentIds { get; set; }
    }
}
