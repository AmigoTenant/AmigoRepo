using System;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class Last24HoursDTO
    {
        public int AmigoTenantTUserId
        {
            get; set;
        }
        public string Username
        {
            get; set;
        }
        public string ReportedActivityTimeZone
        {
            get; set;
        }
        public DateTimeOffset? ReportedActivityDate
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
        public string ChargeType
        {
            get; set;
        }
        public string ActivityTypeName
        {
            get; set;
        }
        public string ActivityTypeCode
        {
            get; set;
        }
        public string TractorNumber
        {
            get; set;
        }
        public string FirstName
        {
            get; set;
        }
        public string LastName
        {
            get; set;
        }
        public string ChassisNumber
        {
            get; set;
        }
        public string EquipmentNumber
        {
            get; set;
        }

        public int Index
        {
            get; set;
        }
        public string ChargeNo
        {
            get; set;
        }

        public string Product
        {
            get; set;
        }
        public string Origin
        {
            get; set;
        }
        public string Destination
        {
            get; set;
        }
        public string ServiceName
        {
            get; set;
        }
        public string EquipmentTypeName
        {
            get; set;
        }

    }
}
