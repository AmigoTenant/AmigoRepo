
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public  class ParameterApplicationService : IParameterApplicationService
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ParameterDTO> _parameterDataAcces;

        public ParameterApplicationService(IBus bus,
            IQueryDataAccess<ParameterDTO> parameterDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _parameterDataAcces = parameterDataAcces;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<ParameterDTO>>> SearchParametersAll()
        {
            Expression<Func<ParameterDTO, bool>> queryFilter = c => c.RowStatus;

            var list = (await _parameterDataAcces.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

    }
}
