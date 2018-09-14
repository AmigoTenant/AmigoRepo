using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Security;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Module;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using System.Linq;

namespace Amigo.Tenant.CommandHandlers.Security.Modules
{
    public class UpdateModuleCommandHandler : IAsyncCommandHandler<UpdateModuleCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Action> _actionRepository;

        public UpdateModuleCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<Module> moduleRepository,
            IRepository<Action> actionRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _moduleRepository = moduleRepository;
            _actionRepository = actionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(UpdateModuleCommand message)
        {
            //Validate using domain models
            Module moduleAux = new Module();
            moduleAux = _mapper.Map<UpdateModuleCommand, Module>(message);


            #region Module

            //validate code uniqueness
            var existingModule = await _moduleRepository.GetModule(message.Code);

            if (existingModule == null)
            {
                moduleAux.AddError("Module code not found.");
            }
            else
            {
                existingModule.Name = moduleAux.Name;
                existingModule.URL = moduleAux.URL;
                existingModule.SortOrder = moduleAux.SortOrder;
                existingModule.Update(message.UserId);

                if (!string.IsNullOrEmpty(message.ParentModuleCode))
                {

                    //Get ParentModuleId
                    var parentModule = await _moduleRepository.GetModule(message.ParentModuleCode);

                    if (parentModule != null)
                    {
                        existingModule.ParentModuleId = parentModule.ModuleId;
                    }
                    else
                    {
                        moduleAux.AddError("We cound't find a module with code " + message.ParentModuleCode);
                        return moduleAux.ToResult();
                    }
                }
            }

            //if is not valid
            if (moduleAux.HasErrors) return moduleAux.ToResult();


            #endregion


            #region Actions
            if (message.Actions != null && message.Actions.Count > 0)
            {
                var actionsInUI = message.Actions.ToList();
                foreach (var item in actionsInUI.Where(q => q.TableStatus != Application.DTOs.Requests.Common.ObjectStatus.Unchanged))
                {
                    if (item.TableStatus == Application.DTOs.Requests.Common.ObjectStatus.Added)
                    {

                        //-------   Code and Name are required  ----------

                        if (string.IsNullOrEmpty(item.Code))
                        {
                            existingModule.AddError("Action Code is required");
                            return existingModule.ToResult();
                        }
                        if (string.IsNullOrEmpty(item.Name))
                        {
                            existingModule.AddError("Action name is required");
                            return existingModule.ToResult();
                        }

                        //------------------------------------------------

                        var existingAction = await _actionRepository.GetAction(item.Code);
                        if (existingAction != null)
                        {
                            existingModule.AddError("An action already exists for the code " + existingAction.Code);
                            return existingModule.ToResult();
                        }

                        var entityAction = _mapper.Map<ActionCommand, Action>(item);
                        entityAction.RowStatus = true;
                        entityAction.Creation(message.UserId);
                        existingModule.Actions.Add(entityAction);
                    }
                    else if (item.TableStatus == Application.DTOs.Requests.Common.ObjectStatus.Modified)
                    {
                        var action = existingModule.Actions.FirstOrDefault(a => a.Code == item.Code);
                        action.Name = item.Name;
                        action.Description = item.Description;
                        action.Type = item.Type;
                        action.Update(message.UserId);

                    }
                    else if (item.TableStatus == Application.DTOs.Requests.Common.ObjectStatus.Deleted)
                    {
                        var action = existingModule.Actions.FirstOrDefault(a => a.Code == item.Code);
                        action.RowStatus = false;
                        action.Update(message.UserId);
                    }
                }
            }

            #endregion

            _moduleRepository.Update(existingModule);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new ModuleUpdated() { ModuleId = existingModule.ModuleId });

            //Return result
            return existingModule.ToResult();
        }

    }

}
