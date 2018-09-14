using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using System.Linq.Expressions;

namespace Amigo.Tenant.Application.Services.MasterData
{
    public class CountryApplicationService : ICountryApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<CountryDTO> _countryDataAccess;

        public CountryApplicationService(IBus bus,
            IQueryDataAccess<CountryDTO> countryDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _mapper = mapper;
            _countryDataAccess = countryDataAccess;
        }


        public async Task<ResponseDTO<List<CountryDTO>>> GetCountrieAll()
        {
            List<OrderExpression<CountryDTO>> orderExpressionList = new List<OrderExpression<CountryDTO>>();
            orderExpressionList.Add(new OrderExpression<CountryDTO>(OrderType.Asc, p => p.Name));

            Expression<Func<CountryDTO, bool>> queryFilter = c => true;

            var countries = await _countryDataAccess.ListAsync(queryFilter, orderExpressionList.ToArray());

            return ResponseBuilder.Correct(countries.ToList());
        }
    }
}
