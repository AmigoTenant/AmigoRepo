using Amigo.Tenant.Application.DTOs.Responses.UtilityBills;
using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class UtilityHouseServicePeriodMapping : Map<UtilityHouseServicePeriodDTO>
    {
        public UtilityHouseServicePeriodMapping()
        {
            TableName("vwUtilityHouseService");
            //Columns(p =>
            //{
            //    p.Column(q => q.HouseService).Ignore();
            //    p.Column(q => q.ServicePeriod).Ignore();
            //    p.Column(q => q.Period).Ignore();
            //});
        }
    }
}
