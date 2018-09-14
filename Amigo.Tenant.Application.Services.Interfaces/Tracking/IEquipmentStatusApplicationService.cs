using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IEquipmentStatusApplicationService
    {
        Task<ResponseDTO<PagedList<EquipmentStatusDTO>>> SearchEquipmentStatusByNameAsync(EquipmentStatusSearchRequest search);
        Task<ResponseDTO<List<EquipmentStatusDTO>>> SearchEquipmentStatusAll();
    }
}
