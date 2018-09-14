using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using System.Linq.Expressions;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.MasterData
{
    public class FeatureApplicationService : IFeatureApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<FeatureDTO> _featureDataAccess;

        public FeatureApplicationService(IBus bus, IQueryDataAccess<FeatureDTO> featureDataAccess, IMapper mapper) 
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _featureDataAccess = featureDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<FeatureDTO>>> SearchFeatureAll(string houseTypeCode)
        {
            Expression<Func<FeatureDTO, bool>> queryFilter = c => c.RowStatus;

            if (!string.IsNullOrEmpty(houseTypeCode))
                queryFilter = queryFilter.And(p => p.HouseTypeCode == houseTypeCode);

            var list = (await _featureDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }
    }
}
