using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Common;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/device")]
    public class DeviceController : ApiController
    {
        private readonly IDeviceApplicationService _deviceApplicationService;

        public DeviceController(IDeviceApplicationService deviceApplicationService)
        {
            _deviceApplicationService = deviceApplicationService;
        }


        [HttpGet, Route("search"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.DeviceSearch)]
        public async Task<ResponseDTO<PagedList<DeviceDTO>>> Search([FromUri]DeviceSearchRequest search)
        {
            var resp = await _deviceApplicationService.SearchDeviceAsync(search);
            return resp;
        }


        [HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.DeviceCreate)]
        public async Task<ResponseDTO> Register(RegisterDeviceRequest device)
        {
            if (ModelState.IsValid)
            {
                return await _deviceApplicationService.RegisterDeviceAsync(device);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.DeviceUpdate)]
        public async Task<ResponseDTO> Update(UpdateDeviceRequest device)
        {
            if (ModelState.IsValid)
            {
                return await _deviceApplicationService.UpdateDeviceAsync(device);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.DeviceDelete)]
        public async Task<ResponseDTO> Delete(DeleteDeviceRequest device)
        {
            if (ModelState.IsValid)
            {
                return await _deviceApplicationService.DeleteDeviceAsync(device);
            }
            return ModelState.ToResponse();
        }

        [HttpGet, Route("getModels")]
        public async Task<ResponseDTO<List<ModelDTO>>> GetModels()
        {
            var resp = await _deviceApplicationService.GetModelsAsync();
            return resp;
        }


        [HttpGet, Route("getOSVersions")]
        public async Task<ResponseDTO<List<OSVersionDTO>>> GetOSVersions()
        {
            var resp = await _deviceApplicationService.GetOSVersionsAsync();
            return resp;
        }

        [HttpGet, Route("getAppVersions")]
        public async Task<ResponseDTO<List<AppVersionDTO>>> GetAppVersions()
        {
            var resp = await _deviceApplicationService.GetAppVersionsAsync();
            return resp;
        }
    }
}