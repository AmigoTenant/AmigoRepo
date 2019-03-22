
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Expense;
using Amigo.Tenant.Commands.Leasing.Contracts;
using Amigo.Tenant.Commands.PaymentPeriod;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.Leasing.Contracts
{
    public class ExpenseDetailChangeStatusCommandHandler: IAsyncCommandHandler<ExpenseDetailChangeStatusCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ExpenseDetail> _repository;
        private readonly IRepository<PaymentPeriod> _repositoryPayment;


        public ExpenseDetailChangeStatusCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<ExpenseDetail> repository,
         IUnitOfWork unitOfWork,
         IRepository<PaymentPeriod> repositoryPayment)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryPayment = repositoryPayment;
        }


        public async Task<CommandResult> Handle(ExpenseDetailChangeStatusCommand message)
        {
            try
            {
                foreach (var item in message.ExpenseDetailUpdateCommand)
                {
                    //Update Expense DetailStatus
                    var entity = _mapper.Map<ExpenseDetailUpdateCommand, ExpenseDetail>(item);
                    var ent = await _repository.FirstOrDefaultAsync(q => q.ExpenseDetailId == item.ExpenseDetailId);
                    if (ent != null)
                    {
                        ent.ExpenseDetailStatusId = 23;
                        ent.Update(message.UserId);
                    }

                    //entity.ExpenseDetailStatusId = 23; //Expense detail Status Migrated
                    //entity.Update(message.UserId);
                    _repository.Update(ent);


                    var entityPaymentPeriod = _mapper.Map<PaymentPeriodRegisterCommand, PaymentPeriod>(item.PaymentPeriodRegister);
                    entityPaymentPeriod.Creation(message.UserId);
                    _repositoryPayment.Add(entityPaymentPeriod);

                }

                await _unitOfWork.CommitAsync();

                return (new ExpenseDetail() { ExpenseId = message.ExpenseId}).ToRegisterdResult().WithId(message.ExpenseId.Value);

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
