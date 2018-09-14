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

    public class DeleteModuleCommandHandler : IAsyncCommandHandler<DeleteModuleCommand>
    {


        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Module> _moduleRepository;
        private readonly IRepository<Action> _actionRepository;

        public DeleteModuleCommandHandler(
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

        public async Task<CommandResult> Handle(DeleteModuleCommand message)
        {
            //Validate using domain models
            Module moduleAux = new Module();
            moduleAux = _mapper.Map<DeleteModuleCommand, Module>(message);


            #region Module

            //validate code uniqueness
            var existingModule = await _moduleRepository.GetModule(message.Code);

            if (existingModule == null)
            {
                moduleAux.AddError("Module code not found.");
                return moduleAux.ToResult();
            }
            else
            {
                existingModule.RowStatus = false;
                existingModule.Update(message.UserId);
            }

            #endregion


            #region Actions

            existingModule.Actions.ToList().ForEach(p => { p.RowStatus = false; p.Update(message.UserId); });

            #endregion

            _moduleRepository.UpdatePartial(existingModule, new string[] { "RowStatus", "UpdatedBy", "UpdatedDate" });

            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new ModuleDeleted() { ModuleId = existingModule.ModuleId });

            //Return result
            return existingModule.ToResult();
        }

    }

}
