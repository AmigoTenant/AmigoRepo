using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.AmigoTenantTRole;
using Amigo.Tenant.Events.Security;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Roles
{
    public class RegisterAmigoTenantTRoleCommandHandler : IAsyncCommandHandler<RegisterAmigoTenantTRoleCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTRole> _repository;

        public RegisterAmigoTenantTRoleCommandHandler(
            IBus bus, 
            IMapper mapper,
            IRepository<AmigoTenantTRole> repository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }                

        public async Task<CommandResult> Handle(RegisterAmigoTenantTRoleCommand message)
        {
            //Validate using domain models
            var entity = _mapper.Map<RegisterAmigoTenantTRoleCommand, AmigoTenantTRole>(message);

            var alreadyExists = await _repository.ExistsByCodeName(message.Code, message.Name);
            if (alreadyExists) entity.AddError("Role already exists.");

            //if is not valid
            if (entity.HasErrors) return entity.ToResult();

            //Insert
            _repository.Add(entity);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new RoleEditionEvent(entity.Code,RoleEditionEventType.Register));

            //Return result
            return entity.ToResult();
        }
    }
}
