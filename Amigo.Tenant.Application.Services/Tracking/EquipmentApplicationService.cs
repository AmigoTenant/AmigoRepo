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
   public class EquipmentApplicationService : IEquipmentApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<EquipmentDTO> _equipmentDataAccess;

        public EquipmentApplicationService(IBus bus, IQueryDataAccess<EquipmentDTO> equipmentDataAccess, IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _equipmentDataAccess = equipmentDataAccess;
            _mapper = mapper;   

        }

       public async Task<ResponseDTO<PagedList<EquipmentDTO>>> SearchEquipmentAsync(EquipmentSearchRequest search)
       {
           Expression<Func<EquipmentDTO, bool>> queryFilter = c => c.RowStatus;

           if (!string.IsNullOrEmpty(search.EquipmentNo))
               queryFilter = queryFilter.And(p => p.EquipmentNo.Contains(search.EquipmentNo));

           var equipment = await _equipmentDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);

           var pagedResult = new PagedList<EquipmentDTO>()
           {
               Items = equipment.Items,
               PageSize = equipment.PageSize,
               Page = equipment.Page,
               Total = equipment.Total
           };

           var response = ResponseBuilder.Correct(pagedResult);
           response.Messages = new List<ApplicationMessage>();
           response.Messages.Add(new ApplicationMessage()
           {
               Key = "Info",
               Message = !pagedResult.Items.Any() ? "No records found for this request" : ""
           });

           return response;
       }

    }
}
