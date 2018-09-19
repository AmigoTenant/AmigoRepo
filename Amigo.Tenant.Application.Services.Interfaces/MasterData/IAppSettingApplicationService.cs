using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.Interfaces.MasterData
{
    public interface IAppSettingApplicationService
    {
        Task<AppSettingDTO> GetAppSettingByCodeAsync(string appSettingCode);
    }
}
