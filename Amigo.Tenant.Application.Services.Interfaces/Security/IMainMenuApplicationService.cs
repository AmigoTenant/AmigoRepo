using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;

namespace Amigo.Tenant.Application.Services.Interfaces.Security
{
    public interface IMainMenuApplicationService
    {
        Task<ResponseDTO<IEnumerable<MainMenuDTO>>> SearchMainMenuAsync(MainMenuSearchRequest search);

    }
}
