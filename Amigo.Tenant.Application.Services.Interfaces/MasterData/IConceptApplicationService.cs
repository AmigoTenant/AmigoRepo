using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IConceptApplicationService
    {
        Task<ResponseDTO<ConceptDTO>> GetConceptByCodeAsync(string code);
        Task<ResponseDTO<List<ConceptDTO>>> GetConceptsByTypeIdAsync(int? typeId);
        Task<ResponseDTO<List<ConceptDTO>>> GetConceptsByTypeCodeAsync(string code);
        Task<ResponseDTO<List<ConceptDTO>>> GetConceptByTypeIdListAsync(List<string> idList);

    }
}
