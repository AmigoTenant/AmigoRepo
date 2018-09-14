using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class EquipmentMap : EntityTypeConfiguration<Equipment>
    {
        public EquipmentMap()
        {
            // Primary Key
            this.HasKey(t => t.EquipmentId);

            // Properties
            this.Property(t => t.EquipmentNo)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Equipment");
            this.Property(t => t.EquipmentId).HasColumnName("EquipmentId");
            this.Property(t => t.EquipmentNo).HasColumnName("EquipmentNo");
            //this.Property(t => t.EquipmentTypeId).HasColumnName("EquipmentTypeId");
            this.Property(t => t.TestDate25Year).HasColumnName("TestDate25Year");
            this.Property(t => t.TestDate5Year).HasColumnName("TestDate5Year");
            this.Property(t => t.EquipmentSizeId).HasColumnName("EquipmentSizeId");
            this.Property(t => t.EquipmentStatusId).HasColumnName("EquipmentStatusId");
            this.Property(t => t.LocationId).HasColumnName("LocationId");
            this.Property(t => t.IsMasterRecord).HasColumnName("IsMasterRecord");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");

            // Relationships
            this.HasOptional(t => t.EquipmentSize)
                .WithMany(t => t.Equipments)
                .HasForeignKey(d => d.EquipmentSizeId);
            this.HasOptional(t => t.EquipmentStatu)
                .WithMany(t => t.Equipments)
                .HasForeignKey(d => d.EquipmentStatusId);
            //this.HasOptional(t => t.EquipmentType)
            //    .WithMany(t => t.Equipments)
            //    .HasForeignKey(d => d.EquipmentTypeId);
            this.HasOptional(t => t.Location)
                .WithMany(t => t.Equipments)
                .HasForeignKey(d => d.LocationId);

        }
    }
}
