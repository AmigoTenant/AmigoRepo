
using System;
using System.Threading.Tasks;
using ExpressMapper;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Move;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Approve;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Tracking.Approve
{
   public class UpdateAmigoTenantTServiceApproveCommandHandler : IAsyncCommandHandler<UpdateAmigoTenantTServiceApproveCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTService> _repository;

        public UpdateAmigoTenantTServiceApproveCommandHandler(
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

        public string[] FieldsToUpdate { get; set; }

        public async Task<CommandResult> Handle(UpdateAmigoTenantTServiceApproveCommand message)
        {
            try
            {
                var entity = _mapper.Map<UpdateAmigoTenantTServiceApproveCommand, AmigoTenantTService>(message);
                entity.RowStatus = true;
                message.RowStatus = true;
                _repository.UpdatePartial(entity, new string[] {
                                "AmigoTenantTServiceId",
                                "ChargeType",
                                "ChargeNo",
                                "EquipmentNumber",
                                "ChassisNumber",
                                "EquipmentStatusId",
                                "ServiceId",
                                "OriginLocationId",
                                "DestinationLocationId",
                                "ServiceStartDate",
                                "ServiceStartDateUTC",
                                "ServiceFinishDate",
                                "ServiceFinishDateUTC",
                                "ProductId",
                                "HasH34",
                                "RowStatus",
                                "UpdatedDate",
                                "UpdatedBy"
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

        private async Task SendLogToAmigoTenantTEventLog(UpdateAmigoTenantTServiceApproveCommand message, string errorMsg = "")
        {
            //Publish bussines Event
            var eventData = _mapper.Map<UpdateAmigoTenantTServiceApproveCommand, RegisterMoveEvent>(message);
            eventData.LogType = string.IsNullOrEmpty(errorMsg) ? Amigo.Tenant.Common.Constants.AmigoTenantTEventLogType.In : Amigo.Tenant.Common.Constants.AmigoTenantTEventLogType.Err;
            eventData.Parameters = errorMsg;
            eventData.ReportedActivityDate = DateTime.Now;
            eventData.CreatedBy = message.UpdatedBy;
            await _bus.PublishAsync(eventData);

        }

    }
}
