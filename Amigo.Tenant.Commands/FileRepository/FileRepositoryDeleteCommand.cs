using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.FileRepository
{
    public class FileRepositoryDeleteCommand: IAsyncRequest<CommandResult>
    {
        public int FileRepositoryId { get; set; }
    }
}
