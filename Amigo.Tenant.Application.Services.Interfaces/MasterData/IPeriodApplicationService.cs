using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IPeriodApplicationService
    {
        Task<ResponseDTO<List<PeriodDTO>>> GetPeriodAllAsync();
        Task<ResponseDTO<PeriodDTO>> GetPeriodByCodeAsync(string code);
        Task<ResponseDTO<PeriodDTO>> GetPeriodByIdAsync(int? id);
        Task<ResponseDTO<PeriodDTO>> GetPeriodBySequenceAsync(int? sequence);
        Task<ResponseDTO<PeriodDTO>> GetLastPeriodAsync();
        Task<ResponseDTO<List<PeriodDTO>>> SearchForTypeAhead(string search);
        Task<ResponseDTO<PeriodDTO>> GetCurrentPeriodAsync();

    }
}
