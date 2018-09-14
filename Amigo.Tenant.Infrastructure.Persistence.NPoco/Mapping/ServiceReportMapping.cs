using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Tracking;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ServiceReportMapping : Map<InternalReportDTO>
    {

        public ServiceReportMapping()
        {
            TableName("vwAmigoTenantTServiceReport");

            Columns(x =>
            {
                x.Column(y => y.AmigoTenantTUserId);
                x.Column(y => y.Username);
                x.Column(y => y.Drivername);
                x.Column(y => y.EquipmentNumber);
                x.Column(y => y.EquipmentSizeCode);
                x.Column(y => y.EquipmentSize);
                x.Column(y => y.EquipmentTypeCode);
                x.Column(y => y.EquipmentType);
                x.Column(y => y.ServiceCode);
                x.Column(y => y.Service);
                x.Column(y => y.Product);

                x.Column(y => y.IsHazardous);
                x.Column(y => y.ChargeType);

                x.Column(y => y.OriginBlockCode);
                x.Column(y => y.OriginBlock);
                x.Column(y => y.DestinationBlockCode);
                x.Column(y => y.DestinationBlock);
                x.Column(y => y.Approver);
                x.Column(y => y.ServiceStatus);
                x.Column(y => y.DispatchingParty);
                x.Column(y => y.ServiceStartDate);
                x.Column(y => y.ServiceFinishDate);
                x.Column(y => y.CustomerBill);
                x.Column(y => y.DriverPay);

                x.Column(y => y.Status).Ignore();
                x.Column(y => y.IsHazardousLabel).Ignore();
                x.Column(y => y.ChargeNo);
                x.Column(y => y.ApprovalStatus).Ignore();
                x.Column(y => y.EquipmentStatusName);
                x.Column(y => y.DriverComments);
                x.Column(y => y.ServiceStartDayName).Ignore();
                x.Column(y => y.ServiceFinishDayName).Ignore();
                x.Column(y => y.ServiceTotalHours).Ignore();
                x.Column(y => y.EquipmentStatusId);
                x.Column(y => y.ChassisNo);
                x.Column(y => y.ProductId);
            });

        }
    }
}
