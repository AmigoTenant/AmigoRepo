using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Module;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.Security;
using System.Linq;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandHandlers.Security.Modules
{
   
    public class RegisterModuleCommandHandler : IAsyncCommandHandler<RegisterModuleCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Action> _actionRepository;

        public RegisterModuleCommandHandler(
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

       public async Task<CommandResult> Handle(RegisterModuleCommand message)
       {
           //Validate using domain models
           var module = _mapper.Map<RegisterModuleCommand, Module>(message);
           module.Creation(message.UserId);

           //validate code uniqueness
           var existingModule = await _moduleRepository.GetModule(message.Code);
           if (existingModule != null && existingModule.RowStatus.Value)
           {
                module.AddError("A module already exists for this code.");
                return module.ToResult();
           }
                


           if (!string.IsNullOrEmpty(message.ParentModuleCode))
           {
                //Get parentModuleId
                var parentModule = await _moduleRepository.GetModule(message.ParentModuleCode);

                if (parentModule != null)
                {
                    module.ParentModuleId = parentModule.ModuleId;
                }
                else
                {
                    module.AddError("We cound't find a module with code " + message.ParentModuleCode);
                    return module.ToResult();
                }
                
           }

           module.RowStatus = true;

            if (module.Actions != null)
            {
                foreach (var action in module.Actions)
                {
                    var existingAction = await _actionRepository.GetAction(action.Code);

                    if (existingAction != null)
                    {
                        module.AddError("An action already exists for the code " + existingAction.Code);
                        return module.ToResult();
                    }

                    action.RowStatus = true;
                    action.Creation(message.UserId);
                }
            }

            //if is not valid
            if (module.HasErrors) return module.ToResult();


            //Insert
            _moduleRepository.Add(module);
           await _unitOfWork.CommitAsync();

           //Publish bussines Event
           await _bus.PublishAsync(new ModuleRegistered() { ModuleId = module.ModuleId });

           //Return result
           return module.ToResult();
       }

    
    }
}
