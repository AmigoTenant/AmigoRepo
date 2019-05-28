
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Expense;
using Amigo.Tenant.Commands.FileRepository;
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
    public class FileRepositoryRegisterCommandHandler : IAsyncCommandHandler<FileRepositoryRegisterCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<model.FileRepository> _repository;

        public FileRepositoryRegisterCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<model.FileRepository> repository,
         IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }


        public async Task<CommandResult> Handle(FileRepositoryRegisterCommand message)
        {
            try
            {
                var entity = _mapper.Map<FileRepositoryRegisterCommand, model.FileRepository>(message);

                //Insert
                entity.RowStatus = true;
                entity.Creation(message.UserId);
                _repository.Add(entity);
                await _unitOfWork.CommitAsync();

                //if (entity.FileRepositoryId != 0)
                //{
                //    message.FileRepositoryId = entity.FileRepositoryId.Value;
                //}
                return entity.ToRegisterdResult().WithId(entity.FileRepositoryId.Value);
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
