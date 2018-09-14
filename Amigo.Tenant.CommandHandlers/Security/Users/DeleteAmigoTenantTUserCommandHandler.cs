using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.AmigoTenantTUsers;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Users
{
    public class DeleteAmigoTenantTUserCommandHandler : IAsyncCommandHandler<DeleteAmigoTenantTUserCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTUser> _repository;

        public DeleteAmigoTenantTUserCommandHandler(
            IBus bus, 
            IMapper mapper,
            IRepository<AmigoTenantTUser> repository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }                

        public async Task<CommandResult> Handle(DeleteAmigoTenantTUserCommand message)
        {
            //TODO: Este mapper debe ignorar campos, para que no actualice la INFO
            var entity = _mapper.Map<DeleteAmigoTenantTUserCommand, AmigoTenantTUser>(message);
            _repository.UpdatePartial(entity,new string[] { "RowStatus" });
            await _unitOfWork.CommitAsync();
            return entity.ToResult();
        }
    }
}
