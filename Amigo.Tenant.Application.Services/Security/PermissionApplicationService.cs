using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Commands.Security.Permission;
//using Amigo.Tenant.Commands.Security.Permission;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.Security
{
    public class PermissionApplicationService: IPermissionApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<PermissionDTO> _permissionDataAccess;

        public PermissionApplicationService(IBus bus, IQueryDataAccess<PermissionDTO> permissionDataAccess, 
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _permissionDataAccess = permissionDataAccess;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PagedList<PermissionDTO>>> SearchPermissionByCriteriaAsync(PermissionSearchRequest search)
        {
            var queryFilter = GetQueryFilter(search);
            var result = await _permissionDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);
            var pagedResult = new PagedList<PermissionDTO>()
            {
                Items = result.Items,
                PageSize = result.PageSize,
                Page = result.Page,
                Total = result.Total
            };
            return ResponseBuilder.Correct(pagedResult);
        }

        //public async Task<PermissionDTO> SearchUsersByIdAsync(UserSearchRequest search)
        //{
        //    var queryFilter = GetQueryFilter(search);
        //    var result = await _permissionDataAccess.FirstOrDefaultAsync(queryFilter);
        //    var user = result;
        //    return user;
        //}

        public async Task<bool> Exists(PermissionSearchRequest search) {
            var queryFilter = GetQueryFilter(search);
            var result = await _permissionDataAccess.AnyAsync(queryFilter);
            return result;
        }

        public async Task<ResponseDTO> Register(PermissionDTO dto)
        {
            //Map to Command
            var command = _mapper.Map<PermissionDTO, RegisterPermissionCommand>(dto);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        //public async Task<ResponseDTO> Update(PermissionDTO dto)
        //{
        //    //Map to Command
        //    var command = _mapper.Map<PermissionDTO, UpdatePermissionCommand>(dto);

        //    //Execute Command
        //    var resp = await _bus.SendAsync(command);

        //    return ResponseBuilder.Correct(resp);
        //}

        private Expression<Func<PermissionDTO, bool>> GetQueryFilter(PermissionSearchRequest search)
        {
            Expression<Func<PermissionDTO, bool>> queryFilter = p => true;

            if (search.PermissionId > 0)
                queryFilter = queryFilter.And(p => p.PermissionId == search.PermissionId);

            //if (!string.IsNullOrEmpty(search.ActionCode))
            //    queryFilter = queryFilter.And(p => p.ActionCode.Contains(search.ActionCode));

            //if (!string.IsNullOrEmpty(search.AmigoTenantTRoleCode))
            //    queryFilter = queryFilter.And(p => p.AmigoTenantTRoleCode.Contains(search.AmigoTenantTRoleCode));

            return queryFilter;
        }

        public async Task<ResponseDTO> Delete(PermissionStatusDTO dto)
        {
            //Map to Command
            var command = _mapper.Map<PermissionStatusDTO, DeletePermissionCommand>(dto);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }


    }
}
