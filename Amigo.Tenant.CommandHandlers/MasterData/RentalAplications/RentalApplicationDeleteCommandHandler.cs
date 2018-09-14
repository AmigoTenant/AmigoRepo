
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
using Amigo.Tenant.Commands.MasterData.RentalApplication;

namespace Amigo.Tenant.CommandHandlers.MasterData.RentalApplications
{
    public class RentalApplicationDeleteCommandHandler : IAsyncCommandHandler<RentalApplicationDeleteCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<RentalApplication> _repository;
        

        public RentalApplicationDeleteCommandHandler(
         IBus bus,
         IMapper mapper,
         IRepository<RentalApplication> repository,
         IUnitOfWork unitOfWork   )
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
           
        }


        public async Task<CommandResult> Handle(RentalApplicationDeleteCommand message)
        {
            try
            {
                var entity = _mapper.Map<RentalApplicationDeleteCommand, RentalApplication>(message);
                entity.RowStatus = false;
                entity.Update(message.UserId);
                //=================================================
                //Contract
                //=================================================

                _repository.UpdatePartial(entity, new string[] { "RentalApplicationId",
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
