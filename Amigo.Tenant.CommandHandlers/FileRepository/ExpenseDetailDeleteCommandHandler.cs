
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Expense;
using Amigo.Tenant.Commands.FileRepository;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Threading.Tasks;
using model = Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandHandlers.Expense
{
    public class FileRepositoryDeleteCommandHandler : IAsyncCommandHandler<FileRepositoryDeleteCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<model.FileRepository> _repository;
        

        public FileRepositoryDeleteCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.FileRepository> repository,
         IUnitOfWork unitOfWork   )
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(FileRepositoryDeleteCommand message)
        {
            try
            {
                //var entity = _mapper.Map<FileRepositoryDeleteCommand, model.FileRepository>(message);
                var entity = await _repository.FirstOrDefaultAsync(q => q.FileRepositoryId.Value == message.FileRepositoryId);

                if (entity != null)
                {
                    _repository.Delete(entity);
                    await _unitOfWork.CommitAsync();
                }

                return entity.ToResult();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
