using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IAmigoTenantTEventLogApplicationService
    {
        Task<ResponseDTO> RegisterAmigoTenantTEventLogAsync(AmigoTenantTEventLogDTO maintenance);
        Task<ResponseDTO<PagedList<AmigoTenantTEventLogSearchResultDTO>>> SearchAsync(AmigoTenantTEventLogSearchRequest search);
       

    }
}
