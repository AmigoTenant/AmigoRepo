using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Commands.Security.Permission;
using Amigo.Tenant.Commands.Security.AmigoTenantTRole;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using Amigo.Tenant.Common;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Application.Services.Security
{
    public class AmigoTenantTRoleApplicationService: IAmigoTenantTRoleApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<AmigoTenantTRoleDTO> _dataAccess;
        private readonly IQueryDataAccess<AmigoTenantTRoleBasicDTO> _dataAccessBasic;
        private readonly IQueryDataAccess<RoleDTO> _rolDataAccess;
        private readonly IQueryDataAccess<ModuleDTO> _moduleDataAcces;
        private readonly IQueryDataAccess<PermissionDTO> _permissionDataAccess;
        private readonly IQueryDataAccess<ActionDTO> _actionDataAcces;

        public AmigoTenantTRoleApplicationService(IBus bus, IQueryDataAccess<AmigoTenantTRoleDTO> dataAccess,
            IQueryDataAccess<AmigoTenantTRoleBasicDTO> dataAccessBasic,
            IQueryDataAccess<RoleDTO> rolDataAccess,
            IQueryDataAccess<ModuleDTO> moduleDataAcces,
            IQueryDataAccess<PermissionDTO> permissionDataAccess,
            IQueryDataAccess<ActionDTO> actionDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _dataAccess = dataAccess;
            _dataAccessBasic = dataAccessBasic;
            _rolDataAccess = rolDataAccess;
            _moduleDataAcces = moduleDataAcces;
            _permissionDataAccess = permissionDataAccess;
            _actionDataAcces = actionDataAcces;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<PagedList<AmigoTenantTRoleDTO>>> SearchAmigoTenantTRoleByCriteriaAsync(AmigoTenantTRoleSearchRequest search)
        {

            var queryFilter = GetQueryFilter(search);
            var result = await _dataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize);
            var pagedResult = new PagedList<AmigoTenantTRoleDTO>()
            {
                Items = result.Items,
                PageSize = result.PageSize,
                Page = result.Page,
                Total = result.Total
            };
            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<PagedList<AmigoTenantTRoleBasicDTO>>> SearchAmigoTenantTRoleBasicCriteriaAsync(AmigoTenantTRoleSearchRequest search)
        {

            var queryFilter = GetQueryFilterBasic(search);
            var result = await _dataAccessBasic.ListPagedAsync(queryFilter, search.Page, search.PageSize);
            var pagedResult = new PagedList<AmigoTenantTRoleBasicDTO>()
            {
                Items = result.Items,
                PageSize = result.PageSize,
                Page = result.Page,
                Total = result.Total
            };
            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<List<ModuleTreeDTO>>> GetModuleActionAsyn()
        {
            // ModuleTreeDTO
            var modules = (await _moduleDataAcces.ListAllAsync()).ToList();            
            var actions = (await _actionDataAcces.ListAllAsync()).ToList();                        
            var tree = GetModulesForParent(null, modules, actions,new List<string>());                        
            return ResponseBuilder.Correct(tree);
        }

        public async Task<ResponseDTO<List<ModuleTreeDTO>>> GetRoleTree(string roleCode)
        {
            // ModuleTreeDTO
            var role = (await _rolDataAccess.FirstOrDefaultAsync(x=>x.Code == roleCode && x.RowStatus));
            var permissions = (await _permissionDataAccess.ListAsync(x => x.AmigoTenantTRoleId == role.AmigoTenantTRoleId)).Select(x=>x.ActionCode).ToList();

            var modules = (await _moduleDataAcces.ListAllAsync()).ToList();
            var actions = (await _actionDataAcces.ListAllAsync()).ToList();

            var tree = GetModulesForParent(null, modules, actions,permissions);
            
            return ResponseBuilder.Correct(tree);
        }

        private List<ModuleTreeDTO> GetModulesForParent(string code, List<ModuleDTO> modules, List<ActionDTO> actions, List<string> permissions)
        {
            var childs = modules.Where(x => x.ParentModuleCode == code).ToList();            
            if(!childs.Any()) return new List<ModuleTreeDTO>();

            var result = new List<ModuleTreeDTO>();

            foreach (var module in childs)
            {
                var m = _mapper.Map<ModuleDTO, ModuleTreeDTO>(module);
                var mchilds = new List<ModuleTreeDTO>();
                var childactions = _mapper.Map<List<ActionDTO>,List<ModuleTreeDTO>>(actions.Where(x => x.ModuleCode == m.Code).ToList());
                childactions = ApplyPermissions(childactions, permissions);
                var childmodules = GetModulesForParent(m.Code, modules, actions,permissions);
                mchilds.AddRange(childactions);
                mchilds.AddRange(childmodules);
                m.Children = mchilds;                
                result.Add(m);
            }
            return result;
        }

        private List<ModuleTreeDTO> ApplyPermissions(List<ModuleTreeDTO> childactions, List<string> permissions)
        {
            foreach (var action in childactions)
            {               
                action.Enabled = permissions.Contains(action.Code);
            }
            return childactions;
        }

        public async Task<ResponseDTO<AmigoTenantTRoleBasicDTO>> GetAmigoTenantTRoleBasicByIdAsync(AmigoTenantTRoleSearchRequest search)
        {
            Expression<Func<AmigoTenantTRoleBasicDTO, bool>> queryFilter = p => true;
            if (search.AmigoTenantTRoleId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTRoleId == search.AmigoTenantTRoleId);
            var result = await _dataAccessBasic.FirstOrDefaultAsync(queryFilter);
            return ResponseBuilder.Correct(result);
        }

        public async Task<bool> Exists(AmigoTenantTRoleSearchRequest search) {
            var queryFilter = GetQueryFilter(search);
            var result = await _dataAccess.AnyAsync(queryFilter);
            return result;
        }

        private async Task<bool> ExistsToSave(AmigoTenantTRoleSearchRequest search)
        {
            var queryFilter = GetQueryFilterForExistsToSave(search);
            var result = await _dataAccess.AnyAsync(queryFilter);
            return result;
        }

        public async Task<ResponseDTO> Register(AmigoTenantTRoleDTO dto)
        {
            var command = _mapper.Map<AmigoTenantTRoleDTO, RegisterAmigoTenantTRoleCommand>(dto);
            var searchRequest = new AmigoTenantTRoleSearchRequest() { AmigoTenantTRoleId = dto.AmigoTenantTRoleId, Code = dto.Code };
            var exists = await ExistsToSave(searchRequest);

            if (exists)
                return ResponseBuilder.InCorrect().WithMessages(new ApplicationMessage() { Message = Constants.Entity.AmigoTenantTRole.ErrorMessage.RoleExist });

            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO> Update(AmigoTenantTRoleDTO dto)
        {
            var command = _mapper.Map<AmigoTenantTRoleDTO, UpdateAmigoTenantTRoleCommand>(dto);            
            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO> UpdateActions(AmigoTenanttRolPermissionRequest actions)
        {
            var role = (await _rolDataAccess.FirstOrDefaultAsync(x => x.Code == actions.CodeRol && x.RowStatus));            

            CommandResult result = null;            

            switch (actions.TableStatus)
            {
                case DTOs.Requests.Common.ObjectStatus.Deleted:
                    var delete = _mapper.Map<AmigoTenanttRolPermissionRequest, DeletePermissionCommand>(actions);
                    result = await _bus.SendAsync(delete);
                    break;
                case DTOs.Requests.Common.ObjectStatus.Added:                    
                    var register = _mapper.Map<AmigoTenanttRolPermissionRequest, RegisterPermissionCommand>(actions);
                    result = await _bus.SendAsync(register);
                    break;
                default:
                    return ResponseBuilder.InCorrect();                    
            }            

            return result.IsCorrect ? ResponseBuilder.Correct():ResponseBuilder.InCorrect();
        }

        private Expression<Func<AmigoTenantTRoleDTO, bool>> GetQueryFilter(AmigoTenantTRoleSearchRequest search)
        {
            Expression<Func<AmigoTenantTRoleDTO, bool>> queryFilter = p => p.RowStatus;

            if (search.AmigoTenantTRoleId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTRoleId == search.AmigoTenantTRoleId);

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));
           
            if (search.IsAdmin != null)
                queryFilter = queryFilter.And(p => p.IsAdmin == search.IsAdmin);
                
            return queryFilter;
        }

        private Expression<Func<AmigoTenantTRoleBasicDTO, bool>> GetQueryFilterBasic(AmigoTenantTRoleSearchRequest search)
        {
            Expression<Func<AmigoTenantTRoleBasicDTO, bool>> queryFilter = p => true;

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));

            return queryFilter;
        }

        public async Task<ResponseDTO> Delete(AmigoTenantTRoleStatusDTO dto)
        {
            var command = _mapper.Map<AmigoTenantTRoleStatusDTO, DeleteAmigoTenantTRoleCommand>(dto);
            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp);
        }

        private Expression<Func<AmigoTenantTRoleDTO, bool>> GetQueryFilterForExistsToSave(AmigoTenantTRoleSearchRequest search)
        {
            Expression<Func<AmigoTenantTRoleDTO, bool>> queryFilter = p => p.RowStatus;

            if (search.AmigoTenantTRoleId > 0)
                queryFilter = queryFilter.And(p => p.AmigoTenantTRoleId != search.AmigoTenantTRoleId);

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code.Contains(search.Code));

            return queryFilter;
        }        
    }
}
