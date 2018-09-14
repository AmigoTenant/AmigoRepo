using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressMapper;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Events.Tracking;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;

namespace Amigo.Tenant.EventHandlers.Mapping
{
    public class AmigoTenantLogProfile: Profile
    {
        public override void Register()
        {
            //Mapper.Register<>();
            Mapper.Register<RegisterMoveEvent, AmigoTenantTEventLog>();

        }
    }
}
