using Amigo.Tenant.Application.DTOs.Requests.Dashboard;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Dashboard;
using Amigo.Tenant.Application.Services.Interfaces.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Amigo.Tenant.Application.Services.WebApi.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {

        private readonly IDashboardApplicationService _dashboardApplicationService;

        public DashboardController(IDashboardApplicationService dashboardApplicationService)
        {
            _dashboardApplicationService = dashboardApplicationService;
        }

        [HttpPost, Route("getDashboardBalance")]
        public async Task<ResponseDTO<List<DashboardBalanceDTO>>> GetDashboardBalance(DashboardBalanceRequest request)
        {
            var resp = await _dashboardApplicationService.GetDashboardBalanceAsync(request);
            return resp;
        }

        
    }
}
