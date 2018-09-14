using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Houses;
using Amigo.Tenant.Application.DTOs.Responses.Services;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Houses;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Caching.Web.Filters;
using Amigo.Tenant.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/house")] //, CachingMasterData]
    public class HouseController : ApiController
    {
        private readonly IHouseApplicationService _houseApplicationService;
        private readonly ILocationApplicationService _locationApplicationService;

        public HouseController(IHouseApplicationService houseApplicationServices,
            ILocationApplicationService locationApplicationService)
        {
            _houseApplicationService = houseApplicationServices;
            _locationApplicationService = locationApplicationService;
        }

        #region House

        [HttpGet, Route("searchHouseAll")]
        public async  Task<ResponseDTO<List<HouseDTO>>> SearchHouseAll()
        {
            var resp = await _houseApplicationService.SearchHouseAll();
            return resp;
        }

        [HttpGet, Route("searchForTypeAhead")]
        public async Task<ResponseDTO<List<HouseBasicDTO>>> SearchForTypeAhead(string search)
        {
            var resp = await _houseApplicationService.SearchForTypeAhead(search);
            return resp;
        }

        [HttpGet, Route("getById")]
        public async Task<ResponseDTO<HouseDTO>> GetById(int? id)
        {
            var resp = await _houseApplicationService.GetById(id);
            return resp;
        }

        [HttpGet, Route("searchCriteria")]
        public async Task<ResponseDTO<PagedList<HouseDTO>>> Search([FromUri]HouseSearchRequest search)
        {
            var resp = await _houseApplicationService.Search(search);
            return resp;
        }

        [HttpGet, Route("getHouseStatuses")]
        public async Task<ResponseDTO<List<HouseStatusDTO>>> GetHouseStatuses()
        {
            var resp = await _houseApplicationService.GetHouseStatusesAsync();
            return resp;
        }

        [HttpGet, Route("getHouseTypes")]
        public async Task<ResponseDTO<List<HouseTypeDTO>>> GetHouseTypes()
        {
            var resp = await _houseApplicationService.GetHouseTypesAsync();
            return resp;
        }

        [HttpGet, Route("get"), Validate]
        //public async Task<ResponseDTO<HouseWithCoordinatesDTO>> Get([FromUri]GetHouseRequest getRequest)
        public async Task<ResponseDTO<HouseDTO>> Get([FromUri]GetHouseRequest getRequest)
        {
            var resp = await _houseApplicationService.GetHouseAsync(new GetHouseRequest { Id = getRequest.Id});
            return resp;
        }


        [HttpPost, Route("register")]   //    , AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseCreate)]
        public async Task<ResponseDTO> Register(RegisterHouseRequest house)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.RegisterHouseAsync(house);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseUpdate)]
        public async Task<ResponseDTO> Update(UpdateHouseRequest house)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.UpdateHouseAsync(house);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete")]  //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseDelete)]
        public async Task<ResponseDTO> Delete(DeleteHouseRequest house)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.DeleteHouseAsync(house);
            }
            return ModelState.ToResponse();
        }

        #endregion

        #region HouseFeature

        [HttpGet, Route("getFeaturesByHouse")]
        public async Task<ResponseDTO<List<HouseFeatureDTO>>> GetFeaturesByHouse(int houseId)
        {
            var resp = await _houseApplicationService.GetFeaturesByHouseAsync(houseId);
            return resp;
        }
        
        [HttpGet, Route("searchHouseFeatureAndDetail")]
        public async Task<ResponseDTO<List<HouseFeatureAndDetailDTO>>> SearchHouseFeatureAndDetail(int? houseId, int? contractId)
        {
            var resp = await _houseApplicationService.SearchHouseFeatureAndDetailAsync(houseId, contractId);
            return resp;
        }

        [HttpPost, Route("registerFeature")]   //    , AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseCreate)]
        public async Task<ResponseDTO> RegisterFeature(HouseFeatureRequest houseFeature)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.RegisterHouseFeatureAsync(houseFeature);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("updateFeature")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseUpdate)]
        public async Task<ResponseDTO> UpdateFeature(HouseFeatureRequest houseFeature)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.UpdateHouseFeatureAsync(houseFeature);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("deleteFeature")]  //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseDelete)]
        public async Task<ResponseDTO> DeleteFeature(DeleteHouseFeatureRequest houseFeature)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.DeleteHouseFeatureAsync(houseFeature);
            }
            return ModelState.ToResponse();
        }

        #endregion

        #region HouseService

        [HttpGet, Route("getHouseService")]
        public async Task<ResponseDTO<List<HouseServiceDTO>>> GetHouseServiceByHouse(int houseId)
        {
            var resp = await _houseApplicationService.GetHouseServiceByHouseAsync(houseId);
            return resp;
        }

        [HttpGet, Route("getServicesHouseAll")]
        public async Task<ResponseDTO<List<ServiceHouseDTO>>> GetServicesHouseAll()
        {
            var resp = await _houseApplicationService.GetServicesHouseAllAsync();
            return resp;
        }
        
        [HttpGet, Route("getServicesHouseNew")]
        public async Task<ResponseDTO<List<ServiceHouseDTO>>> GetServicesHouseNew()
        {
            var resp = await _houseApplicationService.GetHouseServicesNewAsync();
            return resp;
        }

        [HttpPost, Route("registerHouseService")]   //    , AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseCreate)]
        public async Task<ResponseDTO> RegisterHouseService(HouseServiceRequest houseService)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.RegisterHouseServiceAsync(houseService);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("updateHouseService")] //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseUpdate)]
        public async Task<ResponseDTO> UpdateHouseService(HouseServiceRequest houseService)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.UpdateHouseServiceAsync(houseService);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("deleteHouseService")]  //, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.HouseDelete)]
        public async Task<ResponseDTO> DeleteHouseService(DeleteHouseServiceRequest houseService)
        {
            if (ModelState.IsValid)
            {
                return await _houseApplicationService.DeleteHouseServiceAsync(houseService);
            }
            return ModelState.ToResponse();
        }

        #endregion
    }
}
