using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.FileRepository
{
    public class FileRepositoryEntityDTO: FileRepositoryBase
    {
        public byte[] UtMediaFile { get; set; }
    }
}
