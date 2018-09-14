using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.AmigoTenantTRole;
using Amigo.Tenant.Events.Security;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Roles
{
    public class DeleteAmigoTenantTRoleCommandHandler : IAsyncCommandHandler<DeleteAmigoTenantTRoleCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTRole> _repository;

        public DeleteAmigoTenantTRoleCommandHandler(
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

        public async Task<CommandResult> Handle(DeleteAmigoTenantTRoleCommand message)
        {
            //TODO: Este mapper debe ignorar campos, para que no actualice la INFO
            var entity = _mapper.Map<DeleteAmigoTenantTRoleCommand, AmigoTenantTRole>(message);

            _repository.UpdatePartial(entity,"RowStatus");

            await _unitOfWork.CommitAsync();

            await _bus.PublishAsync(new RoleEditionEvent(entity.Code, RoleEditionEventType.Delete));

            return entity.ToResult();
        }
    }
}
