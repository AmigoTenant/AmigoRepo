using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.MasterData.RentalApplication
{
    public class RentalApplicationFeatureCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int? RentalApplicationFeatureId { get; set; }
        public int? RentalApplicationId { get; set; }
        public int? FeatureId { get; set; }
    }
}
