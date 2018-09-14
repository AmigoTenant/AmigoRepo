using ExpressMapper;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Tracking.Approve;
using Amigo.Tenant.Commands.Tracking.Moves;
using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Commands.Leasing.Contracts;

namespace Amigo.Tenant.CommandHandlers.Mapping
{
    public class ContractProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<ContractRegisterCommand, Contract>();
            //Mapper.Register<UpdateAmigoTenantServiceCommand, AmigoTenantTService>();
            
            //TODO:Insert mapper here Event
            //Mapper.Register<RegisterAmigoTenanttServiceCommand, RegisterMoveEvent>();
            //Mapper.Register<UpdateAmigoTenantServiceCommand, RegisterMoveEvent>();

            //Mapper.Register<UpdateAmigoTenantTServiceApproveCommand, RegisterMoveEvent>();

            //Mapper.Register<RegisterDriverReportCommand, RegisterMoveEvent>();

            //For approve
            //Mapper.Register<RegisterDriverReportCommand, DriverReport>();
            //Mapper.Register<UpdateAmigoTenantTServiceApproveCommand, AmigoTenantTService>();

            //For Acknowledge
            //Mapper.Register<UpdateAmigoTenantServiceAckCommand, AmigoTenantTService>();
            //Mapper.Register<UpdateAmigoTenantServiceAckCommand, RegisterMoveEvent>();

            //For Cancel
            //Mapper.Register<CancelAmigoTenantServiceCommand, AmigoTenantTService>();
            //Mapper.Register<CancelAmigoTenantServiceCommand, RegisterMoveEvent>();

        }
    }
}
