
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.Move;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Common;
using Amigo.Tenant.Events.Tracking;

namespace Amigo.Tenant.CommandHandlers.Tracking.Moves
{
  public  class RegisterAmigoTenanttServiceCommandHandler : IAsyncCommandHandler<RegisterAmigoTenanttServiceCommand,RegisteredCommandResult>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTService> _repository;


        public RegisterAmigoTenanttServiceCommandHandler(
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


        public async Task<RegisteredCommandResult> Handle(RegisterAmigoTenanttServiceCommand message)
        {
            try
            {
                var entity = _mapper.Map<RegisterAmigoTenanttServiceCommand, AmigoTenantTService>(message);

                //Insert
                entity.RowStatus = true;
                message.RowStatus = true;
                entity.Creation(message.UserId);
                entity.AmigoTenantTUserId = entity.CreatedBy;

                _repository.Add(entity);
                await _unitOfWork.CommitAsync();

                if (entity.AmigoTenantTServiceId != 0)
                {
                    message.AmigoTenantTServiceId = entity.AmigoTenantTServiceId;

                    //Publish bussines Event
                    await SendLogToAmigoTenantTEventLog(message);
                }
                //Return result
                return entity.ToRegisterdResult().WithId(entity.AmigoTenantTServiceId);
            }
            catch (Exception ex)
            {
                await SendLogToAmigoTenantTEventLog(message, ex.Message);

                throw;
            }

        }

        private async Task SendLogToAmigoTenantTEventLog(RegisterAmigoTenanttServiceCommand message, string errorMsg = "")
        {
            //Publish bussines Event
            var eventData = _mapper.Map<RegisterAmigoTenanttServiceCommand, RegisterMoveEvent>(message);
            eventData.LogType = string.IsNullOrEmpty(errorMsg)? Constants.AmigoTenantTEventLogType.In:Constants.AmigoTenantTEventLogType.Err;
            eventData.Parameters = errorMsg;
            eventData.Username = message.Username;
            await _bus.PublishAsync(eventData);
        }
    }
}
