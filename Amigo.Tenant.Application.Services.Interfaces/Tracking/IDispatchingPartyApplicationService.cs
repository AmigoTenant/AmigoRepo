using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IDispatchingPartyApplicationService
    {
        Task<ResponseDTO<PagedList<DispatchingPartyDTO>>> SearchDispatchingPartyAsync(DispatchingPartySearchRequest search);
        Task<ResponseDTO<List<DispatchingPartyDTO>>> GetAllAsync();
    }
}