using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.MasterData.Houses;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.Mapping
{
    public class HouseProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<RegisterHouseCommand, House>();
            Mapper.Register<UpdateHouseCommand, House>();
            Mapper.Register<DeleteHouseCommand, House>();

            Mapper.Register<RegisterHouseFeatureCommand, HouseFeature>();
            Mapper.Register<UpdateHouseFeatureCommand, HouseFeature>();
            Mapper.Register<DeleteHouseFeatureCommand, HouseFeature>();
        }
    }
}
