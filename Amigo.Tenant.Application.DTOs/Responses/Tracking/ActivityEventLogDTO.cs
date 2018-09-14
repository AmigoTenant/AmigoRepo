
using System;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class ActivityEventLogDTO : BaseDTO
    {
        public int AmigoTenantTEventLogId
        {
            get; set;
        }
        public int ActivityTypeId
        {
            get; set;
        }
        public string ActivityName
        {
            get; set;
        }
        public string Username
        {
            get; set;
        }
        public Decimal Latitude
        {
            get; set;
        }
        public Decimal Longitude
        {
            get; set;
        }
        public DateTimeOffset? ReportedActivityDate
        {
            get; set;
        }
        public string LocationProvider
        {
            get; set;
        }
        public int OriginLocationId
        {
            get; set;
        }
        public string OriginLocationName
        {
            get; set;
        }
        public int DestinationLocationId
        {
            get; set;
        }
        public string DestinationLocationName
        {
            get; set;
        }
        public string EquipmentNumber
        {
            get; set;
        }

        public string ProductName
        {
            get; set;
        }

        public string ChargeNo { get; set; }
        public string Parameters { get; set; }
        public string LogType { get; set; }
    }
}
