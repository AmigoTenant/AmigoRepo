using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Application.Services.WebApi.Helpers.Identity;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/mainMenu")]
    public class MainMenuController : ApiController
    {
        private readonly IMainMenuApplicationService _mainMenuApplicationService;

        public MainMenuController(IMainMenuApplicationService mainMenuApplicationService)
        {
            _mainMenuApplicationService = mainMenuApplicationService;
        }
        
        [HttpGet, Route("search")]
        public async Task<ResponseDTO<IEnumerable<MainMenuDTO>>> Search([FromUri]MainMenuSearchRequest search)
         {
            //search.UserId = ControllerContext.RequestContext.Principal.Identity.GetUserId();
            search.UserId = User.Identity.GetUserId();
            return await _mainMenuApplicationService.SearchMainMenuAsync(search);
        }

    }
}
