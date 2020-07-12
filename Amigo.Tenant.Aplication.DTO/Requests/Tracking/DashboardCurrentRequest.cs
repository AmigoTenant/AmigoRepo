using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class ReportCurrentRequest : PagedRequest
    {
        public int? AmigoTenantTUserId
        {
            get; set;
        }
        public string ServiceCode
        {
            get; set;
        }
        public string EquipmentNumber
        {
            get; set;
        }
        public string EquipmentSizeCode
        {
            get; set;
        }
        public string EquipmentTypeCode
        {
            get; set;
        }
        public string Approver
        {
            get; set;
        }
        public string ChargeNumber
        {
            get; set;
        }
        public string OriginBlockCode
        {
            get; set;
        }
        public string DestinationBlockCode
        {
            get; set;
        }
        public int? ServiceStatus
        {
            get; set;
        }
        public int? EquipmentStatusId
        {
            get; set;
        }
        public int? ProductId
        {
            get; set;
        }
    }
}
