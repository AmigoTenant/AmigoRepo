using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.UtilityBills;
using Amigo.Tenant.Application.Services.Interfaces.UtilityBills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/utilitybill")]
    public class UtilityBillController : ApiController
    {
        private readonly IUtilityBillApplicationService _utilityBillApplicationService;

        public UtilityBillController(IUtilityBillApplicationService utilityBillApplicationService)
        {
            _utilityBillApplicationService = utilityBillApplicationService;
        }

        [HttpGet, Route("getHouseService")]
        public async Task<ResponseDTO<List<UtilityBillDTO>>> GetHouseServices(int houseId, int year)
        {
            var resp = await _utilityBillApplicationService.GetHouseServices(houseId, year);
            return resp;
        }


    }
}
