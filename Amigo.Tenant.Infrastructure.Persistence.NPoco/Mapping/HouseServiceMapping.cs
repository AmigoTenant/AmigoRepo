using Amigo.Tenant.Application.DTOs.Responses.Houses;
using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class HouseServiceMapping : Map<HouseServiceDTO>
    {
        public HouseServiceMapping()
        {
            TableName("vwHouseService");
            Columns(p =>
            {
                p.Column(q => q.Checked).Ignore();
                p.Column(q => q.HouseServicePeriods).Ignore();
            });
        }
    }
}
