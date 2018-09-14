using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Services;
using NPoco.FluentMappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.NPoco.Mapping
{
    public class RentalApplicationRegisterMapping : Map<RentalApplicationRegisterRequest>
    {
        public RentalApplicationRegisterMapping()
        {
            TableName("RentalApplication");
            Columns(x =>
            {
                //x.Column(y => y.Features).Ignore();
                //x.Column(y => y.Cities).Ignore();
            });
            
        }
    }
}
