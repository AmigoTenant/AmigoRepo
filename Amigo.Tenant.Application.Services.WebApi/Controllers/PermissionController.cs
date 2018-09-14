using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/Permission")]
    public class PermissionController: ApiController
    {
        private readonly IPermissionApplicationService _PermissionApplicationService;

        public PermissionController(IPermissionApplicationService PermissionApplicationService)
        {
            _PermissionApplicationService = PermissionApplicationService;
        }

        [HttpPost, Route("search")]
        public async Task<ResponseDTO<PagedList<PermissionDTO>>> Search(PermissionSearchRequest search)
        {
            var resp = await _PermissionApplicationService.SearchPermissionByCriteriaAsync(search);
            return resp;
        }

        //[HttpPost, Route("searchCode")]
        //public async Task<PermissionDTO> SearchById(PermissionearchRequest search)
        //{
        //    var resp = await _PermissionApplicationService.SearchPermissionByIdAsync(search);
        //    return resp;
        //}


        [HttpPost, Route("exists")]
        public async Task<bool> Exists(PermissionSearchRequest search)
        {
            var resp = await _PermissionApplicationService.Exists(search);
            return resp;
        }

        [HttpPost, Route("register")]
        public async Task<ResponseDTO> Register(PermissionDTO dto)
        {
            if (ModelState.IsValid)
            {
                return await _PermissionApplicationService.Register(dto);
            }
            return ModelState.ToResponse();
        }

        //[HttpPost, Route("update")]
        //public async Task<ResponseDTO> Update(PermissionDTO dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        return await _PermissionApplicationService.Update(dto);
        //    }
        //    return ModelState.ToResponse();
        //}

        [HttpPost, Route("delete")]
        public async Task<ResponseDTO> Delete(PermissionStatusDTO dto)
        {
            if (ModelState.IsValid)
            {
                //PermissionDTO dto = new PermissionDTO();
                //dto.PermissionId = deleteDto.PermissionId;
                //dto.RowStatus = 0;
                ////TODO: Auditory Fields
                return await _PermissionApplicationService.Delete(dto);
            }
            return ModelState.ToResponse();
        }

    }
}