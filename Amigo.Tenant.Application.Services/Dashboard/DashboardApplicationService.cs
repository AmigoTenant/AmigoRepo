using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Dashboard;
using Amigo.Tenant.Application.DTOs.Requests.Dashboard;
using Amigo.Tenant.Common;
using Amigo.Tenant.Application.Services.Interfaces.Dashboard;

namespace Amigo.Tenant.Application.Services.Dashboard
{
    public class DashboardApplicationService : IDashboardApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<DashboardBalanceDTO> _dashboardDataAccess;
        private readonly IGeneralTableApplicationService _generalTableApplicationService;

        public DashboardApplicationService(IBus bus,
            IQueryDataAccess<DashboardBalanceDTO> dashboardDataAccess,
            IMapper mapper,
            IGeneralTableApplicationService generalTableApplicationService)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _dashboardDataAccess = dashboardDataAccess;
            _generalTableApplicationService = generalTableApplicationService;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<DashboardBalanceDTO>>> GetDashboardBalanceAsync(DashboardBalanceRequest request)
        {
            List<OrderExpression<DashboardBalanceDTO>> orderExpressionList = new List<OrderExpression<DashboardBalanceDTO>>();
            orderExpressionList.Add(new OrderExpression<DashboardBalanceDTO>(OrderType.Asc, p => p.Anio));
            orderExpressionList.Add(new OrderExpression<DashboardBalanceDTO>(OrderType.Asc, p => p.PeriodCode));

            Expression<Func<DashboardBalanceDTO, bool>> queryFilter = c => true;
            var frecuencyByAnualId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.Frecuency, Constants.GeneralTableCode.Frecuency.Anual);
            var frecuencyByPeriodId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.Frecuency, Constants.GeneralTableCode.Frecuency.Period);

            if (request.Frecuency.HasValue && !request.PeriodId.HasValue)
            {
                queryFilter = queryFilter.And(p => p.Anio == request.Frecuency);
            } else if (request.PeriodId.HasValue)
            {
                queryFilter = queryFilter.And(p => p.PeriodCode == request.PeriodId);
            }

            var dashboardBalance = await _dashboardDataAccess.ListAsync(queryFilter, orderExpressionList.ToArray());
            

            //FRECUENCIA ANUAL SE AGRUPA
            if (!request.Frecuency.HasValue && !request.PeriodId.HasValue)
            {
                var resultado = new List<DashboardBalanceDTO>();
                var query = dashboardBalance.ToList().GroupBy(
                balance => balance.Anio,
                (baseBalance, dashboardBalances) => new DashboardBalanceDTO()
                {
                    PeriodCode = baseBalance,
                    TotalExpenseAmount = dashboardBalances.Sum(q => q.TotalExpenseAmount),
                    TotalIncomePaidAmount = dashboardBalances.Sum(q => q.TotalIncomePaidAmount),
                    TotalIncomePendingAmount = dashboardBalances.Sum(q => q.TotalIncomePendingAmount)
                });
                resultado = query.ToList();
                return ResponseBuilder.Correct(resultado);
            }

            return ResponseBuilder.Correct(dashboardBalance.ToList());

        }

        private async Task<int?> GetGeneralTableIdByTableNameAndCode(string tableName, string tableCode)
        {
            var entity = await _generalTableApplicationService.GetGeneralTableByEntityAndCodeAsync(tableName, tableCode);
            if (entity != null)
                return entity.GeneralTableId;
            return null;
        }

    }
}
