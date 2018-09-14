using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.MasterData.MainTenants;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.CommandModel.BussinesEvents.MasterData;

namespace Amigo.Tenant.CommandHandlers.MasterData.MainTenants
{
    public class DeleteMainTenantCommandHandler : IAsyncCommandHandler<DeleteMainTenantCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MainTenant> _mainTenantRepository;

        public DeleteMainTenantCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<MainTenant> mainTenantRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _mainTenantRepository = mainTenantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(DeleteMainTenantCommand message)
        {
            MainTenant mainTenant = new MainTenant();
            mainTenant = _mapper.Map<DeleteMainTenantCommand, MainTenant>(message);
            mainTenant.RowStatus = false;
            mainTenant.Update(message.UserId);
            if (mainTenant.HasErrors) return mainTenant.ToResult();

            _mainTenantRepository.UpdatePartial(mainTenant, new string[] { "TenantId",
                        "RowStatus",
                        "UpdatedBy",
                        "UpdatedDate"
            });
            await _unitOfWork.CommitAsync();
            //await _bus.PublishAsync(new MainTenantDeleted() { TenantId = mainTenant.TenantId });

            return mainTenant.ToResult();
        }
    }
}
