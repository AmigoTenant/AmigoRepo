
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Leasing.Contracts;
using Amigo.Tenant.Commands.PaymentPeriod;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.Leasing.Contracts
{
    //public class RegisterProductCommandHandler : IAsyncCommandHandler<RegisterProductCommand>


    public class ContractChangeStatusCommandHandler : IAsyncCommandHandler<ContractChangeStatusCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Contract> _repository;
        private readonly IRepository<PaymentPeriod> _repositoryPayment;
        private readonly IRepository<ContractChangeStatus> _repositoryContractChangeStatus;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;
        private readonly IRepository<Period> _repositoryPeriod;

        public ContractChangeStatusCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<Contract> repository,
         IUnitOfWork unitOfWork,
         IRepository<PaymentPeriod> repositoryPayment,
         IRepository<ContractChangeStatus> repositoryContractChangeStatus,
         IRepository<EntityStatus> repositoryEntityStatus,
         IRepository<Period> repositoryPeriod)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryPayment = repositoryPayment;
            _repositoryContractChangeStatus = repositoryContractChangeStatus;
            _repositoryEntityStatus = repositoryEntityStatus;
            _repositoryPeriod = repositoryPeriod;
        }


        public async Task<CommandResult> Handle(ContractChangeStatusCommand message)
        {
            try
            {
                //=================================================
                //Contract
                //=================================================
                var entity = await _repository.FirstOrDefaultAsync(q => q.ContractId == message.ContractId);
                //var entity = _mapper.Map<ContractChangeStatusCommand, Contract>(message);
                entity.Update(message.UserId);
                entity.ContractStatusId = message.ContractStatusId.Value;
                _repository.Update(entity);

                //=================================================
                //ContractChangeStatus
                //=================================================
                await CreateContractChangeStatus(entity);

                //=================================================
                //Payment Period
                //=================================================
                var payments = message.PaymentsPeriod;
                foreach (var paymentPeriod in payments)
                {
                    var entityPayment = _mapper.Map<PaymentPeriodRegisterCommand, PaymentPeriod>(paymentPeriod);
                    entityPayment.RowStatus = true;
                    entityPayment.Creation(message.UserId);
                    _repositoryPayment.Add(entityPayment);
                }

                await _unitOfWork.CommitAsync();

                if (entity.ContractId != 0)
                {
                    message.ContractId = entity.ContractId;
                }

                return entity.ToRegisterdResult().WithId(entity.ContractId);
            }
            catch (Exception ex)
            {
                //await SendLogToAmigoTenantTEventLog(message, ex.Message);

                throw;
            }

        }

        private async Task CreateContractChangeStatus(Contract entity)
        {
            var finalPeriod = entity.EndDate.Value.Year.ToString() + entity.EndDate.Value.Month.ToString().PadLeft(2, '0');
            var endPeriod = await _repositoryPeriod.FirstOrDefaultAsync(q => q.Code == finalPeriod);
            
            var contractChangeStatus = new ContractChangeStatus()
            {
                ContractChangeStatusId = -1,
                ContractId = entity.ContractId,
                ContractStatusId = entity.ContractStatusId,
                TenantId = entity.TenantId,
                HouseId = entity.HouseId,
                Rent = entity.RentPrice,
                Deposit = entity.RentDeposit,
                ContractTermType = Constants.ContractTypeTerm.Formal,
                BeginPeriodId = entity.PeriodId,
                EndPeriodId = endPeriod.PeriodId,
                CreatedBy = entity.UpdatedBy,
                CreationDate = entity.UpdatedDate,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate
            };
            _repositoryContractChangeStatus.Add(contractChangeStatus);
        }

        private async Task<int?> GetStatusbyEntityAndCodeAsync(string entityCode, string statusCode)
        {
            var entityStatus = await _repositoryEntityStatus.FirstOrDefaultAsync(q => q.EntityCode == entityCode && q.Code == statusCode);
            if (entityStatus != null)
                return entityStatus.EntityStatusId;

            return null;
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
