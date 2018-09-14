
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Move;
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
   public class UpdateAmigoTenanttServiceCommandHandler : IAsyncCommandHandler<UpdateAmigoTenantServiceCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTService> _repository;

        public UpdateAmigoTenanttServiceCommandHandler(
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

        public async Task<CommandResult> Handle(UpdateAmigoTenantServiceCommand message)
        {
            try
            {
                var entity = _mapper.Map<UpdateAmigoTenantServiceCommand, AmigoTenantTService>(message);

                message.RowStatus = true;
                entity.Update(message.UserId);
                //entity.UpdatedBy = entity.CreatedBy;

                _repository.UpdatePartial(entity, new string[]
                {
                    "EquipmentStatusId",
                    "ServiceFinishDate",
                    "ServiceFinishDateTZ",
                    "ServiceFinishDateUTC",
                    "DestinationLocationId",
                    "ChargeType",
                    "ChargeNo",
                    "DriverComments",
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

        private async Task SendLogToAmigoTenantTEventLog(UpdateAmigoTenantServiceCommand message, string errorMsg = "")
        {
            //Publish bussines Event
            var eventData = _mapper.Map<UpdateAmigoTenantServiceCommand, RegisterMoveEvent>(message);
            eventData.LogType = string.IsNullOrEmpty(errorMsg) ? Constants.AmigoTenantTEventLogType.In : Constants.AmigoTenantTEventLogType.Err;
            eventData.Parameters = errorMsg;
            eventData.Username = message.Username;
            await _bus.PublishAsync(eventData);
        }

    }
}
