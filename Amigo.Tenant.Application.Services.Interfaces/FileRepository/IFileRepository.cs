using Amigo.Tenant.Application.DTOs.FileRepository.Requests;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.FileRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.FileRepository
{
    public interface IFileRepositoryApplicationService
    {
        Task<ResponseDTO<PagedList<FileRepositoryDTO>>> GetFileRepositoriesAsync(string entityCode, int? fileRepositoryId);
        Task<ResponseDTO> RegisterAsync(FileRepositoryEntityDTO fileRepositoryEntityDtoRequest);
        Task<FileRepositoryEntityDTO> GetFileRepositoryByIdAsync(int fileRepositoryId);
        Task<bool> DeleteAsync(int fileRepositoryId);
    }
}
