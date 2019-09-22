
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
    public class ContractDeleteCommandHandler : IAsyncCommandHandler<ContractDeleteCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Contract> _repository;
        private readonly IRepository<ContractChangeStatus> _repositoryContractChangeStatus;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;
        private readonly IRepository<Period> _repositoryPeriod;

        public ContractDeleteCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<Contract> repository,
         IUnitOfWork unitOfWork,
         IRepository<ContractChangeStatus> repositoryContractChangeStatus,
         IRepository<EntityStatus> repositoryEntityStatus,
         IRepository<Period> repositoryPeriod)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryContractChangeStatus = repositoryContractChangeStatus;
            _repositoryEntityStatus = repositoryEntityStatus;
            _repositoryPeriod = repositoryPeriod;
        }

        public async Task<CommandResult> Handle(ContractDeleteCommand message)
        {
            try
            {
                //=================================================
                //Contract
                //=================================================
                var entity = await _repository.FirstOrDefaultAsync(q => q.ContractId == message.ContractId);
                entity.RowStatus = false;
                entity.Update(message.UserId);
                _repository.Update(entity);

                //=================================================
                //ContractChangeStatus
                //=================================================
                await CreateContractChangeStatus(entity);

                //var entity = _mapper.Map<ContractDeleteCommand, Contract>(message);
                //entity.RowStatus = false;
                //entity.Update(message.UserId);
                //=================================================
                //Contract
                //=================================================
                //_repository.UpdatePartial(entity, new string[] { "ContractId",
                //    "RowStatus",
                //    "UpdatedDate",
                //    "UpdatedBy",
                //    });

                await _unitOfWork.CommitAsync();
                return entity.ToResult();
            }
            catch (Exception ex)
            {
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
                ContractTermType = Constants.ContractTypeTerm.Delete,
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


    }
}
