using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.BussinesEvents.MasterData;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.MasterData.MainTenants;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.MasterData.MainTenants
{
    public class UpdateMainTenantCommandHandler : IAsyncCommandHandler<UpdateMainTenantCommand>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MainTenant> _mainTenantRepository;

        public UpdateMainTenantCommandHandler(
            IBus bus,
            IMapper mapper,
            IRepository<MainTenant> deviceRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _mainTenantRepository = deviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(UpdateMainTenantCommand message)
        {
            //Validate using domain models
            MainTenant entity =  _mapper.Map<UpdateMainTenantCommand, MainTenant>(message);

            //if is not valid
            //if (entity.HasErrors) return entity.ToResult();
            entity.UpdatedBy = message.UserId;
            entity.UpdatedDate = DateTime.Now;

            _mainTenantRepository.UpdatePartial(entity, new string[] { "TenantId",
                        "Code",
                        "FullName",
                        "CountryId",
                        "PassportNo",
                        "PhoneN01",
                        "Email",
                        "RowStatus",
                        "UpdatedBy",
                        "UpdatedDate",
                        "TypeId",
                        "Address",
                        "Reference",
                        "PhoneNo2",
                        "ContactName",
                        "ContactPhone",
                        "ContactEmail",
                        "ContactRelation",
                        "IdRef"
            }); 
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            //await _bus.PublishAsync(new MainTenantUpdated() { TenantId = entity.TenantId });

            //Return result
            return entity.ToResult();
        }
    }
}
