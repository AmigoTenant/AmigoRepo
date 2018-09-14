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
    public class RentalApplicationMapping : Map<RentalApplicationSearchDTO>
    {
        public RentalApplicationMapping()
        {
            TableName("vwRentalApplicationSearch");
            //Columns(x =>
            //{
            //    x.Column(y => y.FeaturesCode).Ignore();
            //    x.Column(y => y.Cities).Ignore();
            //});
            
        }
    }
}
