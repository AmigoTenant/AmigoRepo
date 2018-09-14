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
    public class EntityStatusApplicationService : IEntityStatusApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<EntityStatusDTO> _entityStatusDataAccess;

        public EntityStatusApplicationService(IBus bus,
            IQueryDataAccess<EntityStatusDTO> entityStatusDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _entityStatusDataAccess = entityStatusDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<EntityStatusDTO>>> GetEntityStatusByEntityCodeAsync(string entityCode)
        {
            Expression<Func<EntityStatusDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(entityCode))
                queryFilter = queryFilter.And(p => p.EntityCode == entityCode);

            var entityStatus = await _entityStatusDataAccess.ListAsync(queryFilter);

            return ResponseBuilder.Correct(entityStatus.ToList());
        }

        public async Task<EntityStatusDTO> GetEntityStatusByEntityAndCodeAsync(string entityCode, string entityStatusCode)
        {
            Expression<Func<EntityStatusDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(entityCode))
                queryFilter = queryFilter.And(p => p.EntityCode == entityCode);

            if (!string.IsNullOrEmpty(entityStatusCode))
                queryFilter = queryFilter.And(p => p.Code == entityStatusCode);

            var entityStatus = await _entityStatusDataAccess.FirstOrDefaultAsync(queryFilter);

            return entityStatus;
        }

    }
}
