using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class AmigoTenantParameterApplicationService: IAmigoTenantParameterApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<AmigoTenantParameterDTO> _amigoTenantParameterDataAcces;

        public AmigoTenantParameterApplicationService(IBus bus,
            IQueryDataAccess<AmigoTenantParameterDTO> amigoTenantParameterDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _amigoTenantParameterDataAcces = amigoTenantParameterDataAcces;
            _mapper = mapper;
        }

        public async Task<AmigoTenantParameterDTO> GetAmigoTenantParameterByCodeAsync(string code)
        {
            Expression<Func<AmigoTenantParameterDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(code))
                queryFilter = queryFilter.And(p => p.Code == code);

            var list = await _amigoTenantParameterDataAcces.FirstOrDefaultAsync(queryFilter);

            return list;

        }

        public async Task<ResponseDTO<List<AmigoTenantParameterDTO>>> GetAmigoTenantParameters()
        {
            Expression<Func<AmigoTenantParameterDTO, bool>> queryFilter = c => true;

            var list = (await _amigoTenantParameterDataAcces.ListAsync(queryFilter)).ToList();
            
            return ResponseBuilder.Correct(list);

        }

        public async Task<ResponseDTO<List<CustomAmigoTenantParameterDTO>>> SearchParametersForMobile()
        {
            Expression<Func<AmigoTenantParameterDTO, bool>> queryFilter = c => c.RowStatus;
            queryFilter.And(c => c.IsForMobile == "Y");
            var originalList = (await _amigoTenantParameterDataAcces.ListAsync(queryFilter)).ToList();
            var list = new List<CustomAmigoTenantParameterDTO>();
            originalList.ForEach(p =>
            {
                list.Add(new CustomAmigoTenantParameterDTO()
                {
                    Code = p.Code,
                    Value = p.Value
                });
            });

            return ResponseBuilder.Correct(list);
        }


    }
}
