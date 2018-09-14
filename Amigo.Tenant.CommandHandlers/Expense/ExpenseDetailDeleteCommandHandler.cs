
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
    public class ExpenseDetailDeleteCommandHandler : IAsyncCommandHandler<ExpenseDetailDeleteCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<model.ExpenseDetail> _repository;
        

        public ExpenseDetailDeleteCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.ExpenseDetail> repository,
         IUnitOfWork unitOfWork   )
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
           
        }


        public async Task<CommandResult> Handle(ExpenseDetailDeleteCommand message)
        {
            try
            {
                var entity = _mapper.Map<ExpenseDetailDeleteCommand, model.ExpenseDetail>(message);
                entity.RowStatus = false;
                entity.Update(message.UserId);
                //=================================================
                //Contract
                //=================================================

                _repository.UpdatePartial(entity, new string[] { "ExpenseDetailId",
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
