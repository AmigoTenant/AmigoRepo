using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Tracking;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;
using Amigo.Tenant.Application.Services.Common.Extensions;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Commands.Security.CostCenter;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class CostCenterApplicationService : ICostCenterApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<CostCenterDTO> _costCenterDataAccess;

        public CostCenterApplicationService(IBus bus,
            IQueryDataAccess<CostCenterDTO> costCenterDataAccess,
            IMapper mapper)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _costCenterDataAccess = costCenterDataAccess;
            _mapper = mapper;

        }

        public async Task<ResponseDTO<PagedList<CostCenterDTO>>> SearchCostCenterByNameAsync(CostCenterSearchRequest search)
        {
            List<OrderExpression<CostCenterDTO>> orderExpressionList = new List<OrderExpression<CostCenterDTO>>();
            orderExpressionList.Add(new OrderExpression<CostCenterDTO>(OrderType.Desc, p => p.CostCenterId));

            Expression<Func<CostCenterDTO, bool>> queryFilter = c => c.RowStatus;

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            var costCenter = await _costCenterDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<CostCenterDTO>()
            {
                Items = costCenter.Items,
                PageSize = costCenter.PageSize,
                Page = costCenter.Page,
                Total = costCenter.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<List<CostCenterDTO>>> SearchCostCenterAll()
        {
            Expression<Func<CostCenterDTO, bool>> queryFilter = c => true;

            var list = (await _costCenterDataAccess.ListAsync(queryFilter)).ToList();

            return ResponseBuilder.Correct(list);
        }

        public async Task<ResponseDTO<List<CostCenterTypeAheadDTO>>> SearchCostCenterAllTypeAhead(string name)
        {
            Expression<Func<CostCenterDTO, bool>> queryFilter = c => true;
            if (!string.IsNullOrEmpty(name))
            {
                queryFilter = queryFilter.And(p => p.Name.Contains(name));
            }

            var list = (await _costCenterDataAccess.ListAsync(queryFilter)).ToList();
            var typeAheadList = list.Select(x => new CostCenterTypeAheadDTO() { CostCenterIdId = x.CostCenterId, Code = x.Code, Name = x.Name }).ToList();
            return ResponseBuilder.Correct(typeAheadList);

        }

        public async Task<CostCenterTypeAheadDTO> SearchCostCenterByNameAsync(string name)
        {
            Expression<Func<CostCenterDTO, bool>> queryFilter = c => true;
            if (!string.IsNullOrEmpty(name))
            {
                queryFilter = queryFilter.And(p => p.Name.Contains(name));
            }
            var typeAheadElement = new CostCenterTypeAheadDTO();
            var location = (await _costCenterDataAccess.FirstOrDefaultAsync(queryFilter));
            if (location != null)
            {
                typeAheadElement.CostCenterIdId = location.CostCenterId;
                typeAheadElement.Code = location.Code;
                typeAheadElement.Name = location.Name;
            }

            return typeAheadElement;
        }

        public async Task<ResponseDTO> RegisterModuleAsync(CostCenterDTO costCenter)
        {
            var command = _mapper.Map<CostCenterDTO, RegisterCostCenterCommand>(costCenter);
            var resp = await _bus.SendAsync(command);
            return resp.ToResponse();
        }

        public async Task<ResponseDTO> UpdateCostCenterAsync(CostCenterDTO costCenter)
        {
            var command = _mapper.Map<CostCenterDTO, UpdateCostCenterCommand>(costCenter);
            var response = await ValidateCodeAndNameFromCostCenter(costCenter.Code, costCenter.Name);
            if (response.IsValid)
            {
                var resp = await _bus.SendAsync(command);
                ResponseBuilder.Correct(resp);
            }
            return response;
        }

        public async Task<ResponseDTO> DeleteCostCenterAsync(CostCenterDTO costCenter)
        {
            //Map to Command
            var command = _mapper.Map<CostCenterDTO, DeleteCostCenterCommand>(costCenter);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        private async Task<ResponseDTO> ValidateCodeAndNameFromCostCenter(string code, string name)
        {
            Expression<Func<CostCenterDTO, bool>> queryFilter = p => true;
            queryFilter = queryFilter.And(p => p.Code == code.Trim());
            queryFilter = queryFilter.And(p => p.Name == name.Trim());
            var costCenter = await _costCenterDataAccess.FirstOrDefaultAsync(queryFilter);
            var result = "";
            if (costCenter != null)
            {
                result = "The cost center is already registered";
            }

            var response = new ResponseDTO()
            {
                IsValid = string.IsNullOrEmpty(result),
                Messages = new List<ApplicationMessage>()
            };

            response.Messages.Add(new ApplicationMessage()
            {
                Key = string.IsNullOrEmpty(result) ? "Ok" : "Error",
                Message = result
            });

            return response;
        }
    }
}
