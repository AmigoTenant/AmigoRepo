using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;


namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class AmigoTenantTServiceMap : EntityTypeConfiguration<AmigoTenantTService>
    {
        public AmigoTenantTServiceMap()
        {
            // Primary Key
            this.HasKey(t => t.AmigoTenantTServiceId);

            // Properties
            this.Property(t => t.ServiceOrderNo);

            this.Property(t => t.ServiceStartDateTZ)
                .HasMaxLength(20);

            this.Property(t => t.ServiceFinishDateTZ)
                .HasMaxLength(20);

            this.Property(t => t.EquipmentNumber)
                .HasMaxLength(20);

            this.Property(t => t.ChassisNumber)
                .HasMaxLength(20);

            this.Property(t => t.ChargeType)
                .HasMaxLength(1);

            this.Property(t => t.AuthorizationCode)
                .HasMaxLength(20);

            this.Property(t => t.AcknowledgeBy)
                .HasMaxLength(50);

            this.Property(t => t.ServiceAcknowledgeDateTZ)
                .HasMaxLength(20);

            this.Property(t => t.ApprovedBy)
                .HasMaxLength(50);

            this.Property(t => t.ApprovalModified)
                .HasMaxLength(1);

            this.Property(t => t.ProductDescription)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("AmigoTenantTService");
            this.Property(t => t.AmigoTenantTServiceId).HasColumnName("AmigoTenantTServiceId");
            this.Property(t => t.ServiceOrderNo).HasColumnName("ServiceOrderNo");
            this.Property(t => t.ServiceStartDate).HasColumnName("ServiceStartDate");
            this.Property(t => t.ServiceStartDateTZ).HasColumnName("ServiceStartDateTZ");
            this.Property(t => t.ServiceFinishDate).HasColumnName("ServiceFinishDate");
            this.Property(t => t.ServiceFinishDateTZ).HasColumnName("ServiceFinishDateTZ");
            this.Property(t => t.ServiceStartDateUTC).HasColumnName("ServiceStartDateUTC");
            this.Property(t => t.ServiceFinishDateUTC).HasColumnName("ServiceFinishDateUTC");
            this.Property(t => t.EquipmentNumber).HasColumnName("EquipmentNumber");
            this.Property(t => t.EquipmentTestDate25Year).HasColumnName("EquipmentTestDate25Year");
            this.Property(t => t.EquipmentTestDate5Year).HasColumnName("EquipmentTestDate5Year");
            this.Property(t => t.ChassisNumber).HasColumnName("ChassisNumber");
            this.Property(t => t.ChargeType).HasColumnName("ChargeType");
            this.Property(t => t.AuthorizationCode).HasColumnName("AuthorizationCode");
            this.Property(t => t.HasH34).HasColumnName("HasH34");
            this.Property(t => t.DetentionInMinutesReal).HasColumnName("DetentionInMinutesReal");
            this.Property(t => t.DetentionInMinutesRounded).HasColumnName("DetentionInMinutesRounded");
            this.Property(t => t.AcknowledgeBy).HasColumnName("AcknowledgeBy");
            this.Property(t => t.ServiceAcknowledgeDate).HasColumnName("ServiceAcknowledgeDate");
            this.Property(t => t.ServiceAcknowledgeDateTZ).HasColumnName("ServiceAcknowledgeDateTZ");
            this.Property(t => t.ServiceAcknowledgeDateUTC).HasColumnName("ServiceAcknowledgeDateUTC");
            this.Property(t => t.IsAknowledged).HasColumnName("IsAknowledged");
            this.Property(t => t.ApprovedBy).HasColumnName("ApprovedBy");
            this.Property(t => t.ApprovalDate).HasColumnName("ApprovalDate");
            this.Property(t => t.ServiceStatus).HasColumnName("ServiceStatus");
            this.Property(t => t.ApprovalModified).HasColumnName("ApprovalModified");
            this.Property(t => t.OriginLocationId).HasColumnName("OriginLocationId");
            this.Property(t => t.DestinationLocationId).HasColumnName("DestinationLocationId");
            this.Property(t => t.DispatchingPartyId).HasColumnName("DispatchingPartyId");
            this.Property(t => t.EquipmentSizeId).HasColumnName("EquipmentSizeId");
            this.Property(t => t.EquipmentTypeId).HasColumnName("EquipmentTypeId");
            this.Property(t => t.EquipmentStatusId).HasColumnName("EquipmentStatusId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ProductDescription).HasColumnName("ProductDescription");
            this.Property(t => t.ServiceId).HasColumnName("ServiceId");
            this.Property(t => t.AmigoTenantTUserId).HasColumnName("AmigoTenantTUserId");
            this.Property(t => t.RowStatus).HasColumnName("RowStatus");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy).HasColumnName("UpdatedBy");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t=> t.PayBy).HasColumnName("PayBy");



            // Relationships
            this.HasOptional(t => t.DispatchingParty)
                .WithMany(t => t.AmigoTenantTServices)
                .HasForeignKey(d => d.DispatchingPartyId);
            this.HasOptional(t => t.EquipmentSize)
                .WithMany(t => t.AmigoTenantTServices)
                .HasForeignKey(d => d.EquipmentSizeId);
            this.HasOptional(t => t.EquipmentStatu)
                .WithMany(t => t.AmigoTenantTServices)
                .HasForeignKey(d => d.EquipmentStatusId);
            this.HasOptional(t => t.EquipmentType)
                .WithMany(t => t.AmigoTenantTServices)
                .HasForeignKey(d => d.EquipmentTypeId);
            this.HasOptional(t => t.Location)
                .WithMany(t => t.AmigoTenantTServices)
                .HasForeignKey(d => d.DestinationLocationId);
            this.HasOptional(t => t.Location1)
                .WithMany(t => t.AmigoTenantTServices1)
                .HasForeignKey(d => d.OriginLocationId);
            this.HasOptional(t => t.Product)
                .WithMany(t => t.AmigoTenantTServices)
                .HasForeignKey(d => d.ProductId);
            this.HasOptional(t => t.Service)
                .WithMany(t => t.AmigoTenantTServices)
                .HasForeignKey(d => d.ServiceId);
            this.HasOptional(t => t.AmigoTenantTUser)
                .WithMany(t => t.AmigoTenantTServices)
                .HasForeignKey(d => d.AmigoTenantTUserId);

        }
    }
}
