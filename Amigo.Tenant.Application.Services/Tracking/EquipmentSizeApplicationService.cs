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
   public class EquipmentSizeApplicationService : IEquipmentSizeApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<EquipmentSizeDTO> _equipmentSizeDataAccess;

        public EquipmentSizeApplicationService(IBus bus, IQueryDataAccess<EquipmentSizeDTO> equipmentSizeDataAccess, IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _equipmentSizeDataAccess = equipmentSizeDataAccess;
            _mapper = mapper;

        }

        public async Task<ResponseDTO<PagedList<EquipmentSizeDTO>>> SearchEquipmentSizeAsync(EquipmentSizeSearchRequest search)
        {
            Expression<Func<EquipmentSizeDTO, bool>> queryFilter = c => c.RowStatus;

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));

            var equipmentSize = await _equipmentSizeDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

            var pagedResult = new PagedList<EquipmentSizeDTO>()
            {
                Items = equipmentSize.Items,
                PageSize = equipmentSize.PageSize,
                Page = equipmentSize.Page,
                Total = equipmentSize.Total
            };

            return ResponseBuilder.Correct(pagedResult);

        }

        public async  Task<ResponseDTO<List<EquipmentSizeDTO>>> SearchEquipmentSizeAll()
        {
            Expression<Func<EquipmentSizeDTO, bool>> queryFilter = c => true;

            var list = (await _equipmentSizeDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

        
    }
}
