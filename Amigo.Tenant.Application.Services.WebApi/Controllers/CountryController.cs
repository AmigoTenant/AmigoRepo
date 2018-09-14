using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Caching.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Amigo.Country.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/country"), CachingMasterData]
    public class CountryController : ApiController
    {
        private readonly ICountryApplicationService _CountryApplicationService;

        public CountryController(ICountryApplicationService CountryApplicationService)
        {
            _CountryApplicationService = CountryApplicationService;
        }

        [HttpGet, Route("getCountriesAll")]
        public async Task<ResponseDTO<List<CountryDTO>>> GetCountriesAll()
        {
            var resp = await _CountryApplicationService.GetCountrieAll();
            return resp;
        }

    }
}
