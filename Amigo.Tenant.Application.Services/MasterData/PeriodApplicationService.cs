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

namespace Amigo.Tenant.Application.Services.MasterData
{
    public class PeriodApplicationService : IPeriodApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<PeriodDTO> _periodDataAccess;

        public PeriodApplicationService(IBus bus,
            IQueryDataAccess<PeriodDTO> periodDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _periodDataAccess = periodDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<PeriodDTO>>> GetPeriodAllAsync()
        {
            Expression<Func<PeriodDTO, bool>> queryFilter = c => true;
            var periods = await _periodDataAccess.ListAsync(queryFilter);
            return ResponseBuilder.Correct(periods.ToList());
        }

        public async Task<ResponseDTO<List<PeriodDTO>>> GetPeriodLastPeriodsAsync(int periodNumber)
        {
            var currentPeriod = await GetCurrentPeriodAsync();
            Expression<Func<PeriodDTO, bool>> queryFilter = c => c.BeginDate <= currentPeriod.Data.BeginDate;
            //queryFilter = queryFilter.And(p => int.Parse(p.Code) <= int.Parse(currentPeriod.Data.Code));
            var periods = (await _periodDataAccess.ListAsync(queryFilter)).ToList();

            if (periodNumber != 0)
            {
                var filterPeriods = periods.OrderByDescending(q => q.Code).Take(periodNumber);
                return ResponseBuilder.Correct(filterPeriods.ToList());
            }

            return ResponseBuilder.Correct(periods.ToList());
        }


        public async Task<ResponseDTO<List<PeriodDTO>>> GetPeriodsByYearAsync(int? year)
        {
            Expression<Func<PeriodDTO, bool>> queryFilter = c => true;
            if (year.HasValue)
            {
                var dateBegin = new DateTime(year.Value, 1, 1);
                var dateEnd = new DateTime(year.Value,12, 31);
                queryFilter = queryFilter.And(c => c.BeginDate.Value >= dateBegin && c.BeginDate.Value <= dateEnd);
            }
            var periods = (await _periodDataAccess.ListAsync(queryFilter)).ToList();
            return ResponseBuilder.Correct(periods.ToList());
        }

        public async Task<ResponseDTO<List<YearDTO>>> GetYearsFromPeriodsAsync()
        {
            List<OrderExpression<PeriodDTO>> orderExpressionList = new List<OrderExpression<PeriodDTO>>();
            orderExpressionList.Add(new OrderExpression<PeriodDTO>(OrderType.Asc, p => p.Code));

            Expression<Func<PeriodDTO, bool>> queryFilter = c => true;
            var periods = (await _periodDataAccess.ListAsync(queryFilter, orderExpressionList.ToArray())).ToList();
            var query = periods.ToList().GroupBy(
                q => q.Code.Substring(0, 4),
                (baseYear, years) => new YearDTO()
                {
                    Anio = int.Parse(baseYear)
                }
                );
            var resultado = query.ToList();
            return ResponseBuilder.Correct(resultado);
        }

        public async Task<ResponseDTO<PeriodDTO>> GetPeriodByCodeAsync(string code)
        {
            Expression<Func<PeriodDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(code))
                queryFilter = queryFilter.And(p => p.Code == code);

            var period = await _periodDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(period);
        }

        public async Task<ResponseDTO<PeriodDTO>> GetPeriodByIdAsync(int? id)
        {
            Expression<Func<PeriodDTO, bool>> queryFilter = c => true;

            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.PeriodId == id);

            var period = await _periodDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(period);
        }

        public async Task<ResponseDTO<PeriodDTO>> GetPeriodBySequenceAsync(int? sequence)
        {
            Expression<Func<PeriodDTO, bool>> queryFilter = c => true;

            if (sequence.HasValue)
                queryFilter = queryFilter.And(p => p.Sequence == sequence);

            var period = await _periodDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(period);
        }

        public async Task<ResponseDTO<PeriodDTO>> GetLastPeriodAsync()
        {
            List<OrderExpression<PeriodDTO>> orderExpressionList = new List<OrderExpression<PeriodDTO>>();
            orderExpressionList.Add(new OrderExpression<PeriodDTO>(OrderType.Desc, p => p.BeginDate));

            Expression<Func<PeriodDTO, bool>> queryFilter = p => p.RowStatus;

            var period = await _periodDataAccess.FirstOrDefaultAsync(queryFilter, orderExpressionList.ToArray());

            return ResponseBuilder.Correct(period);
        }

        public async Task<ResponseDTO<List<PeriodDTO>>> SearchForTypeAhead(string search)
        {
            Expression<Func<PeriodDTO, bool>> queryFilter = c => c.Code.Contains(search) && c.RowStatus;

            var list = (await _periodDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

        public async Task<ResponseDTO<PeriodDTO>> GetCurrentPeriodAsync()
        {
            Expression<Func<PeriodDTO, bool>> queryFilter = c => true;
            queryFilter = queryFilter.And(p => p.Code == string.Format("{0:yyyyMM}", DateTime.Now.AddMonths(1)));
            var period = await _periodDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(period);
        }

        public async Task<PeriodDTO> GetInProcessPeriodAsync()
        {
            Expression<Func<PeriodDTO, bool>> queryFilter = c => true;
            queryFilter = queryFilter.And(p => p.Code == string.Format("{0:yyyyMM}", DateTime.Now));
            var period = await _periodDataAccess.FirstOrDefaultAsync(queryFilter);

            return period;
        }
    }
}
