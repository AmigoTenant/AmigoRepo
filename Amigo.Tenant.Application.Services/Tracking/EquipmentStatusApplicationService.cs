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
    public class EquipmentStatusApplicationService : IEquipmentStatusApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<EquipmentStatusDTO> _statusDataAcces;

        public EquipmentStatusApplicationService(IBus bus,
            IQueryDataAccess<EquipmentStatusDTO> statusDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _statusDataAcces = statusDataAcces;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PagedList<EquipmentStatusDTO>>> SearchEquipmentStatusByNameAsync(EquipmentStatusSearchRequest search)
        {
            Expression<Func<EquipmentStatusDTO, bool>> queryFilter = c => c.RowStatus;

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));


            var product = await _statusDataAcces.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            var pagedResult = new PagedList<EquipmentStatusDTO>()
            {
                Items = product.Items,
                PageSize = product.PageSize,
                Page = product.Page,
                Total = product.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<List<EquipmentStatusDTO>>> SearchEquipmentStatusAll()
        {
            Expression<Func<EquipmentStatusDTO, bool>> queryFilter = c => true;

            var list = (await _statusDataAcces.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

    }
}
