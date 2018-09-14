using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.ServiceAgent.IdentityServer;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface ILast24HoursApplicationService
    {
        ISClientSettings IdentityServerClientSettings { get; set; }
        Task<ResponseDTO<List<Last24HoursDTO>>> SearchAsync(Last24HoursRequest search);
    }
}
