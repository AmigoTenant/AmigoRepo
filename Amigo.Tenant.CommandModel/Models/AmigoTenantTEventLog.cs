using System;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Models
{
    public class AmigoTenantTEventLog : EntityBase
    {
        public int AmigoTenantTEventLogId { get; set; }
        public int? ActivityTypeId { get; set; }
        public string Username { get; set; }
        public DateTimeOffset? ReportedActivityDate { get; set; }
        public string ReportedActivityTimeZone { get; set; }
        public DateTime? ConvertedActivityUTC { get; set; }
        public string LogType { get; set; }
        public string Parameters { get; set; }
        public int AmigoTenantTServiceId { get; set; }
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
        public bool? RowStatus { get; set; }
        public string ChargeNo { get; set; }
        //public int? CreatedBy { get; set; }
        //public DateTime? CreationDate { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public virtual ActivityType ActivityType { get; set; }
    }
}