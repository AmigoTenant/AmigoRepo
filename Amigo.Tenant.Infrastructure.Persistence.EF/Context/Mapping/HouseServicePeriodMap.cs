using Amigo.Tenant.CommandModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class HouseServicePeriodMap : EntityTypeConfiguration<HouseServicePeriod>
    {
        public HouseServicePeriodMap()
        {
            this.HasKey(t => t.HouseServicePeriodId);

            this.HasOptional(t => t.ConsumptionUnm).WithMany().HasForeignKey(t => t.ConsumptionUnmId);

            this.ToTable("HouseServicePeriod");

        }
    }
}
