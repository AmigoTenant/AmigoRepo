using Amigo.Tenant.CommandModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ServiceHousePeriodMap : EntityTypeConfiguration<ServiceHousePeriod>
    {
        public ServiceHousePeriodMap()
        {
            this.HasKey(t => t.ServiceHousePeriodId);

            this.ToTable("ServiceHousePeriod");


        }
    }
}
