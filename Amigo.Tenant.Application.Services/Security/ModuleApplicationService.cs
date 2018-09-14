using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Security;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Security;
using Amigo.Tenant.Application.Services.Common.Extensions;
using Amigo.Tenant.Application.Services.Interfaces.Security;
using Amigo.Tenant.Commands.Security.Module;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;

namespace Amigo.Tenant.Application.Services.Security
{
    public class ModuleApplicationService:IModuleApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ModuleDTO> _moduleDataAcces;
        private readonly IQueryDataAccess<ActionDTO> _actionDataAcces;

        public ModuleApplicationService(IBus bus,
            IQueryDataAccess<ModuleDTO> moduleDataAcces,
            IQueryDataAccess<ActionDTO> actionDataAcces,
            IMapper mapper)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _moduleDataAcces = moduleDataAcces;
            _actionDataAcces = actionDataAcces;
            _mapper = mapper;
        }

     

        public async Task<ResponseDTO<PagedList<ModuleDTO>>> SearchModulesAsync(ModuleSearchRequest search)
        {
            
            Expression<Func<ModuleDTO, bool>> queryFilter = c => true;

            if (!string.IsNullOrEmpty(search.Code))
                queryFilter = queryFilter.And(p => p.Code == search.Code);

            if (!string.IsNullOrEmpty(search.Name))
                queryFilter = queryFilter.And(p => p.Name.Contains(search.Name));

            if (search.OnlyParents != null && search.OnlyParents == true)
                queryFilter = queryFilter.And(p => p.Url == null);

            if (!string.IsNullOrEmpty(search.ParentName))
                queryFilter = queryFilter.And(p => p.ParentModuleName.Contains(search.ParentName));

            Expression<Func<ModuleDTO, object>> expressionModuleCode = p => p.Code;
            List<OrderExpression<ModuleDTO>> orderExpressions = new List<OrderExpression<ModuleDTO>>();
            orderExpressions.Add(new OrderExpression<ModuleDTO>(OrderType.Asc, expressionModuleCode));

            var module = await _moduleDataAcces.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressions.ToArray());

            var pagedResult = new PagedList<ModuleDTO>()
            {
                Items = module.Items,
                PageSize = module.PageSize,
                Page = module.Page,
                Total = module.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }





        public async Task<ResponseDTO<ModuleActionsDTO>> GetModuleAsync(GetModuleRequest getRequest)
        {

            var moduleAux = await SearchModulesAsync(new ModuleSearchRequest { Code = getRequest.Code, Page = 1, PageSize = 1 });

            var module = moduleAux.Data.Items.FirstOrDefault();

            if (module != null)
            {

                var actions = await _actionDataAcces.ListAsync(w=> w.ModuleCode == module.Code);

                var moduleActions = new ModuleActionsDTO
                {
                    Code = module.Code,
                    Name = module.Name,
                    Url = module.Url,
                    SortOrder = module.SortOrder,
                    ParentModuleCode = module.ParentModuleCode,
                    Actions = actions.ToList()
                };

                if (moduleActions.Actions != null)
                    moduleActions.Actions.ForEach(p => p.TableStatus = DTOs.Requests.Common.ObjectStatus.Unchanged);

                return ResponseBuilder.Correct(moduleActions);
            }
            else
            {
                ModuleActionsDTO obj = null;
                return ResponseBuilder.Correct(obj);
            }

        }


        public async Task<ResponseDTO> RegisterModuleAsync(RegisterModuleRequest newModule)
        {

            //Map to Command
            var command = _mapper.Map<RegisterModuleRequest, RegisterModuleCommand>(newModule);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return resp.ToResponse();
        }


        public async Task<ResponseDTO> UpdateModuleAsync(UpdateModuleRequest module)
        {
            //Map to Command
            var command = _mapper.Map<UpdateModuleRequest, UpdateModuleCommand>(module);

            //Execute Command
            var resp = await _bus.SendAsync(command);
            return resp.ToResponse();
        }

        public async Task<ResponseDTO> DeleteModuleAsync(DeleteModuleRequest module)
        {
            //Map to Command
            var command = _mapper.Map<DeleteModuleRequest, DeleteModuleCommand>(module);

            //Execute Command
            var resp = await _bus.SendAsync(command);
            return resp.ToResponse();
        }
    }
}
