
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Leasing.Contracts;
using Amigo.Tenant.Commands.PaymentPeriod;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.Leasing.Contracts
{
    public class ContractChangeTermCommandHandler : IAsyncCommandHandler<ContractChangeTermCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Contract> _repository;
        private readonly IRepository<PaymentPeriod> _repositoryPayment;
        private readonly IRepository<Invoice> _repositoryInvoice;

        public ContractChangeTermCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<Contract> repository,
         IUnitOfWork unitOfWork,
         IRepository<PaymentPeriod> repositoryPayment,
         IRepository<Invoice> repositoryInvoice)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryPayment = repositoryPayment;
            _repositoryInvoice = repositoryInvoice;
        }


        public async Task<CommandResult> Handle(ContractChangeTermCommand message)
        {
            try
            {
                List<OrderExpression<PaymentPeriod>> orderExpressionList = new List<OrderExpression<PaymentPeriod>>();
                orderExpressionList.Add(new OrderExpression<PaymentPeriod>(OrderType.Desc, p => p.PaymentPeriodId.Value));

                //List<OrderExpression<Invoice>> orderExpressionList = new List<OrderExpression<Invoice>>();
                //orderExpressionList.Add(new OrderExpression<Invoice>(OrderType.Desc, p => p.InvoiceNo));
                Expression<Func<PaymentPeriod, bool>> queryFilter = q => q.RowStatus;

                var firstPaymentPeriod = (await _repositoryPayment.FirstOrDefaultAsync(queryFilter, orderExpressionList.ToArray()));


                List<OrderExpression<Invoice>> orderExpressionList1 = new List<OrderExpression<Invoice>>();
                orderExpressionList1.Add(new OrderExpression<Invoice>(OrderType.Desc, p => p.InvoiceNo));
                Expression<Func<Invoice, bool>> queryFilter1 = p => p.RowStatus.Value;

                var maxInvoice = await _repositoryInvoice.FirstOrDefaultAsync(queryFilter1, orderExpressionList1.ToArray());
                //var entity = _mapper.Map<ContractChangeTermCommand, Contract>(message);

                //Insert
                //entity.Update(message.UserId);

                //=================================================
                //Contract
                //=================================================

                //_repository.UpdatePartial(entity, new string[] {    "ContractId",
                //                                                    "ContractStatusId",
                //                                                    "UpdatedBy",
                //                                                    "UpdatedDate"});

                //=================================================
                //Payment Period
                //=================================================
                //var payments = message.PaymentsPeriod;
                //foreach (var paymentPeriod in payments)
                //{
                //    var entityPayment = _mapper.Map<PaymentPeriodRegisterCommand, PaymentPeriod>(paymentPeriod);
                //    entityPayment.RowStatus = true;
                //    entityPayment.Creation(message.UserId);
                //    _repositoryPayment.Add(entityPayment);
                //}

                //await _unitOfWork.CommitAsync();

                //if (entity.ContractId != 0)
                //{
                //    message.ContractId = entity.ContractId;
                //}

                return null; // entity.ToRegisterdResult().WithId(entity.ContractId);
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
