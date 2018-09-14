using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.AmigoTenantTUsers;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Users
{
    public class UpdateAmigoTenantTUserCommandHandler : IAsyncCommandHandler<UpdateAmigoTenantTUserCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTUser> _repository;

        public UpdateAmigoTenantTUserCommandHandler(
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

        public async Task<CommandResult> Handle(UpdateAmigoTenantTUserCommand message)
        {
            var entity = _mapper.Map<UpdateAmigoTenantTUserCommand, AmigoTenantTUser>(message);
            _repository.UpdatePartial(entity, new string[] {
                                "AmigoTenantTRoleId",
                                "PayBy",
                                "UserType",
                                "DedicatedLocationId",
                                "BypassDeviceValidation",
                                "UnitNumber",
                                "TractorNumber",
                                "RowStatus",
                                "UpdatedDate",
                                "UpdatedBy"});
            await _unitOfWork.CommitAsync();
            //Publish bussines Event
            await _bus.PublishAsync(new AmigoTenantTUserRegistered() { AmigoTenantTUserId = entity.AmigoTenantTUserId });
            return entity.ToResult();
        }
    }
}
