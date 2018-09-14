using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Commands.MasterData.MainTenants;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.MasterData
{
    public class MainTenantApplicationService : IMainTenantApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<MainTenantDTO> _mainTenantSearchDataAccess;
        private readonly IQueryDataAccess<MainTenantBasicDTO> _mainTenantSearchBasicDataAccess;

        public MainTenantApplicationService(IBus bus,
            IQueryDataAccess<MainTenantDTO> mainTenantSearchDataAccess,
            IQueryDataAccess<MainTenantBasicDTO> mainTenantDataAccess,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _mapper = mapper;
            _mainTenantSearchDataAccess = mainTenantSearchDataAccess;
            _mainTenantSearchBasicDataAccess = mainTenantDataAccess;
        }

        public async Task<ResponseDTO<PagedList<MainTenantDTO>>> SearchTenantAsync(MainTenantSearchRequest search)
        {
            List<OrderExpression<MainTenantDTO>> orderExpressionList = new List<OrderExpression<MainTenantDTO>>();
            orderExpressionList.Add(new OrderExpression<MainTenantDTO>(OrderType.Asc, p => p.FullName));

            Expression<Func<MainTenantDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            if (!string.IsNullOrEmpty(search.FullName))
                queryFilter = queryFilter.And(p => p.FullName.Contains(search.FullName));

            if (!string.IsNullOrEmpty(search.PhoneNumber))
                queryFilter = queryFilter.And(p => p.PhoneN01.Contains(search.PhoneNumber));

            if (search.CountryId.HasValue)
                queryFilter = queryFilter.And(p => p.CountryId == search.CountryId.Value);

            if (search.TypeId.HasValue)
                queryFilter = queryFilter.And(p => p.TypeId== search.TypeId.Value);

            if (search.RowStatus.HasValue)
                queryFilter = queryFilter.And(p => p.RowStatus == search.RowStatus);

            var mainTenant = await _mainTenantSearchDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<MainTenantDTO>()
            {
                Items = mainTenant.Items,
                PageSize = mainTenant.PageSize,
                Page = mainTenant.Page,
                Total = mainTenant.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<PagedList<MainTenantBasicDTO>>> SearchByCodeAndName(MainTenantCodeNameRequest search)
        {
            List<OrderExpression<MainTenantBasicDTO>> orderExpressionList = new List<OrderExpression<MainTenantBasicDTO>>();
            orderExpressionList.Add(new OrderExpression<MainTenantBasicDTO>(OrderType.Asc, p => p.FullName));

            Expression<Func<MainTenantBasicDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            if (!string.IsNullOrEmpty(search.FullName))
                queryFilter = queryFilter.And(p => p.FullName.Contains(search.FullName));

            var mainTenant = await _mainTenantSearchBasicDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<MainTenantBasicDTO>()
            {
                Items = mainTenant.Items,
                PageSize = mainTenant.PageSize,
                Page = mainTenant.Page,
                Total = mainTenant.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<List<MainTenantBasicDTO>>> SearchForTypeAhead(string search, bool validateInActiveContract)
        {
            List<OrderExpression<MainTenantBasicDTO>> orderExpressionList = new List<OrderExpression<MainTenantBasicDTO>>();
            orderExpressionList.Add(new OrderExpression<MainTenantBasicDTO>(OrderType.Asc, p => p.FullName));

            Expression<Func<MainTenantBasicDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(search))
                queryFilter = queryFilter.And(p => p.Code.ToUpper().Contains(search.ToUpper()) || p.FullName.ToUpper().Contains(search.ToUpper()));
            if (validateInActiveContract)
            {
                queryFilter = queryFilter.And(p =>  p.ContractStatusCode != Constants.EntityStatus.Contract.Formalized &&
                                                    p.ContractStatusCode != Constants.EntityStatus.Contract.Draft );
            }

            var mainTenant = await _mainTenantSearchBasicDataAccess.ListAsync(queryFilter, orderExpressionList.ToArray());

            return ResponseBuilder.Correct(mainTenant.ToList());
        }

        public async Task<ResponseDTO<MainTenantBasicDTO>> GetByIdTypeId(int? id, int? typeId)
        {
            Expression<Func<MainTenantBasicDTO, bool>> queryFilter = c => true;

            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.TenantId == id.GetValueOrDefault(0));

            //if (typeId.HasValue)
            //    queryFilter = queryFilter.And(p => p.TypeId == typeId.GetValueOrDefault(0));

            var mainTenant = await _mainTenantSearchBasicDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(mainTenant);
        }

        public async Task<ResponseDTO<MainTenantDTO>> GetById(int? id)
        {
            Expression<Func<MainTenantDTO, bool>> queryFilter = c => true;

            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.TenantId == id.GetValueOrDefault(0));

            var mainTenant = await _mainTenantSearchDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(mainTenant);
        }

        public async Task<ResponseDTO<MainTenantDTO>> GetLastCode()
        {
            List<OrderExpression<MainTenantDTO>> orderExpressionList = new List<OrderExpression<MainTenantDTO>>();
            orderExpressionList.Add(new OrderExpression<MainTenantDTO>(OrderType.Desc, p => p.Code));
            Expression<Func<MainTenantDTO, bool>> queryFilter = c => true;
            var mainTenant = await _mainTenantSearchDataAccess.FirstOrDefaultAsync(queryFilter, orderExpressionList.ToArray());

            return ResponseBuilder.Correct(mainTenant);
        }

        public async Task<ResponseDTO> RegisterMainTenantAsync(RegisterMainTenantRequest newMainTenant)
        {
            newMainTenant.Code = await GetNextCode();
            var command = _mapper.Map<RegisterMainTenantRequest, RegisterMainTenantCommand>(newMainTenant);

            var resp = await _bus.SendAsync(command);
            //var response = ResponseBuilder.Correct(resp);
            return ResponseBuilder.Correct(resp, command.TenantId, command.Code);

            //return response;
        }
        
        private async Task<string> GetNextCode()
        {
            var lastCode = await GetLastCode();
            var nextCode = "T" + (Convert.ToInt32((lastCode?.Data?.Code ?? "T00000").Substring(1)) + 1).ToString().PadLeft(5, '0');
            return nextCode;
        }

        public async Task<ResponseDTO> UpdateMainTenantAsync(UpdateMainTenantRequest mainTenant)
        {
            //Map to Command
            var command = _mapper.Map<UpdateMainTenantRequest, UpdateMainTenantCommand>(mainTenant);

            //Execute Command
            var resp = await _bus.SendAsync(command);
            var response = ResponseBuilder.Correct(resp);

            return response;
        }

        public async Task<ResponseDTO> DeleteMainTenantAsync(DeleteMainTenantRequest mainTenant)
        {
            //Map to Command
            var command = _mapper.Map<DeleteMainTenantRequest, DeleteMainTenantCommand>(mainTenant);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }
    }
}
