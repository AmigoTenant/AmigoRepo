using Amigo.Tenant.Application.DTOs.Requests.Dashboard;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.Dashboard
{
    public interface IDashboardApplicationService
    {
        Task<ResponseDTO<List<DashboardBalanceDTO>>> GetDashboardBalanceAsync(DashboardBalanceRequest request);
    }
}
