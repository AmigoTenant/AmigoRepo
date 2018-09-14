
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

namespace Amigo.Tenant.CommandHandlers.Leasing.Contracts
{
    //public class RegisterProductCommandHandler : IAsyncCommandHandler<RegisterProductCommand>


    public class ContractRegisterCommandHandler : IAsyncCommandHandler<ContractRegisterCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Contract> _repository;


        public ContractRegisterCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<Contract> repository,
         IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }


        public async Task<CommandResult> Handle(ContractRegisterCommand message)
        {
            try
            {
                var entity = _mapper.Map<ContractRegisterCommand, Contract>(message);

                //Insert
                entity.RowStatus = true;
                message.RowStatus = true;
                entity.Creation(message.UserId);

                _repository.Add(entity);
                await _unitOfWork.CommitAsync();

                if (entity.ContractId != 0)
                {
                    message.ContractId = entity.ContractId;

                    //Publish bussines Event
                    //await SendLogToAmigoTenantTEventLog(message);
                }
                //Return result
                //return entity;
                return entity.ToRegisterdResult().WithId(entity.ContractId);
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
