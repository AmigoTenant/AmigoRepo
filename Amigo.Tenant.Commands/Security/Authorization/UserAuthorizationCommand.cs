using System;
using MediatR;
using Amigo.Tenant.Commands.Common;

namespace Amigo.Tenant.Commands.Security.Authorization
{
    public class UserAuthorizationCommand : IRequest<CommandResult>
    {
        public string AssignedAmigoTenantTUserUsername { get; set; }
        public int? DedicatedLocationId { get; set; }
        public bool? BypassDeviceValidation { get; set; }
        public string DedicatedLocationCode { get; set; }
        public DateTime? UserUpdatedDate { get; set; }
        public int? DeviceId { get; set; }
        public string CellphoneNumber { get; set; }
        public string Identifier { get; set; }
        public int? AssignedAmigoTenantTUserId { get; set; }
        public string PayBy { get; set; }
    }
}
