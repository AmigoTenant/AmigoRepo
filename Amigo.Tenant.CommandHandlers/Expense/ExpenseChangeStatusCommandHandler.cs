﻿
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
                var entity = _mapper.Map<ExpenseDetailChangeStatusCommand, ExpenseDetail>(message);

                //Insert
                entity.Update(message.UserId);

                //=================================================
                //Contract
                //=================================================

                _repository.UpdatePartial(entity, new string[] {    "ExpenseDetailId",
                                                                    "ExpenseDetailStatusId",
                                                                    "UpdatedBy",
                                                                    "UpdatedDate"});

                //=================================================
                //Payment Period
                //=================================================
                var payment = message.PaymentsPeriod;
                var entityPayment = _mapper.Map<PaymentPeriodRegisterCommand, PaymentPeriod>(payment);
                entityPayment.RowStatus = true;
                entityPayment.Creation(message.UserId);
                _repositoryPayment.Add(entityPayment);
                await _unitOfWork.CommitAsync();

                //foreach (var paymentPeriod in payments)
                //{
                //    var entityPayment = _mapper.Map<PaymentPeriodRegisterCommand, PaymentPeriod>(paymentPeriod);
                //    entityPayment.RowStatus = true;
                //    entityPayment.Creation(message.UserId);
                //    _repositoryPayment.Add(entityPayment);
                //}
                ////TODO: List no esta permitido agregar al model
                ////_repository.Add(entity);
                //await _unitOfWork.CommitAsync();

                //if (entity.ExpenseId != 0)
                //{
                //    message.ExpenseDetailId = entity.ContractId;
                //}

                return entity.ToRegisterdResult().WithId(entity.ExpenseId.Value);
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
