using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Permission;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Permissions
{
    public class RegisterPermissionCommandHandler : IAsyncCommandHandler<RegisterPermissionCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Permission> _repository;
        private readonly IRepository<AmigoTenantTRole> _roleRepository;

        public RegisterPermissionCommandHandler(
            IBus bus, 
            IMapper mapper,
            IRepository<Permission> repository,
            IRepository<AmigoTenantTRole> roleRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
        }                

        public async Task<CommandResult> Handle(RegisterPermissionCommand message)
        {
            //Validate using domain models
            var entity = _mapper.Map<RegisterPermissionCommand, Permission>(message);

            var role = await _roleRepository.FirstOrDefaultAsync(x => x.Code == message.CodeRol && x.RowStatus == true).ConfigureAwait(false);
            if (role == null)
            {
                entity.AddError($"Role for code {message.CodeRol} doesn't exist.");
                return entity.ToResult();
            }
            var roleId = role.AmigoTenantTRoleId;

            var alreadyExists = await _repository.Exists(roleId,message.ActionId).ConfigureAwait(false);
            if (alreadyExists) entity.AddError("Permission already exists for this Role.");

            //if is not valid
            if (entity.HasErrors) return entity.ToResult();

            entity.AmigoTenantTRoleId = role.AmigoTenantTRoleId;

            //Insert
            _repository.Add(entity);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new PermissionRegistered() { PermissionId = entity.PermissionId });

            //Return result
            return entity.ToResult();
        }
    }
}
