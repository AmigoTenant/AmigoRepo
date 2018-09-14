using System;
using MediatR;
using Amigo.Tenant.Commands.Common;
using System.Data.Spatial;

namespace Amigo.Tenant.Commands.Tracking.AmigoTenanttEventLog
{
    public class RegisterAmigoTenanttEventLogCommand : IAsyncRequest<CommandResult>
    {

       // public int AmigoTenantTEventLogId { get; set; }
        public int? ActivityTypeId { get; set; }
        public string Username { get; set; }
        public DateTimeOffset? ReportedActivityDate { get; set; }
        public string ReportedActivityTimeZone { get; set; }
        public DateTime? ConvertedActivityUTC { get; set; }
        public string LogType { get; set; }
        public string Parameters { get; set; }
        public int? AmigoTenantMoveId { get; set; }
        public string AmigoTenantMoveNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public int? EquipmentId { get; set; }
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
        public string ChargeNo { get; set; }
        public string Request { get; set; }
        public bool? IncludeRequestLog { get; set; }
        public int UserId { get; set; }
    }
}
