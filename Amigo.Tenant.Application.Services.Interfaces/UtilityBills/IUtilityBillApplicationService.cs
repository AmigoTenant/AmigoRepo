using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.UtilityBills;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.UtilityBills
{
    public interface IUtilityBillApplicationService
    {
        Task<ResponseDTO<List<UtilityBillDTO>>> GetHouseServices(int houseId, int year);
    }
}