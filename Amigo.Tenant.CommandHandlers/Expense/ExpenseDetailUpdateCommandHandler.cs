
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
    public class ExpenseDetailUpdateCommandHandler : IAsyncCommandHandler<ExpenseDetailUpdateCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<model.ExpenseDetail> _repository;

        public ExpenseDetailUpdateCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.ExpenseDetail> repository,
         IUnitOfWork unitOfWork
            )
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }


        public async Task<CommandResult> Handle(ExpenseDetailUpdateCommand message)
        {
            try
            {
                var entity = _mapper.Map<ExpenseDetailUpdateCommand, model.ExpenseDetail>(message);
                entity.Update(message.UserId);

                //=================================================
                //Detail
                //=================================================

                _repository.UpdatePartial(entity, new string[] {
                    "ExpenseDetailId",
                    "ConceptId",
                    "TenantId",
                    "Remark",
                    "Amount",
                    "Tax",
                    "TotalAmount",
                    "Quantity",
                    "UpdatedBy",
                    "UpdatedDate",
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
