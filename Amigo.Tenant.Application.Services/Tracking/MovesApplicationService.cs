using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class MovesApplicationService: IMovesApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public MovesApplicationService(IBus bus, IMapper mapper)
        {
            _bus = bus;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> RegisterAsync(RegisterMoveRequest move)
        {            
            //Map to Command
            var command = _mapper.Map<RegisterMoveRequest, RegisterMoveCommand>(move);
            
            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<PagedList<MoveResponse>>> ListPendentMoves(int page, int pageSize)
        {           

            return null;
        }
    }
}
