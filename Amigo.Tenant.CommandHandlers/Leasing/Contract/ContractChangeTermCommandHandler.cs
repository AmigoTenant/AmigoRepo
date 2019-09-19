
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
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepository<Period> _repositoryPeriod;
        private readonly IRepository<Concept> _repositoryConcept;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;
        private readonly IRepository<GeneralTable> _repositoryGeneralTable;

        public ContractChangeTermCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<Contract> repository,
         IUnitOfWork unitOfWork,
         IRepository<PaymentPeriod> repositoryPayment,
         IRepository<Period> repositoryPeriod,
         IRepository<Concept> repositoryConcept,
         IRepository<EntityStatus> repositoryEntityStatus,
         IRepository<GeneralTable> repositoryGeneralTable)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryPayment = repositoryPayment;
            _repositoryPeriod = repositoryPeriod;
            _repositoryConcept = repositoryConcept;
            _repositoryEntityStatus = repositoryEntityStatus;
            _repositoryGeneralTable = repositoryGeneralTable;
        }


        public async Task<CommandResult> Handle(ContractChangeTermCommand message)
        {
            try
            {
                //Ultimo Periodo del contrato Actual
                string[] includes = new string[] { "Period" };
                List<OrderExpression<PaymentPeriod>> orderExpressionList = new List<OrderExpression<PaymentPeriod>>();
                orderExpressionList.Add(new OrderExpression<PaymentPeriod>(OrderType.Desc, p=> p.Period.Code.ToString()));
                Expression<Func<PaymentPeriod, bool>> queryFilter = q => q.RowStatus && q.ContractId == message.ContractId;
                var firstPaymentPeriod = (await _repositoryPayment.FirstOrDefaultAsync(queryFilter, orderExpressionList.ToArray(), includes: includes));
                //Periodo hasta donde hay que traer
                var newPeriod = await _repositoryPeriod.FirstOrDefaultAsync(q => q.PeriodId == message.PeriodId);
                //Lista de Periodos hasta donde hay que traer
                var periodList = (await _repositoryPeriod.ListAsync(q => q.Sequence > firstPaymentPeriod.Period.Sequence && 
                                                                        q.Sequence <= newPeriod.Sequence &&
                                                                        q.RowStatus)).OrderBy(q=> q.Sequence);

                await CreatePaymentsByPeriod(newPeriod, firstPaymentPeriod.Period, periodList.ToList(), message);





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

        private async Task CreatePaymentsByPeriod(Period newPeriod, Period currentPeriod, List<Period> periodList, ContractChangeTermCommand request)
        {
            var isLastPeriod = false;
            //var period = (await _periodApplicationService.GetPeriodByIdAsync(request.PeriodId)).Data;
            var contractEndDate = newPeriod.EndDate;
            var currentPeriodDueDate = currentPeriod.DueDate.Value.AddMonths(1);
            var id = -1;
            //var contractDetails = new List<ContractDetailRegisterRequest>();
            var paymentsPeriod = new List<PaymentPeriodRegisterCommand>();
            var rentConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Rent);
            var depositConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Deposit);
            var entityStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);
            var paymentTypeRentId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Rent);
            var paymentTypeDepositId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Deposit);

            //while (contractEndDate > currentPeriodDueDate)
            foreach (var period in periodList)
            {
                SetPaymentsPeriod(paymentsPeriod, request, period, id, isLastPeriod, rentConceptId, entityStatusId, paymentTypeRentId, depositConceptId, paymentTypeDepositId);

                //TODO: nO HA SEQUENCE 13 SE ESTA CAYENDO
                //period = (await _periodApplicationService.GetPeriodBySequenceAsync(period.Sequence + 1)).Data;
                currentPeriodDueDate = period.DueDate.Value.AddMonths(1);
                id--;

                //if (contractEndDate.Value.Year == currentPeriodDueDate.Year && contractEndDate.Value.Month == currentPeriodDueDate.Month)
                //{
                //    isLastPeriod = true;
                //    SetPaymentsPeriod(paymentsPeriod, request, period, id, isLastPeriod, rentConceptId, entityStatusId, paymentTypeRentId, depositConceptId, paymentTypeDepositId);
                //    request.PaymentsPeriod = paymentsPeriod;
                //    return;
                //}
                //else if (currentPeriodDueDate.CompareTo(contractEndDate) > 0)
                //{
                //    request.PaymentsPeriod = paymentsPeriod;
                //    return;
                //}
            }
        }

        private void SetPaymentsPeriod(List<PaymentPeriodRegisterCommand> paymentsPeriod, ContractChangeTermCommand request, Period period, int id, bool isLastPeriod, int? rentId, int? entityStatusId, int? paymentTypeId, int? depositConceptId, int? paymentTypeDepositId)
        {
            ///////////////////
            //SETTING FOR  DEPOSIT
            ///////////////////

            if (Math.Abs(id) == 1)
            {
                var paymentPeriodDeposit = new PaymentPeriodRegisterCommand();
                paymentPeriodDeposit.PaymentPeriodId = id;
                paymentPeriodDeposit.ConceptId = depositConceptId; //"CODE FOR CONCEPT"; //TODO:
                paymentPeriodDeposit.ContractId = request.ContractId;
                paymentPeriodDeposit.TenantId = request.TenantId;
                paymentPeriodDeposit.PeriodId = period.PeriodId;
                paymentPeriodDeposit.PaymentPeriodStatusId = entityStatusId; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
                paymentPeriodDeposit.RowStatus = true;
                paymentPeriodDeposit.CreatedBy = request.UserId;
                paymentPeriodDeposit.CreationDate = DateTime.Now;
                paymentPeriodDeposit.UpdatedBy = request.UserId;
                paymentPeriodDeposit.UpdatedDate = DateTime.Now;
                paymentPeriodDeposit.PaymentTypeId = paymentTypeDepositId;
                paymentPeriodDeposit.PaymentAmount = request.newDeposit;
                paymentPeriodDeposit.DueDate = period.DueDate;
                paymentsPeriod.Add(paymentPeriodDeposit);
            }

            ///////////////////
            //SETTING FOR  RENT
            ///////////////////

            var paymentPeriodRent = new PaymentPeriodRegisterCommand();
            paymentPeriodRent.PaymentPeriodId = id;
            paymentPeriodRent.ConceptId = rentId; //"CODE FOR CONCEPT"; //TODO:
            paymentPeriodRent.ContractId = request.ContractId;
            paymentPeriodRent.TenantId = request.TenantId;
            paymentPeriodRent.PeriodId = period.PeriodId;
            paymentPeriodRent.PaymentPeriodStatusId = entityStatusId; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
            paymentPeriodRent.RowStatus = true;
            paymentPeriodRent.CreatedBy = request.UserId;
            paymentPeriodRent.CreationDate = DateTime.Now;
            paymentPeriodRent.UpdatedBy = request.UserId;
            paymentPeriodRent.UpdatedDate = DateTime.Now;
            paymentPeriodRent.PaymentTypeId = paymentTypeId;
            paymentPeriodRent.PaymentAmount = request.newRent;
            paymentPeriodRent.DueDate = period.DueDate;
            paymentsPeriod.Add(paymentPeriodRent);
        }

        //private decimal CalculateFirstRent(DateTime? beginDate, decimal rentPrice)
        //{
        //    var daysNumber = 31 - beginDate.Value.Day;
        //    var rent = rentPrice / 30 * daysNumber;
        //    return rent;
        //}

        //private decimal CalculateLastRent(DateTime? endDate, decimal rentPrice)
        //{
        //    var daysNumber = endDate.Value.Day;
        //    if (daysNumber >= 28)
        //        daysNumber = 30;
        //    var rent = rentPrice / 30 * daysNumber;
        //    return rent;
        //}

        private async Task<int?> GetStatusbyEntityAndCodeAsync(string entityCode, string statusCode)
        {
            var entityStatus = await _repositoryEntityStatus.FirstOrDefaultAsync(q=> q.EntityCode == entityCode && q.Code ==  statusCode );
            if (entityStatus != null)
                return entityStatus.EntityStatusId;

            return null;
        }

        private async Task<Concept> GetConceptByCode(string conceptCode)
        {
            var entity = await _repositoryConcept.FirstOrDefaultAsync(q=> q.Code == conceptCode && q.RowStatus.Value);
            return entity;
        }

        private async Task<int?> GetConceptIdByCode(string conceptCode)
        {
            var entity = await GetConceptByCode(conceptCode);
            if (entity != null)
                return entity.ConceptId;
            return null;
        }

        private async Task<int?> GetGeneralTableIdByTableNameAndCode(string tableName, string tableCode)
        {
            var entity = await _repositoryGeneralTable.FirstOrDefaultAsync(q=> q.TableName == tableName && q.Code== tableCode && q.RowStatus);
            if (entity != null)
                return entity.GeneralTableId;
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
