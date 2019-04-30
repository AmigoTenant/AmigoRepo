using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IBusinessPartnerApplicationService
    {
        Task<ResponseDTO<List<BusinessPartnerDTO>>> GetBusinessPartnerByNameAsync(string name, string bpTypeCode);
        Task<ResponseDTO<List<BusinessPartnerDTO>>> GetBusinessPartnerByBPTypeCodeAsync(string bpTypeCode);
    }
}
