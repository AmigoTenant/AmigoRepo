using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IMainTenantApplicationService
    {
        Task<ResponseDTO<PagedList<MainTenantDTO>>> SearchTenantAsync(MainTenantSearchRequest search);

        Task<ResponseDTO<PagedList<MainTenantBasicDTO>>> SearchByCodeAndName(MainTenantCodeNameRequest search);

        Task<ResponseDTO<MainTenantDTO>> GetById(int? id);

        Task<ResponseDTO<MainTenantBasicDTO>> GetByIdTypeId(int? id, int? typeId);

        Task<ResponseDTO<List<MainTenantBasicDTO>>> SearchForTypeAhead(string search, bool validateInActiveContract);

        Task<ResponseDTO> RegisterMainTenantAsync(RegisterMainTenantRequest newMainTenant);

        Task<ResponseDTO> UpdateMainTenantAsync(UpdateMainTenantRequest mainTenant);

        Task<ResponseDTO> DeleteMainTenantAsync(DeleteMainTenantRequest mainTenant);
    }
}
