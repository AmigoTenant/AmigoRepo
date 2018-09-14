
using System;
using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Leasing.Contracts;
using Amigo.Tenant.Commands.MasterData.RentalApplication;

namespace Amigo.Tenant.CommandHandlers.MasterData.RentalApplications
{
    public class RentalApplicationRegisterCommandHandler : IAsyncCommandHandler<RentalApplicationRegisterCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<RentalApplication> _repository;


        public RentalApplicationRegisterCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<RentalApplication> repository,
         IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }


        public async Task<CommandResult> Handle(RentalApplicationRegisterCommand message)
        {
            try
            {
                var entity = _mapper.Map<RentalApplicationRegisterCommand, RentalApplication>(message);

                //Insert
                entity.RowStatus = true;
                message.RowStatus = true;
                entity.Creation(message.UserId);

                _repository.Add(entity);
                await _unitOfWork.CommitAsync();

                if (entity.RentalApplicationId != 0)
                {
                    message.RentalApplicationId = entity.RentalApplicationId;

                    //Publish bussines Event
                    //await SendLogToAmigoTenantTEventLog(message);
                }
                //Return result
                //return entity;
                return entity.ToRegisterdResult().WithId(entity.RentalApplicationId.Value);
            }
            catch (Exception ex)
            {
                //await SendLogToAmigoTenantTEventLog(message, ex.Message);

                throw;
            }

        }

        //private async Task SendLogToAmigoTenantTEventLog(RegisterAmigoTenanttServiceCommand message, string errorMsg = "")
        //{
        //    //Publish bussines Event
        //    var eventData = _mapper.Map<RegisterAmigoTenanttServiceCommand, RegisterMoveEvent>(message);
        //    eventData.LogType = string.IsNullOrEmpty(errorMsg)? Constants.AmigoTenantTEventLogType.In:Constants.AmigoTenantTEventLogType.Err;
        //    eventData.Parameters = errorMsg;
        //    eventData.Username = message.Username;
        //    await _bus.PublishAsync(eventData);
        //}
    }
}
