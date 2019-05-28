using Amigo.Tenant.Application.DTOs.Responses.Expense;
using Amigo.Tenant.Application.DTOs.Responses.FileRepository;
using NPoco.FluentMappings;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class FileRepositoryDtoMapping : Map<FileRepositoryDTO>
    {
        public FileRepositoryDtoMapping()
        {
            TableName("FileRepository");
        }
    }
}
