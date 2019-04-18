
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Expense;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using model = Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandHandlers.Expenses
{
    public class ExpenseRegisterCommandHandler : IAsyncCommandHandler<ExpenseRegisterCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<model.Expense> _repository;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;
        private readonly IRepository<GeneralTable> _repositoryGeneralTable;

        public ExpenseRegisterCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.Expense> repository,
         IUnitOfWork unitOfWork,
         IRepository<EntityStatus> repositoryEntityStatus,
         IRepository<GeneralTable> repositoryGeneralTable)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryEntityStatus = repositoryEntityStatus;
            _repositoryGeneralTable = repositoryGeneralTable;
        }


        public async Task<CommandResult> Handle(ExpenseRegisterCommand message)
        {
            try
            {
                var entity = _mapper.Map<ExpenseRegisterCommand, model.Expense>(message);

                //Insert
                entity.RowStatus = true;
                message.RowStatus = true;
                entity.Creation(message.UserId);

                if (message.AddAutomaticDetail.HasValue && message.AddAutomaticDetail.Value)
                {
                    var expensePending = await _repositoryEntityStatus.FirstOrDefaultAsync(q => q.EntityCode == Constants.EntityCode.Expense && q.Code == Constants.EntityStatus.Expense.Pending);
                    var applyToPeriod = await _repositoryGeneralTable.FirstOrDefaultAsync(q => q.TableName == Constants.GeneralTableName.ApplyTo && q.Code == Constants.GeneralTableCode.ApplyTo.Period && q.RowStatus);

                    var detail = new model.ExpenseDetail()
                    {
                        ExpenseId = message.ExpenseId,
                        ApplyTo = applyToPeriod.GeneralTableId,
                        ConceptId = message.ConceptId,
                        ExpenseDetailId = -1,
                        ExpenseDetailStatusId = (expensePending != null ? expensePending.EntityStatusId : (int?)null), //PENDING
                        SubTotalAmount = message.SubTotalAmount,
                        Tax = message.Tax,
                        TotalAmount = message.TotalAmount,
                        Quantity = 1,
                        Remark = message.Remark,
                        RowStatus = true,
                        CreatedBy = entity.CreatedBy,
                        CreationDate = entity.CreationDate,
                    };
                    entity.ExpenseDetails = new List<ExpenseDetail>{detail};
                }


                _repository.Add(entity);
                await _unitOfWork.CommitAsync();

                if (entity.ExpenseId != 0)
                {
                    message.ExpenseId = entity.ExpenseId;
                }
                return entity.ToRegisterdResult().WithId(entity.ExpenseId.Value);
            }
            catch (Exception ex)
            {
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
