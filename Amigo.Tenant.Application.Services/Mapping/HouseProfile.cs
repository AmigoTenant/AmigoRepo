using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Houses;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.MasterData.Houses;
using Amigo.Tenant.Commands.Security.Permission;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Mapping
{
    public class HouseProfile : Profile
    {
        public override void Register()
        {
            Mapper.Register<HouseServiceRequest, RegisterHouseServiceCommand>();

            Mapper.Register<HouseServicePeriodRequest, RegisterHouseServiceCommand>();

            Mapper.Register<HouseServiceMonthDayRequest, RegisterHouseServiceMonthDayCommand>();

            Mapper.Register<HouseService, HouseServiceDTO>();

            Mapper.Register<RegisterHouseServiceCommand, HouseService>();

            Mapper.Register<RegisterHouseServicePeriodCommand, HouseServicePeriod>();
        }
    }
}
