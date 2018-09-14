using System;
using MediatR;
using Amigo.Tenant.Events.Common;

namespace Amigo.Tenant.Events.Tracking
{
    public class RegisterMoveEvent:MobileOperationEvent, IAsyncNotification
    {

        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? RowStatus { get; set; }
        public string Parameters { get; set; }
        public string Username { get; set; }
        public string EquipmentNumber { get; set; }
        public int AmigoTenantTServiceId { get; set; }

        public string ChargeNo { get; set; }
        public string LogType { get; set; }

    }
}
