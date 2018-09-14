using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;


namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IActivityTypeApplicationService
    {
        Task<ActivityTypeDTO> SearchActivityByCodeAsync(string code);
        Task<ResponseDTO<List<ActivityTypeDTO>>> SearchActivityTypeAll();

    }
}
