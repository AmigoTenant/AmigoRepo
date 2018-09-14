using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
//using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.Tracking
{
   public class DispatchingPartyApplicationService : IDispatchingPartyApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<DispatchingPartyDTO> _DispatchingPartyDataAccess;

        public DispatchingPartyApplicationService(IBus bus, IQueryDataAccess<DispatchingPartyDTO> DispatchingPartyDataAccess, IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _DispatchingPartyDataAccess = DispatchingPartyDataAccess;
            _mapper = mapper;

        }

        public async Task<ResponseDTO<PagedList<DispatchingPartyDTO>>> SearchDispatchingPartyAsync(DispatchingPartySearchRequest search)
        {
            Expression<Func<DispatchingPartyDTO, bool>> queryFilter = c => c.RowStatus;

            if (search.DispatchingPartyId>0)
                queryFilter = queryFilter.And(p => p.DispatchingPartyId == search.DispatchingPartyId);

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Name));

            var DispatchingParty = await _DispatchingPartyDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            var pagedResult = new PagedList<DispatchingPartyDTO>()
            {
                Items = DispatchingParty.Items,
                PageSize = DispatchingParty.PageSize,
                Page = DispatchingParty.Page,
                Total = DispatchingParty.Total
            };

            return ResponseBuilder.Correct(pagedResult);

        }

        public async Task<ResponseDTO<List<DispatchingPartyDTO>>> GetAllAsync()
        {
            Expression<Func<DispatchingPartyDTO, bool>> queryFilter = c => c.RowStatus;

            var list = (await _DispatchingPartyDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);

        }



    }
}
