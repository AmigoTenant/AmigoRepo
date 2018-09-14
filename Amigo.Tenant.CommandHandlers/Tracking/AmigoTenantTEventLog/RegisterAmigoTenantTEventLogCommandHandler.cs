using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
//using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.AmigoTenanttEventLog;
using Amigo.Tenant.Common;
using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Tracking.AmigoTenantTEventLog
{
    public class RegisterAmigoTenantTEventLogCommandHandler : IAsyncCommandHandler<RegisterAmigoTenanttEventLogCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Amigo.Tenant.CommandModel.Models.AmigoTenantTEventLog> _repository;

        public RegisterAmigoTenantTEventLogCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<Amigo.Tenant.CommandModel.Models.AmigoTenantTEventLog> repository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(RegisterAmigoTenanttEventLogCommand message)
        {
            //Validate using domain models
            var entity = _mapper.Map<RegisterAmigoTenanttEventLogCommand, Amigo.Tenant.CommandModel.Models.AmigoTenantTEventLog>(message);

            if (string.IsNullOrEmpty(entity.LogType))
            {
                entity.LogType = Constants.AmigoTenantTEventLogType.In;
            }

            //Insert
            entity.RowStatus = true;
            _repository.Add(entity);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new AmigoTenantTUserRegistered() { AmigoTenantTUserId = entity.AmigoTenantTEventLogId });
                
            //Return result
            return entity.ToResult();
        }
    }
}
