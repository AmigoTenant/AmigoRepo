
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
        private readonly IRepository<ContractChangeStatus> _repositoryContractChangeStatus;

        public ContractChangeTermCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<Contract> repository,
         IUnitOfWork unitOfWork,
         IRepository<PaymentPeriod> repositoryPayment,
         IRepository<Period> repositoryPeriod,
         IRepository<Concept> repositoryConcept,
         IRepository<EntityStatus> repositoryEntityStatus,
         IRepository<GeneralTable> repositoryGeneralTable,
         IRepository<ContractChangeStatus> repositoryContractChangeStatus)
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
            _repositoryContractChangeStatus = repositoryContractChangeStatus;
        }


        public async Task<CommandResult> Handle(ContractChangeTermCommand request)
        {
            try
            {
                var entity = await CreateOrModifyPaymentPeriod(request);

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

        private async Task<Contract> UpdateContract(ContractChangeTermCommand request, DateTime? contractEndDate)
        {
            int? contractRenewedStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.Contract, Constants.EntityStatus.Contract.Renewed);
            var entity = await _repository.FirstAsync(q => q.ContractId == request.ContractId);
            entity.ContractStatusId = contractRenewedStatusId.Value;
            entity.TenantId = request.NewTenantId;
            entity.HouseId = request.NewHouseId.Value;
            entity.RentPrice = request.NewRent.Value;
            if (request.ContractTermType == Constants.ContractTypeTerm.Extension && request.NewDeposit.HasValue)
            {
                entity.RentDeposit = request.NewDeposit.Value;
            }
            if (request.ContractTermType == Constants.ContractTypeTerm.Extension && contractEndDate.HasValue)
            {
                entity.EndDate = contractEndDate;
            }
            entity.Update(request.UserId);
            _repository.Update(entity);

            //=================================================
            //ContractChangeStatus
            //=================================================
            await CreateContractChangeStatus(entity, request.ContractTermType);

            return entity;
        }

        private async Task<Contract> CreateOrModifyPaymentPeriod(ContractChangeTermCommand request)
        {
            var rentConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Rent);
            var depositConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Deposit);
            var paymentPendingStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);
            var paymentTypeRentId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Rent);
            var paymentTypeDepositId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Deposit);

            //Ultimo Periodo del contrato Actual para el concepto Renta
            string[] includes = new string[] { "Period" };
            List<OrderExpression<PaymentPeriod>> orderExpressionList = new List<OrderExpression<PaymentPeriod>>();
            orderExpressionList.Add(new OrderExpression<PaymentPeriod>(OrderType.Desc, p => p.Period.Code.ToString()));
            Expression<Func<PaymentPeriod, bool>> queryFilter = q => q.RowStatus && q.ContractId == request.ContractId && q.ConceptId == rentConceptId;
            var lastPaymentPeriodByContract = (await _repositoryPayment.FirstOrDefaultAsync(queryFilter, orderExpressionList.ToArray(), includes: includes));

            //Reasignacion de valores
            request.NewTenantId = request.NewTenantId.HasValue ? request.NewTenantId : lastPaymentPeriodByContract.TenantId;
            request.NewRent = request.NewRent.HasValue ? request.NewRent : lastPaymentPeriodByContract.PaymentAmount;
            request.NewHouseId = request.NewHouseId.HasValue ? request.NewHouseId : lastPaymentPeriodByContract.HouseId;

            //Periodo hasta donde hay que traer
            Period finalNewPeriod = new Period();
            Period startPeriodOnPaymentPeriod = new Period();
            List<Period> newPeriodList;
            DateTime? contractEndDate= null;

            if (request.ContractTermType == Constants.ContractTypeTerm.Extension)
            {
                finalNewPeriod = await _repositoryPeriod.FirstOrDefaultAsync(q => q.PeriodId == request.FinalPeriodId);
                newPeriodList = (await _repositoryPeriod.ListAsync(q => q.Sequence > lastPaymentPeriodByContract.Period.Sequence &&
                                                                    q.Sequence <= finalNewPeriod.Sequence &&
                                                                    q.RowStatus)).OrderBy(q => q.Sequence).ToList();
                contractEndDate = finalNewPeriod.EndDate;
                var id = -1;
                foreach (var newPeriod in newPeriodList)
                {
                    await SetNewPaymentsPeriod(request, newPeriod, id--, rentConceptId, paymentPendingStatusId, paymentTypeRentId, depositConceptId, paymentTypeDepositId);
                }

            }
            else 
            {
                startPeriodOnPaymentPeriod = await _repositoryPeriod.FirstOrDefaultAsync(q => q.PeriodId == request.FromPeriodId);
                newPeriodList = (await _repositoryPeriod.ListAsync(q => q.Sequence >= startPeriodOnPaymentPeriod.Sequence &&
                                                                    q.Sequence <= lastPaymentPeriodByContract.Period.Sequence &&
                                                                    q.RowStatus)).OrderBy(q => q.Sequence).ToList();
                var id = -1;
                foreach (var newPeriod in newPeriodList)
                {
                    await SetExistingPaymentsPeriod(request, newPeriod, rentConceptId, paymentPendingStatusId);
                }
            }

            return await UpdateContract(request, contractEndDate);

        }

        private async Task SetNewPaymentsPeriod(ContractChangeTermCommand request, Period newPeriod, int id, int? rentConceptId, int? paymentPendingStatusId, int? paymentTypeId, int? depositConceptId, int? paymentTypeDepositId)
        {
            ///////////////////
            //SETTING FOR  DEPOSIT
            ///////////////////

            if (Math.Abs(id) == 1 && request.NewDeposit.HasValue)
            {
                var paymentPeriodDeposit = new PaymentPeriod();
                paymentPeriodDeposit.PaymentPeriodId = id;
                paymentPeriodDeposit.ConceptId = depositConceptId; 
                paymentPeriodDeposit.ContractId = request.ContractId;

                paymentPeriodDeposit.TenantId = request.NewTenantId;
                paymentPeriodDeposit.PaymentTypeId = paymentTypeDepositId;
                paymentPeriodDeposit.PaymentAmount = request.NewDeposit;
                paymentPeriodDeposit.PeriodId = newPeriod.PeriodId;
                paymentPeriodDeposit.DueDate = newPeriod.DueDate;
                paymentPeriodDeposit.HouseId = request.NewHouseId;

                paymentPeriodDeposit.PaymentPeriodStatusId = paymentPendingStatusId; 
                paymentPeriodDeposit.RowStatus = true;
                paymentPeriodDeposit.CreatedBy = request.UserId;
                paymentPeriodDeposit.CreationDate = DateTime.Now;
                paymentPeriodDeposit.UpdatedBy = request.UserId;
                paymentPeriodDeposit.UpdatedDate = DateTime.Now;

                _repositoryPayment.Add(paymentPeriodDeposit);
            }

            ///////////////////
            //SETTING FOR  RENT
            ///////////////////

            var paymentPeriodRent = new PaymentPeriod();
            paymentPeriodRent.PaymentPeriodId = id--;
            paymentPeriodRent.ConceptId = rentConceptId; 
            paymentPeriodRent.ContractId = request.ContractId;
            

            paymentPeriodRent.TenantId = request.NewTenantId;
            paymentPeriodRent.PaymentTypeId = paymentTypeId;
            paymentPeriodRent.PaymentAmount = request.NewRent;
            paymentPeriodRent.PeriodId = newPeriod.PeriodId;
            paymentPeriodRent.DueDate = newPeriod.DueDate;
            paymentPeriodRent.HouseId = request.NewHouseId;

            paymentPeriodRent.PaymentPeriodStatusId = paymentPendingStatusId; 
            paymentPeriodRent.RowStatus = true;
            paymentPeriodRent.CreatedBy = request.UserId;
            paymentPeriodRent.CreationDate = DateTime.Now;
            paymentPeriodRent.UpdatedBy = request.UserId;
            paymentPeriodRent.UpdatedDate = DateTime.Now;

            _repositoryPayment.Add(paymentPeriodRent);
        }

        private async Task SetExistingPaymentsPeriod(ContractChangeTermCommand request, Period newPeriod, int? rentConceptId, int? paymentPendingStatusId)
        {
            //Esta Logica no cambia depositos
            var paymentPeriodRent = await _repositoryPayment.FirstOrDefaultAsync(q => q.PeriodId == newPeriod.PeriodId 
                                            && q.PaymentPeriodStatusId == paymentPendingStatusId
                                            && q.ConceptId == rentConceptId
                                            && q.RowStatus);

            if (paymentPeriodRent != null)
            {
                paymentPeriodRent.ContractId = request.ContractId;
                paymentPeriodRent.TenantId = request.NewTenantId;
                paymentPeriodRent.PaymentAmount = request.NewRent;
                paymentPeriodRent.HouseId = request.NewHouseId;
                paymentPeriodRent.UpdatedBy = request.UserId;
                paymentPeriodRent.UpdatedDate = DateTime.Now;
                _repositoryPayment.Update(paymentPeriodRent);
            }
        }

        private async Task CreateContractChangeStatus(Contract entity, string contractTermType)
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
                ContractTermType = contractTermType,
                BeginPeriodId = entity.PeriodId,
                EndPeriodId = endPeriod.PeriodId,
                CreatedBy = entity.UpdatedBy,
                CreationDate = entity.UpdatedDate,
                UpdatedBy = entity.UpdatedBy,
                UpdatedDate = entity.UpdatedDate
            };
            if (contractTermType == Constants.ContractTypeTerm.Extension)
            {
                contractChangeStatus.Deposit = entity.RentDeposit;
            }

            _repositoryContractChangeStatus.Add(contractChangeStatus);
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
