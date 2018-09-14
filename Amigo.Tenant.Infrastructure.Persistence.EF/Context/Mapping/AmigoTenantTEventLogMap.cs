using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class AmigoTenantTEventLogMap : EntityTypeConfiguration<AmigoTenantTEventLog>
    {
        public AmigoTenantTEventLogMap()
        {
            // Primary Key
            this.HasKey(t => t.AmigoTenantTEventLogId);

            // Properties
            this.Property(t => t.Username)
                .HasMaxLength(64);

            this.Property(t => t.ReportedActivityTimeZone)
                .HasMaxLength(20);

            this.Property(t => t.LogType)
                .HasMaxLength(20);

            this.Property(t => t.Parameters)
                .HasMaxLength(4096);

            //this.Property(t => t.AmigoTenantMoveNumber)
            //    .HasMaxLength(20);

            this.Property(t => t.EquipmentNumber)
                .HasMaxLength(20);

            this.Property(t => t.Platform)
                .HasMaxLength(50);

            this.Property(t => t.OSVersion)
                .HasMaxLength(50);

            this.Property(t => t.AppVersion)
                .HasMaxLength(20);

            this.Property(t => t.LocationProvider)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("AmigoTenantTEventLog");
            this.Property(t => t.AmigoTenantTEventLogId).HasColumnName("AmigoTenantTEventLogId");
            this.Property(t => t.ActivityTypeId).HasColumnName("ActivityTypeId");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.ReportedActivityDate).HasColumnName("ReportedActivityDate");
            this.Property(t => t.ReportedActivityTimeZone).HasColumnName("ReportedActivityTimeZone");
            this.Property(t => t.ConvertedActivityUTC).HasColumnName("ConvertedActivityUTC");
            this.Property(t => t.LogType).HasColumnName("LogType");
            this.Property(t => t.Parameters).HasColumnName("Parameters");
            this.Property(t => t.AmigoTenantTServiceId).HasColumnName("AmigoTenantTServiceId");
            this.Property(t => t.EquipmentNumber).HasColumnName("EquipmentNumber");
            this.Property(t => t.EquipmentId).HasColumnName("EquipmentId");
            this.Property(t => t.IsAutoDateTime).HasColumnName("IsAutoDateTime");
            this.Property(t => t.IsSpoofingGPS).HasColumnName("IsSpoofingGPS");
            this.Property(t => t.IsRootedJailbreaked).HasColumnName("IsRootedJailbreaked");
            this.Property(t => t.Platform).HasColumnName("Platform");
            this.Property(t => t.OSVersion).HasColumnName("OSVersion");
            this.Property(t => t.AppVersion).HasColumnName("AppVersion");
            this.Property(t => t.Latitude).HasColumnName("Latitude").HasPrecision(16, 9); 
            this.Property(t => t.Longitude).HasColumnName("Longitude").HasPrecision(16, 9); 
            this.Property(t => t.Accuracy).HasColumnName("Accuracy");
            this.Property(t => t.LocationProvider).HasColumnName("LocationProvider");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.ActivityType)
                .WithMany(t => t.AmigoTenantTEventLogs)
                .HasForeignKey(d => d.ActivityTypeId);

        }
    }
}
