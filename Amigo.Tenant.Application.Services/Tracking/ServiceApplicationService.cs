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
    public class ServiceApplicationService: IServiceApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ServiceDTO> _serviceDataAcces;

        public ServiceApplicationService(IBus bus,
            IQueryDataAccess<ServiceDTO> serviceDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _serviceDataAcces = serviceDataAcces;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PagedList<ServiceDTO>>> SearchServiceByNameAsync(ServiceSearchRequest search)
        {
            Expression<Func<ServiceDTO, bool>> queryFilter = c => c.RowStatus;

            if (!string.IsNullOrEmpty(search.ServiceTypeCode))
                queryFilter = queryFilter.And(p => p.ServiceTypeCode == search.ServiceTypeCode);


            var product = await _serviceDataAcces.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            var pagedResult = new PagedList<ServiceDTO>()
            {
                Items = product.Items,
                PageSize = product.PageSize,
                Page = product.Page,
                Total = product.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<List<ServiceDTO>>> GetServiceAll()
        {
            Expression<Func<ServiceDTO, bool>> queryFilter = c => true;

            var list = (await _serviceDataAcces.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }


    }
}
