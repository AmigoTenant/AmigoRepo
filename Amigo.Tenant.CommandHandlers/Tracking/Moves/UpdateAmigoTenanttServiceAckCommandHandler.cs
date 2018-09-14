
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
   public class UpdateAmigoTenanttServiceAckCommandHandler : IAsyncCommandHandler<UpdateAmigoTenantServiceAckCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTService> _repository;

        public UpdateAmigoTenanttServiceAckCommandHandler(
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

        public async Task<CommandResult> Handle(UpdateAmigoTenantServiceAckCommand message)
        {
            try
            {
                var entityTmp = _mapper.Map<UpdateAmigoTenantServiceAckCommand, AmigoTenantTService>(message);
                var newAmigoTenantTServiceIdList = message.AmigoTenantTServiceIdList.Where(p => p.HasValue && p > 0).ToList();
                foreach (var id in newAmigoTenantTServiceIdList)
                {
                    Expression<Func<AmigoTenantTService, bool>> queryFilter = p => p.AmigoTenantTServiceId == id;
                    var entity = await _repository.FirstOrDefaultAsync(queryFilter);
                    entity.AcknowledgeBy = message.AcknowledgeBy;
                    entity.ServiceAcknowledgeDate = message.ServiceAcknowledgeDate;
                    entity.ServiceAcknowledgeDateTZ = message.ServiceAcknowledgeDateTZ;
                    entity.ServiceAcknowledgeDateUTC = message.ServiceAcknowledgeDateUTC;
                    entity.IsAknowledged = message.IsAknowledged;
                    entity.UpdatedBy = message.UpdatedBy;
                    entity.UpdatedDate = message.UpdatedDate;
                    _repository.UpdatePartial(entity, new string[]
                    {
                        "AcknowledgeBy",
                        "ServiceAcknowledgeDate",
                        "ServiceAcknowledgeDateTZ",
                        "ServiceAcknowledgeDateUTC",
                        "IsAknowledged",
                        "UpdatedBy",
                        "UpdatedDate"
                    });
                }

                await _unitOfWork.CommitAsync();

                //Publish bussines Event
                await SendLogToAmigoTenantTEventLog(message);

                return entityTmp.ToResult();
            }
            catch (Exception ex)
            {
                await SendLogToAmigoTenantTEventLog(message, ex.Message);

                throw;
            }

        }
        private async Task SendLogToAmigoTenantTEventLog(UpdateAmigoTenantServiceAckCommand message, string errorMsg = "")
        {
            //Publish bussines Event
            var eventData = _mapper.Map<UpdateAmigoTenantServiceAckCommand, RegisterMoveEvent>(message);
            eventData.LogType = string.IsNullOrEmpty(errorMsg) ? Constants.AmigoTenantTEventLogType.In : Constants.AmigoTenantTEventLogType.Err;
            eventData.Parameters = errorMsg;
            eventData.RowStatus = true;
            eventData.CreatedBy = message.UpdatedBy;
            eventData.CreationDate = message.UpdatedDate;

            await _bus.PublishAsync(eventData);
        }

    }
}
