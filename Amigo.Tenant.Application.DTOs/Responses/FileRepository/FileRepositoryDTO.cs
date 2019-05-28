using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.FileRepository
{
    public class FileRepositoryDTO: FileRepositoryBase
    {
        public DateTime? CreationDate { get; set; }
    }
}
