using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Permission;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Permissions
{
    public class DeletePermissionCommandHandler : IAsyncCommandHandler<DeletePermissionCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Permission> _repository;

        public DeletePermissionCommandHandler(
            IBus bus, 
            IMapper mapper,
            IRepository<Permission> repository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }                

        public async Task<CommandResult> Handle(DeletePermissionCommand message)
        {
            //TODO: 
            var entity = _mapper.Map<DeletePermissionCommand, Permission>(message);

            var exists = await _repository.AnyAsync(x => x.Action.Code == message.CodeAction && x.AmigoTenantTRole.Code == message.CodeRol).ConfigureAwait(false);
            if (!exists) entity.AddError("The permission doesn't exist.");

            if (entity.HasErrors) return entity.ToResult();

            var permissions = await _repository.ListAsync(x => x.Action.Code == message.CodeAction && x.AmigoTenantTRole.Code == message.CodeRol).ConfigureAwait(false);

            foreach(var p in permissions)
            {
                _repository.Delete(p);
            }
            await _unitOfWork.CommitAsync();

            foreach (var p in permissions)
            {
                await _bus.PublishAsync(new PermissionDeleted(entity.PermissionId));
            }

            return entity.ToRegisterdResult();
        }
    }
}
