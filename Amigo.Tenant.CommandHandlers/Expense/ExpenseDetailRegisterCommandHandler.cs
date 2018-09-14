
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.ExpenseDetail;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
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


        public ExpenseDetailRegisterCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.ExpenseDetail> repository,
         IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
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

                _repository.Add(entity);
                await _unitOfWork.CommitAsync();

                if (entity.ExpenseDetailId != 0)
                {
                    message.ExpenseDetailId = entity.ExpenseDetailId;
                }
                return entity.ToRegisterdResult().WithId(entity.ExpenseDetailId.Value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
