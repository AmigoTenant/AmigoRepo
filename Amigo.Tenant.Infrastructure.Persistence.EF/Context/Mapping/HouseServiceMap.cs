using Amigo.Tenant.CommandModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class HouseServiceMap : EntityTypeConfiguration<HouseService>
    {
        public HouseServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.HouseServiceId);

            this.ToTable("HouseService");

            // Relationships
            this.HasMany<HouseServicePeriod>(e => e.HouseServicePeriods);

            //this.HasRequired(t => t.House)
            //    .WithMany(t => t.HouseServices)
            //    .HasForeignKey(d => d.HouseServiceId);

            this.HasRequired(t => t.ServiceHouse);
        }
    }
}
