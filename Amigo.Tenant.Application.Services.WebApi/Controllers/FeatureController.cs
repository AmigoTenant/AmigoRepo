using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Caching.Web.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/feature"), CachingMasterData]
    public class FeatureController : ApiController
    {
        private readonly IFeatureApplicationService _featureApplicationService;

        public FeatureController(IFeatureApplicationService featureApplicationServices)
        {
            _featureApplicationService = featureApplicationServices;
        }

        [HttpGet, Route("searchFeatureAll")]
        public async  Task<ResponseDTO<List<FeatureDTO>>> SearchFeatureAll(string houseTypeCode)
        {
            var resp = await _featureApplicationService.SearchFeatureAll(houseTypeCode);
            return resp;
        }

        //[HttpGet, Route("getFeaturesByHouseType")]
        //public async Task<ResponseDTO<List<FeatureDTO>>> GetFeaturesByHouseType(int houseTypeId)
        //{
        //    //var resp = await _featureApplicationService.GetFeaturesByHouseType(houseTypeId);
        //    //return resp;
        //}
    }
}
