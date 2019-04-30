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
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.MasterData
{
    public class BusinessPartnerApplicationService : IBusinessPartnerApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<BusinessPartnerDTO> _businessPartnerDataAccess;

        public BusinessPartnerApplicationService(IBus bus,
            IQueryDataAccess<BusinessPartnerDTO> businessPartnerDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _businessPartnerDataAccess = businessPartnerDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<List<BusinessPartnerDTO>>> GetBusinessPartnerByNameAsync(string tableName, string bpTypeCode)
        {
            Expression<Func<BusinessPartnerDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(tableName))
                queryFilter = queryFilter.And(p => p.Name == tableName && p.BPTypeCode == bpTypeCode && p.RowStatus);

            var businessPartner = await _businessPartnerDataAccess.ListAsync(queryFilter);

            return ResponseBuilder.Correct(businessPartner.ToList());
        }

        public async Task<ResponseDTO<List<BusinessPartnerDTO>>> GetBusinessPartnerByBPTypeCodeAsync(string bpTypeCode)
        {
            Expression<Func<BusinessPartnerDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(bpTypeCode))
                queryFilter = queryFilter.And(p => p.BPTypeCode == bpTypeCode && p.RowStatus);

            var businessPartner = await _businessPartnerDataAccess.ListAsync(queryFilter);

            return ResponseBuilder.Correct(businessPartner.ToList());
        }

    }
}
