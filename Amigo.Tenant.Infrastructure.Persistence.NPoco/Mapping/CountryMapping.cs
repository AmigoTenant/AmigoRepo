using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class CountryMapping : Map<CountryDTO>
    {
        public CountryMapping()
        {
            TableName("vwCountry");

            Columns(x =>
            {
            });
        }
    }
}
