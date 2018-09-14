using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/period")] //, CachingMasterData]
    public class PeriodController : ApiController
    {
        private readonly IPeriodApplicationService _periodApplicationService;

        public PeriodController(IPeriodApplicationService periodApplicationServices)
        {
            _periodApplicationService = periodApplicationServices;
        }

        [HttpGet, Route("getPeriodAll")]
        public async  Task<ResponseDTO<List<PeriodDTO>>> GetPeriodAll()
        {
            var resp = await _periodApplicationService.GetPeriodAllAsync();
            return resp;
        }

        [HttpGet, Route("searchForTypeAhead")]
        public async Task<ResponseDTO<List<PeriodDTO>>> SearchForTypeAhead(string search)
        {
            var resp = await _periodApplicationService.SearchForTypeAhead(search);
            return resp;
        }

        [HttpGet, Route("getById")]
        public async Task<ResponseDTO<PeriodDTO>> GetPeriodById(int? id)
        {
            var resp = await _periodApplicationService.GetPeriodByIdAsync(id);
            return resp;
        }

        [HttpGet, Route("getCurrentPeriod")]
        public async Task<ResponseDTO<PeriodDTO>> GetCurrentPeriod()
        {
            var resp = await _periodApplicationService.GetCurrentPeriodAsync();
            return resp;
        }
    }
}
