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
   public class EquipmentTypeApplicationService : IEquipmentTypeApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<EquipmentTypeDTO> _equipmentTypeDataAccess;

        public EquipmentTypeApplicationService(IBus bus, IQueryDataAccess<EquipmentTypeDTO> equipmentTypeDataAccess, IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _equipmentTypeDataAccess = equipmentTypeDataAccess;
            _mapper = mapper;

        }

        public async Task<ResponseDTO<PagedList<EquipmentTypeDTO>>> SearchEquipmentTypeAsync(EquipmentTypeSearchRequest search)
        {
            Expression<Func<EquipmentTypeDTO, bool>> queryFilter = c => c.RowStatus;

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));

            var equipmentType = await _equipmentTypeDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            var pagedResult = new PagedList<EquipmentTypeDTO>()
            {
                Items = equipmentType.Items,
                PageSize = equipmentType.PageSize,
                Page = equipmentType.Page,
                Total = equipmentType.Total
            };

            return ResponseBuilder.Correct(pagedResult);

        }

        public async Task<ResponseDTO<List<EquipmentTypeDTO>>> SearchEquipmentType()
        {
            Expression<Func<EquipmentTypeDTO, bool>> queryFilter = c => c.RowStatus;

            var list = (await _equipmentTypeDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

    }
}
