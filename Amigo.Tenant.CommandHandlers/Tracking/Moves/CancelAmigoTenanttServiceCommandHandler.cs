
using System;
using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Common;
using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Tracking.Moves
{
    public class CancelAmigoTenanttServiceCommandHandler : IAsyncCommandHandler<CancelAmigoTenantServiceCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTService> _repository;

        public CancelAmigoTenanttServiceCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<AmigoTenantTService> repository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(CancelAmigoTenantServiceCommand message)
        {
            try
            {
                var entity = _mapper.Map<CancelAmigoTenantServiceCommand, AmigoTenantTService>(message);
                entity.Update(message.UserId);
                _repository.UpdatePartial(entity, new string[]
                {
                    "RowStatus",
                    "UpdatedBy",
                    "UpdatedDate"
                });
                await _unitOfWork.CommitAsync();

                //Publish bussines Event
                await SendLogToAmigoTenantTEventLog(message);
               
                return entity.ToResult();
            }
            catch (Exception ex)
            {
                await SendLogToAmigoTenantTEventLog(message, ex.Message);

                throw;
            }

        }

        private async Task SendLogToAmigoTenantTEventLog(CancelAmigoTenantServiceCommand message, string errorMsg = "")
        {
            //Publish bussines Event
            var eventData = _mapper.Map<CancelAmigoTenantServiceCommand, RegisterMoveEvent>(message);
            eventData.LogType = string.IsNullOrEmpty(errorMsg) ? Constants.AmigoTenantTEventLogType.In : Constants.AmigoTenantTEventLogType.Err;
            eventData.Parameters = errorMsg;
            eventData.Username = message.Username;
            await _bus.PublishAsync(eventData);
        }

    }
}
