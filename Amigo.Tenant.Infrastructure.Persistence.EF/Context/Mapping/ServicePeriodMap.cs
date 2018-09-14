using Amigo.Tenant.CommandModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ServicePeriodMap : EntityTypeConfiguration<ServicePeriod>
    {
        public ServicePeriodMap()
        {
            this.ToTable("ServicePeriod");

            // Primary Key
            this.HasKey(t => t.ServicePeriodId);

            //this.HasOptional(p => p.HouseServicePeriod);
                //.WithMany(p => p.ServicePeriods)
                //.HasForeignKey(p => p.HouseServicePeriodId);
        }
    }
}
