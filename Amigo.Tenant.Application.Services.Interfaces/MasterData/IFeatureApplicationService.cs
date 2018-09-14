using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IFeatureApplicationService
    {
        Task<ResponseDTO<List<FeatureDTO>>> SearchFeatureAll(string houseTypeCode);
    }
}
