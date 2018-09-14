using Amigo.Tenant.Application.DTOs.Responses.Services;
using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class ServiceHousePeriodMapping : Map<ServiceHousePeriodDTO>
    {
        public ServiceHousePeriodMapping()
        {
            TableName("vwServiceHousePeriod");
        }
    }
}
