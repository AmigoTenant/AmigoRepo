using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IAmigoTenantParameterApplicationService
    {
        Task<AmigoTenantParameterDTO> GetAmigoTenantParameterByCodeAsync(string code);
        Task<ResponseDTO<List<AmigoTenantParameterDTO>>> GetAmigoTenantParameters();
        Task<ResponseDTO<List<CustomAmigoTenantParameterDTO>>> SearchParametersForMobile();
    }
}
