using Amigo.Tenant.Application.DTOs.Responses.Houses;
using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class HouseMapping : Map<HouseDTO>
    {
        public HouseMapping()
        {
            TableName("vwHouse");

            Columns(x =>
            {

            });
        }
    }
}
