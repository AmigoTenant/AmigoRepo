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
    public class ServiceTypeApplicationService: IServiceTypeApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ServiceTypeDTO> _serviceTypeDataAcces;

        public ServiceTypeApplicationService(IBus bus,
            IQueryDataAccess<ServiceTypeDTO> serviceTypeDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _serviceTypeDataAcces = serviceTypeDataAcces;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PagedList<ServiceTypeDTO>>> SearchServiceTypeByNameAsync(ServiceTypeSearchRequest search)
        {
            Expression<Func<ServiceTypeDTO, bool>> queryFilter = c => c.RowStatus;

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));


            var product = await _serviceTypeDataAcces.ListPagedAsync(x => x.Name.StartsWith(search.Name), search.Page, search.PageSize);

            var pagedResult = new PagedList<ServiceTypeDTO>()
            {
                Items = product.Items,
                PageSize = product.PageSize,
                Page = product.Page,
                Total = product.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }
    }
}
