using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.MasterData.MainTenants;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.CommandModel.BussinesEvents.MasterData;

namespace Amigo.Tenant.CommandHandlers.MasterData.MainTenants
{
    public class RegisterMainTenantCommandHandler : IAsyncCommandHandler<RegisterMainTenantCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MainTenant> _mainTenantRepository;

        public RegisterMainTenantCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<MainTenant> MainTenantRepository,
            IUnitOfWork unitOfWork)

        {
            _bus = bus;
            _mapper = mapper;
            _mainTenantRepository = MainTenantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(RegisterMainTenantCommand message)
        {
            //Validate using domain models
            var mainTenant = _mapper.Map<RegisterMainTenantCommand, MainTenant>(message);

            mainTenant.RowStatus = true;
            mainTenant.Creation(message.UserId);

            //if is not valid
            if (mainTenant.HasErrors) return mainTenant.ToResult();

            //Insert
            _mainTenantRepository.Add(mainTenant);
            await _unitOfWork.CommitAsync();

            if (mainTenant.TenantId != 0)
            {
                message.TenantId = mainTenant.TenantId;

                //Publish bussines Event
                //await SendLogToAmigoTenantTEventLog(message);
            }

            //Publish bussines Event
            //await _bus.PublishAsync(new MainTenantRegistered() { TenantId = mainTenant.TenantId });

            //Return result
            return mainTenant.ToResult();
        }
    }
}
