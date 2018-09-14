using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Commands.MasterData.MainTenants;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Mapping
{
    public class MainTenantProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterMainTenantRequest, RegisterMainTenantCommand>();
            Mapper.Register<UpdateMainTenantRequest, UpdateMainTenantCommand>();
            Mapper.Register<DeleteMainTenantRequest, DeleteMainTenantCommand>();
        }
    }
}
