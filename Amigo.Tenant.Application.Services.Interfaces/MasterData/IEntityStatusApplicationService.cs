using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IEntityStatusApplicationService
    {
        Task<ResponseDTO<List<EntityStatusDTO>>> GetEntityStatusByEntityCodeAsync(string entityCode);
        Task<EntityStatusDTO> GetEntityStatusByEntityAndCodeAsync(string entityCode, string entityStatusCode);
    }
}
