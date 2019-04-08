
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.ExpenseDetail;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using model = Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandHandlers.Expense
{
    public class ExpenseDetailRegisterCommandHandler : IAsyncCommandHandler<ExpenseDetailRegisterCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<model.ExpenseDetail> _repository;
        private readonly IRepository<model.PaymentPeriod> _repositoryPayment;
        private readonly IRepository<model.Contract> _repositoryContract;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;


        public ExpenseDetailRegisterCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.ExpenseDetail> repository,
         IUnitOfWork unitOfWork,
         IRepository<model.PaymentPeriod> repositoryPayment,
         IRepository<EntityStatus> repositoryEntityStatus)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _repositoryPayment = repositoryPayment;
            _repositoryEntityStatus = repositoryEntityStatus;
        }


        public async Task<CommandResult> Handle(ExpenseDetailRegisterCommand message)
        {
            try
            {
                var entity = _mapper.Map<ExpenseDetailRegisterCommand, model.ExpenseDetail>(message);

                //Insert
                entity.RowStatus = true;
                message.RowStatus = true;
                entity.Creation(message.UserId);
                var expensePending = await _repositoryEntityStatus.FirstOrDefaultAsync(q => q.EntityCode == Constants.EntityCode.Expense && q.Code == Constants.EntityStatus.Expense.Pending);

                model.ExpenseDetail obj = new model.ExpenseDetail();
                if (message.ApplyTo.Value == 66) //Cambiar para que pregunte por el codigo NO  por el ID
                {

                    string[] includes = new string[] { "Contract" };
                    var payments = await _repositoryPayment.ListAsync(q => q.PeriodId == message.PeriodId && q.Contract.HouseId == message.HouseId && q.ConceptId == 15, null, includes); //Cambiar 15 por el codigo del concepto RENTA
                    foreach (var item in payments)
                    {
                        obj = new model.ExpenseDetail();
                        obj = entity;
                        obj.TenantId = item.TenantId;
                        obj.ExpenseDetailStatusId = (expensePending != null ? expensePending.EntityStatusId : (int?)null); //PENDING
                        _repository.Add(obj);
                        await _unitOfWork.CommitAsync();
                    }
                }
                else
                {
                    obj = entity;
                    obj.ExpenseDetailStatusId = (expensePending != null ? expensePending.EntityStatusId : (int?)null); //PENDING
                    _repository.Add(obj);
                    await _unitOfWork.CommitAsync();
                }
                
                if (obj.ExpenseDetailId != 0)
                {
                    message.ExpenseDetailId = obj.ExpenseDetailId;
                }
                return obj.ToRegisterdResult().WithId(obj.ExpenseDetailId.Value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
