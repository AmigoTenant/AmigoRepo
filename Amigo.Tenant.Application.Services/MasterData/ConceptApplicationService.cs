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
    public class ConceptApplicationService : IConceptApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ConceptDTO> _conceptDataAccess;

        public ConceptApplicationService(IBus bus,
            IQueryDataAccess<ConceptDTO> conceptDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _conceptDataAccess = conceptDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<ConceptDTO>> GetConceptByCodeAsync(string code)
        {
            Expression<Func<ConceptDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(code))
                queryFilter = queryFilter.And(p => p.Code == code);

            var concept = await _conceptDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(concept);
        }

        public async Task<ResponseDTO<List<ConceptDTO>>> GetConceptsByTypeIdAsync(int? typeId)
        {
            Expression<Func<ConceptDTO, bool>> queryFilter = c => c.RowStatus == true;
            queryFilter = queryFilter.And(p => p.TypeId == typeId);

            var concept = await _conceptDataAccess.ListAsync(queryFilter);

            return ResponseBuilder.Correct(concept.ToList());
        }

        public async Task<ResponseDTO<List<ConceptDTO>>> GetConceptsByTypeCodeAsync(string code)
        {
            Expression<Func<ConceptDTO, bool>> queryFilter = c => c.RowStatus == true;
            queryFilter = queryFilter.And(p => p.TypeCode == code);

            var concept = await _conceptDataAccess.ListAsync(queryFilter);

            return ResponseBuilder.Correct(concept.ToList());
        }

        public async Task<ResponseDTO<List<ConceptDTO>>> GetConceptByTypeIdListAsync(List<string> idList)
        {
            Expression<Func<ConceptDTO, bool>> queryFilter = p=> p.RowStatus;

            var concept = await _conceptDataAccess.ListAsync(queryFilter);
            return ResponseBuilder.Correct(concept.Where(q => idList.Contains(q.TypeId.Value.ToString())).ToList());

        }

    }
}
