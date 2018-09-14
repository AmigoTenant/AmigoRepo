using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
    public class ActivityTypeApplicationService : IActivityTypeApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ActivityTypeDTO> _activityTypeDataAcces;

        public ActivityTypeApplicationService(IBus bus,
            IQueryDataAccess<ActivityTypeDTO> activityTypeDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _activityTypeDataAcces = activityTypeDataAcces;
            _mapper = mapper;
        }

        public async Task<ActivityTypeDTO> SearchActivityByCodeAsync(string code)
        {
            Expression<Func<ActivityTypeDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(code))
                queryFilter = queryFilter.And(p => p.Code == code);

            var list = await _activityTypeDataAcces.FirstOrDefaultAsync(queryFilter);

            return list;

        }

        public async Task<ResponseDTO<List<ActivityTypeDTO>>> SearchActivityTypeAll()
        {
            Expression<Func<ActivityTypeDTO, bool>> queryFilter = c => c.RowStatus;

            var list = (await _activityTypeDataAcces.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list); 

        }
    }
}
