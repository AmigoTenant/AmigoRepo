
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
using Amigo.Tenant.Application.DTOs.Requests.Common;
using System.Linq;
using Amigo.Tenant.Common;

namespace Amigo.Tenant.CommandHandlers.Leasing.Contracts
{
    public class ContractUpdateCommandHandler : IAsyncCommandHandler<ContractUpdateCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Contract> _repository;
        private readonly IRepository<OtherTenant> _repositoryOtherTenant;
        private readonly IRepository<ContractHouseDetail> _repositoryContractHouseDetail;
        private readonly IRepository<ContractChangeStatus> _repositoryContractChangeStatus;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;
        private readonly IRepository<Period> _repositoryPeriod;

        public ContractUpdateCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<Contract> repository,
         IUnitOfWork unitOfWork,
         IRepository<OtherTenant> repositoryOtherTenant,
         IRepository<ContractHouseDetail> repositoryContractHouseDetail,
         IRepository<ContractChangeStatus> repositoryContractChangeStatus,
         IRepository<EntityStatus> repositoryEntityStatus,
         IRepository<Period> repositoryPeriod)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryOtherTenant = repositoryOtherTenant;
            _repositoryContractHouseDetail = repositoryContractHouseDetail;
            _repositoryContractChangeStatus = repositoryContractChangeStatus;
            _repositoryEntityStatus = repositoryEntityStatus;
            _repositoryPeriod = repositoryPeriod;
        }


        public async Task<CommandResult> Handle(ContractUpdateCommand message)
        {
            try
            {
                var entity = _mapper.Map<ContractUpdateCommand, Contract>(message);
                entity.Update(message.UserId);

                //=================================================
                //Contract
                //=================================================

                _repository.UpdatePartial(entity, new string[] { "ContractId",
                    "BeginDate",
                    "EndDate",
                    "RentDeposit",
                    "RentPrice",
                    "ContractDate",
                    "PaymentModeId",
                    "ContractStatusId",
                    "PeriodId",
                    "ContractCode",
                    "ReferencedBy",
                    "HouseId",
                    "UpdatedBy",
                    "UpdatedDate",
                    "FrecuencyTypeId",
                    "TenantId"});

                //=================================================
                //ContractChangeStatus
                //=================================================
                await CreateContractChangeStatus(entity);

                //=================================================
                // Other Tenant
                //=================================================

                foreach (var item in message.OtherTenants)
                {
                    if (item.TableStatus == ObjectStatus.Added)
                    {
                        var otherTenant = _mapper.Map<OtherTenantRegisterCommand, OtherTenant>(item);
                        otherTenant.CreatedBy = message.UserId;
                        otherTenant.CreationDate = DateTime.Now;
                        _repositoryOtherTenant.Add(otherTenant);
                    }
                    if (item.TableStatus == ObjectStatus.Deleted)
                    {
                        var existInDB = await _repositoryOtherTenant.ListAsync(w => w.OtherTenantId == item.OtherTenantId);
                        existInDB.ToList().ForEach(p => _repositoryOtherTenant.Delete(p));
                    }
                }

                //=================================================
                // Contract House Detail
                //=================================================

                foreach (var item in message.ContractHouseDetails)
                {
                    if (item.TableStatus == ObjectStatus.Added)
                    {
                        var contractHouseDetailEntity = _mapper.Map<ContractHouseDetailRegisterCommand, ContractHouseDetail>(item);
                        contractHouseDetailEntity.CreatedBy = message.UserId;
                        contractHouseDetailEntity.CreationDate = DateTime.Now;
                        _repositoryContractHouseDetail.Add(contractHouseDetailEntity);
                    }
                    if (item.TableStatus == ObjectStatus.Deleted)
                    {
                        var contractHouseDetail = await _repositoryContractHouseDetail.FirstAsync(w => w.ContractHouseDetailId== item.ContractHouseDetailId);
                        if (contractHouseDetail != null)
                        _repositoryContractHouseDetail.Delete(contractHouseDetail);
                    }
                }


                await _unitOfWork.CommitAsync();
                return entity.ToResult();
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
                ContractTermType = Constants.ContractTypeTerm.Update,
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
