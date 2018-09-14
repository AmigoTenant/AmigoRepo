
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Expense;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Threading.Tasks;
using model = Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandHandlers.Expense
{
    public class ExpenseDeleteCommandHandler : IAsyncCommandHandler<ExpenseDeleteCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<model.Expense> _repository;
        

        public ExpenseDeleteCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.Expense> repository,
         IUnitOfWork unitOfWork   )
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
           
        }


        public async Task<CommandResult> Handle(ExpenseDeleteCommand message)
        {
            try
            {
                var entity = _mapper.Map<ExpenseDeleteCommand, model.Expense>(message);
                entity.RowStatus = false;
                entity.Update(message.UserId);
                //=================================================
                //Contract
                //=================================================

                _repository.UpdatePartial(entity, new string[] { "ExpenseId",
                    "RowStatus",
                    "UpdatedDate",
                    "UpdatedBy",
                    });

                await _unitOfWork.CommitAsync();
                return entity.ToResult();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        
    }
}
