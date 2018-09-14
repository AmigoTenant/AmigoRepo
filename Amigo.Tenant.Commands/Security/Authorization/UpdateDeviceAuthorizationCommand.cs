using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Commands.Security.Authorization
{
    public class UpdateDeviceAuthorizationCommand : IAsyncRequest<CommandResult<AmigoTenantTUser>>
    {
        public int? DeviceId { get; set; }
        public string Identifier { get; set; }
        public int? AssignedAmigoTenantTUserId { get; set; }
        public string AssignedAmigoTenantTUserName { get; set; }
        public int? CreatedBy { get; set; }

        public int ActivityTypeId { get; set; }
        public bool? IsAutoDateTime { get; set; }
        public bool? IsSpoofingGPS { get; set; }
        public bool? IsRootedJailbreaked { get; set; }
        public string Platform { get; set; }
        public string OSVersion { get; set; }
        public string AppVersion { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? Accuracy { get; set; }
        public string LocationProvider { get; set; }
        public DateTimeOffset? ReportedActivityDate { get; set; }
        public string ReportedActivityTimeZone { get; set; }
        public string Request { get; set; }
        public bool? IncludeRequestLog { get; set; }

        public string LogType { get; set; }
        public string Parameters { get; set; }
        
    }
}
