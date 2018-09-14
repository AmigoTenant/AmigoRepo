using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.MasterData.RentalApplication
{
    public class RentalApplicationCityCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? RentalApplicationCityId { get; set; }
        public int? RentalApplicationId { get; set; }
        public int? CityId { get; set; }
    }
}
