using System.Data.Entity.ModelConfiguration;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.Infrastructure.Persistence.EF.Context.Mapping
{
    public class RentalApplicationMap : EntityTypeConfiguration<RentalApplication>
    {
        public RentalApplicationMap()
        {
            // Primary Key
            this.HasKey(t => t.RentalApplicationId);
            // Table & Column Mappings
            this.ToTable("RentalApplication");
            this.Property(t=> t.RentalApplicationId).HasColumnName("RentalApplicationId");
            this.Property(t=> t.PeriodId           ).HasColumnName("PeriodId");
            this.Property(t=> t.PropertyTypeId     ).HasColumnName("PropertyTypeId");
            this.Property(t=> t.ApplicationDate    ).HasColumnName("ApplicationDate");
            this.Property(t=> t.FullName           ).HasColumnName("FullName");
            this.Property(t=> t.Email              ).HasColumnName("Email");
            this.Property(t=> t.HousePhone         ).HasColumnName("HousePhone");
            this.Property(t=> t.CellPhone          ).HasColumnName("CellPhone");
            this.Property(t=> t.CheckIn            ).HasColumnName("CheckIn");
            this.Property(t=> t.CheckOut           ).HasColumnName("CheckOut");
            this.Property(t=> t.ResidenseCountryId ).HasColumnName("ResidenseCountryId");
            this.Property(t=> t.BudgetId           ).HasColumnName("BudgetId");
            this.Property(t=> t.Comment            ).HasColumnName("Comment");
            this.Property(t=> t.RowStatus          ).HasColumnName("RowStatus");
            this.Property(t=> t.CreatedBy          ).HasColumnName("CreatedBy");
            this.Property(t=> t.CreationDate       ).HasColumnName("CreationDate");
            this.Property(t => t.UpdatedBy         ).HasColumnName("UpdatedBy");
            this.Property(t => t.CityOfInterestId).HasColumnName("CityOfInterestId");
            this.Property(t => t.HousePartId).HasColumnName("HousePartId");
            this.Property(t => t.PersonNo).HasColumnName("PersonNo");
            this.Property(t => t.OutInDownId).HasColumnName("OutInDownId");
            this.Property(t => t.ReferredById).HasColumnName("ReferredById");
            this.Property(t => t.ReferredByOther).HasColumnName("ReferredByOther");
            this.Property(t => t.PriorityId).HasColumnName("PriorityId");
            this.Property(t => t.AlertDate).HasColumnName("AlertDate");
            this.Property(t => t.AlertMessage).HasColumnName("AlertMessage");

    }
}
}
