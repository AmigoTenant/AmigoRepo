using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class RequestLogMap : EntityTypeConfiguration<RequestLog>
    {
        public RequestLogMap()
        {
            // Primary Key
            HasKey(t => t.RequestLogId);

            // Properties
            Property(t => t.URL).HasMaxLength(200);

            Property(t => t.ServiceName)
                .HasMaxLength(200);

            Property(t => t.Request).HasColumnType("xml");

            Property(t => t.Response).HasColumnType("xml");

            Property(t => t.RequestedBy);

            Property(t => t.RequestDate);
            

            // Table & Column Mappings
            ToTable("RequestLog");
            Property(t => t.RequestLogId).HasColumnName("RequestLogId");
            Property(t => t.URL).HasColumnName("URL");
            Property(t => t.ServiceName).HasColumnName("ServiceName");
            Property(t => t.Request).HasColumnName("Request");
            Property(t => t.Response).HasColumnName("Response");
            Property(t => t.RequestedBy).HasColumnName("RequestedBy");
            Property(t => t.RequestDate).HasColumnName("RequestDate");
            
        }
    }
}
