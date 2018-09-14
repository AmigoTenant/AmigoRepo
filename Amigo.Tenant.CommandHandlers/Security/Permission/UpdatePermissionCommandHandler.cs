using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Permission;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Permissions
{
    public class UpdatePermissionCommandHandler : IAsyncCommandHandler<UpdatePermissionCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Permission> _repository;

        public UpdatePermissionCommandHandler(
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

        public async Task<CommandResult> Handle(UpdatePermissionCommand message)
        {
            var entity = _mapper.Map<UpdatePermissionCommand, Permission>(message);
            _repository.Update(entity);
            await _unitOfWork.CommitAsync();
            return entity.ToResult();
        }
    }
}
