
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


        public async Task<CommandResult> Handle(ContractChangeTermCommand request)
        {
            try
            {
                var rentConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Rent);
                //Ultimo Periodo del contrato Actual para el concepto Renta
                string[] includes = new string[] { "Period" };
                List<OrderExpression<PaymentPeriod>> orderExpressionList = new List<OrderExpression<PaymentPeriod>>();
                orderExpressionList.Add(new OrderExpression<PaymentPeriod>(OrderType.Desc, p=> p.Period.Code.ToString()));
                Expression<Func<PaymentPeriod, bool>> queryFilter = q => q.RowStatus && q.ContractId == request.ContractId && q.ConceptId == rentConceptId;
                var lastPaymentPeriod = (await _repositoryPayment.FirstOrDefaultAsync(queryFilter, orderExpressionList.ToArray(), includes: includes));
                //Periodo hasta donde hay que traer
                var finalPeriod = await _repositoryPeriod.FirstOrDefaultAsync(q => q.PeriodId == request.FinalPeriodId);
                //Lista de Periodos hasta donde hay que traer
                var newPeriodList = (await _repositoryPeriod.ListAsync(q => q.Sequence > lastPaymentPeriod.Period.Sequence && 
                                                                        q.Sequence <= finalPeriod.Sequence &&
                                                                        q.RowStatus)).OrderBy(q=> q.Sequence);

                if (request.ContractTermType == Constants.ContractTypeTerm.Extension)
                {
                    var paymentPeriodList = await CreatePaymentsByPeriod(lastPaymentPeriod.Period, newPeriodList.ToList(), request, lastPaymentPeriod, rentConceptId);

                    //=================================================
                    //PAYMENT PERIOD
                    //=================================================
                    foreach (var paymentPeriod in paymentPeriodList)
                    {
                        paymentPeriod.RowStatus = true;
                        paymentPeriod.Creation(request.UserId);
                        _repositoryPayment.Add(paymentPeriod);
                    }
                }

                if (request.ContractTermType == Constants.ContractTypeTerm.Modification)
                {
                    //var paymentPeriodList = await CreatePaymentsByPeriod(lastPaymentPeriod.Period, newPeriodList.ToList(), message, lastPaymentPeriod, rentConceptId);

                    ////=================================================
                    ////PAYMENT PERIOD
                    ////=================================================
                    //foreach (var paymentPeriod in paymentPeriodList)
                    //{
                    //    paymentPeriod.RowStatus = true;
                    //    paymentPeriod.Creation(message.UserId);
                    //    _repositoryPayment.Add(paymentPeriod);
                    //}
                }


                //=================================================
                //CONTRACT
                //=================================================
                int? contractRenewedStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.Contract, Constants.EntityStatus.Contract.Renewed);
                var entity = await _repository.FirstAsync(q=> q.ContractId == request.ContractId);
                entity.ContractStatusId = contractRenewedStatusId.Value;
                entity.TenantId = request.NewTenantId;
                entity.HouseId = request.NewHouseId.Value;
                entity.RentPrice = request.NewRent.Value;
                entity.RentDeposit = request.NewDeposit.Value;
                entity.Update(request.UserId);
                _repository.Update(entity);

                //=================================================
                //PERSIST INFORMATION
                //=================================================
                await _unitOfWork.CommitAsync();

                return entity.ToRegisterdResult().WithId(entity.ContractId);
            }
            catch (Exception ex)
            {
                //await SendLogToAmigoTenantTEventLog(message, ex.Message);
                throw;
            }
        }

        private async Task<List<PaymentPeriod>> CreatePaymentsByPeriod(Period currentPeriod, List<Period> periodList, ContractChangeTermCommand request, PaymentPeriod lastPaymentPeriod, int? rentConceptId)
        {
            var id = -1;
            var paymentPeriodList = new List<PaymentPeriod>();
            var depositConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Deposit);
            var paymentPendingStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);
            var paymentTypeRentId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Rent);
            var paymentTypeDepositId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Deposit);

            request.NewTenantId = request.NewHouseId.HasValue ? request.NewTenantId : lastPaymentPeriod.TenantId;
            request.NewRent = request.NewRent.HasValue ? request.NewRent : lastPaymentPeriod.PaymentAmount;
            request.NewHouseId = request.NewHouseId.HasValue ? request.NewHouseId : lastPaymentPeriod.HouseId;

            foreach (var newPeriod in periodList)
            {
                SetNewPaymentsPeriods(paymentPeriodList, request, newPeriod, id, rentConceptId, paymentPendingStatusId, paymentTypeRentId, depositConceptId, paymentTypeDepositId, lastPaymentPeriod);
                id--;
            }
            return paymentPeriodList;
        }

        private void SetNewPaymentsPeriods(List<PaymentPeriod> paymentsPeriod, ContractChangeTermCommand request, Period newPeriod, int id, int? rentId, int? paymentPendingStatusId, int? paymentTypeId, int? depositConceptId, int? paymentTypeDepositId, PaymentPeriod lastPaymentPeriod)
        {
            

            ///////////////////
            //SETTING FOR  DEPOSIT
            ///////////////////

            if (Math.Abs(id) == 1 && request.NewDeposit.HasValue)
            {
                //Aqui por la pregunta para entrar a la condicion
                //request.NewDeposit = request.NewDeposit.HasValue ? request.NewDeposit : request.NewDeposit;

                var paymentPeriodDeposit = new PaymentPeriod();
                paymentPeriodDeposit.PaymentPeriodId = id;
                paymentPeriodDeposit.ConceptId = depositConceptId; //"CODE FOR CONCEPT"; //TODO:
                paymentPeriodDeposit.ContractId = request.ContractId;

                paymentPeriodDeposit.TenantId = request.NewTenantId;
                paymentPeriodDeposit.PaymentTypeId = paymentTypeDepositId;
                paymentPeriodDeposit.PaymentAmount = request.NewDeposit;
                paymentPeriodDeposit.PeriodId = newPeriod.PeriodId;
                paymentPeriodDeposit.DueDate = newPeriod.DueDate;
                paymentPeriodDeposit.HouseId = request.NewHouseId;

                paymentPeriodDeposit.PaymentPeriodStatusId = paymentPendingStatusId; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
                paymentPeriodDeposit.RowStatus = true;
                paymentPeriodDeposit.CreatedBy = request.UserId;
                paymentPeriodDeposit.CreationDate = DateTime.Now;
                paymentPeriodDeposit.UpdatedBy = request.UserId;
                paymentPeriodDeposit.UpdatedDate = DateTime.Now;

                paymentsPeriod.Add(paymentPeriodDeposit);
            }

            ///////////////////
            //SETTING FOR  RENT
            ///////////////////

            var paymentPeriodRent = new PaymentPeriod();
            paymentPeriodRent.PaymentPeriodId = id;
            paymentPeriodRent.ConceptId = rentId; //"CODE FOR CONCEPT"; //TODO:
            paymentPeriodRent.ContractId = request.ContractId;
            paymentPeriodRent.TenantId = request.NewTenantId;
            paymentPeriodRent.PeriodId = newPeriod.PeriodId;
            paymentPeriodRent.PaymentPeriodStatusId = paymentPendingStatusId; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
            paymentPeriodRent.RowStatus = true;
            paymentPeriodRent.CreatedBy = request.UserId;
            paymentPeriodRent.CreationDate = DateTime.Now;
            paymentPeriodRent.UpdatedBy = request.UserId;
            paymentPeriodRent.UpdatedDate = DateTime.Now;
            paymentPeriodRent.PaymentTypeId = paymentTypeId;
            paymentPeriodRent.PaymentAmount = request.NewRent;
            paymentPeriodRent.DueDate = newPeriod.DueDate;
            paymentPeriodRent.HouseId = request.NewHouseId;

            paymentsPeriod.Add(paymentPeriodRent);
        }

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
