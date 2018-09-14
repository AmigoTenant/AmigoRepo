using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.Services.WebApi.Filters;
using Amigo.Tenant.Application.Services.WebApi.Validation.Fluent;
using Amigo.Tenant.Caching.Web.Filters;
using Amigo.Tenant.Common;
using AuthorizeAttribute = Amigo.Tenant.Security.Api.AuthorizeAttribute;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{

    [RoutePrefix("api/location"), CachingMasterData]
    public class LocationController : ApiController
    {
        private readonly ILocationApplicationService _locationApplicationService;

        public LocationController(ILocationApplicationService locationApplicationService)
        {
            _locationApplicationService = locationApplicationService;
        }


        [HttpGet, Route("search"), Validate, AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.LocationSearch)]
        public async Task<ResponseDTO<PagedList<LocationDTO>>> Search([FromUri]LocationSearchRequest search)
        {

            var resp = await _locationApplicationService.SearchLocationAsync(search);
            return resp;
        }

        [HttpGet, Route("get"), Validate]
        public async Task<ResponseDTO<LocationWithCoordinatesDTO>> Get([FromUri]GetLocationRequest getRequest)
        {

            var resp = await _locationApplicationService.GetLocationAsync(new GetLocationRequest { Code = getRequest.Code });
            return resp;
        }


        [HttpGet, Route("listCoordinates"), Validate]
        public async Task<ResponseDTO<List<LocationCoordinateDTO>>> ListCoordinates([FromUri]LocationCoordinatesListRequest search)
        {
            var resp = await _locationApplicationService.ListCoordinatesAsync(search);
            return resp;
        }


        [HttpGet, Route("searchLocationAll")]
        public  Task<ResponseDTO<List<LocationDTO>>> GetLocationAll()
        {
            var resp = _locationApplicationService.SearchLocationAll();
            return resp;
        }

        [HttpGet, Route("searchLocationAllTypeAhead")]
        public Task<ResponseDTO<List<LocationTypeAheadDTO>>> GetLocationAllTypeAhead()
        {
            var resp = _locationApplicationService.SearchLocationAllTypeAhead();
            return resp;
        }

        [HttpGet, Route("getCities")]
        public async Task<ResponseDTO<List<CityDTO>>> GetCities()
        {
            var resp = await _locationApplicationService.GetCitiesAsync();
            return resp;
        }

        [HttpGet, Route("getParentLocations")]
        public async Task<ResponseDTO<List<ParentLocationDTO>>> GetParentLocations()
        {
            var resp = await _locationApplicationService.GetParentLocationsAsync();
            return resp;
        }


        [HttpGet, Route("getLocationTypes")]
        public async Task<ResponseDTO<List<LocationTypeDTO>>> GetLocationTypes()
        {
            var resp = await _locationApplicationService.GetLocationTypesAsync();
            return resp;
        }

        [HttpPost, Route("register"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.LocationCreate)]
        public async Task<ResponseDTO> Register(RegisterLocationRequest location)
        {
            if (ModelState.IsValid)
            {
                return await _locationApplicationService.RegisterLocationAsync(location);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("update"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.LocationUpdate)]
        public async Task<ResponseDTO> Update(UpdateLocationRequest location)
        {
            if (ModelState.IsValid)
            {
                return await _locationApplicationService.UpdateLocationAsync(location);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("delete"), AmigoTenantClaimsAuthorize(ActionCode = ConstantsSecurity.ActionCode.LocationDelete)]
        public async Task<ResponseDTO> Delete(DeleteLocationRequest location)
        {
            if (ModelState.IsValid)
            {
                return await _locationApplicationService.DeleteLocationAsync(location);
            }
            return ModelState.ToResponse();
        }

        [HttpPost, Route("registerCoordinates")]
        public async Task<ResponseDTO> RegisterCoordinates(RegisterLocationCoordinatesRequest locationCoordinates)
        {
            if (ModelState.IsValid)
            {
                return await _locationApplicationService.RegisterLocationCoordinatesAsync(locationCoordinates);
            }
            return ModelState.ToResponse();
        }


        [HttpPost, Route("deleteCoordinates")]
        public async Task<ResponseDTO> DeleteCoordinates(DeleteLocationCoordinatesRequest location)
        {
            if (ModelState.IsValid)
            {
                return await _locationApplicationService.DeleteLocationCoordinatesAsync(location);
            }
            return ModelState.ToResponse();
        }

        [HttpGet, Route("searchLocationAllTypeAheadByName")]
        public Task<ResponseDTO<List<LocationTypeAheadDTO>>> GetLocationAllTypeAheadByName(string name)
        {
            var resp = _locationApplicationService.SearchLocationAllTypeAhead(name);
            return resp;
        }

        [HttpGet, Route("searchLocationByName")]
        public async Task<LocationTypeAheadDTO> SearchByName([FromUri]string name)
        {
            var resp = await _locationApplicationService.SearchLocationByNameAsync(name);
            return resp;
        }

    }
}
