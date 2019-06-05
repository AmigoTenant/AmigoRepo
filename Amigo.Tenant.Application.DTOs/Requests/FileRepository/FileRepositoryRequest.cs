using Amigo.Tenant.Application.DTOs.Responses.FileRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.FileRepository.Requests
{
    public class FileRepositoryRequest {
        public string EntityCode { get; set; }
        public int? ParentId { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
