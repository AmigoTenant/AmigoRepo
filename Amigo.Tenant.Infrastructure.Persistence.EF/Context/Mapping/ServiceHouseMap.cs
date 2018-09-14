using Amigo.Tenant.CommandModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class ServiceHouseMap : EntityTypeConfiguration<ServiceHouse>
    {
        public ServiceHouseMap()
        {
            // Primary Key
            this.HasKey(t => t.ServiceId);

            this.ToTable("ServiceHouse");

            // Relationships
            this.HasRequired(t => t.Concept);
            //.WithMany(t => t.HouseServices)
            //.HasForeignKey(d => d.ConceptId);

            this.HasRequired(t => t.BusinessPartner);
            //.WithMany(t => t.HouseServices)
            //.HasForeignKey(d => d.ServiceId);

            this.HasRequired(t => t.ServiceType);
            //.WithMany(t => t.HouseServices)
            //.HasForeignKey(d => d.HouseServiceId);

            this.HasMany(t => t.ServiceHousePeriods);
        }
    }
}
