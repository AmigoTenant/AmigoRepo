using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    public class MovesController : ApiController
    {
        private readonly IMovesApplicationService _movesApplication;        
        public MovesController(IMovesApplicationService movesApplication)
        {
            _movesApplication = movesApplication;            
        }

        [HttpPost]
        public async Task<ResponseDTO> Register(RegisterMoveRequest move)
        {
            if (ModelState.IsValid)
            {
                return await _movesApplication.RegisterAsync(move);
            }            
            return ModelState.ToResponse();
        }
    }
}
