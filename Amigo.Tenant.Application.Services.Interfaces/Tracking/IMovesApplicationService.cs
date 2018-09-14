using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Application.Services.Interfaces.Tracking
{
    public interface IMovesApplicationService
    {
        Task<ResponseDTO> RegisterAsync(RegisterMoveRequest move);

        Task<ResponseDTO<PagedList<MoveResponse>>> ListPendentMoves(int page,int pageSize);
    }
}
