namespace Amigo.Tenant.CommandModel.Models
{
    using Amigo.Tenant.CommandModel.Abstract;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    //using System.Data.Entity.Spatial;

    [Table("HouseFeature")]
    public partial class HouseFeature : EntityBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HouseFeature()
        {
            ContractHouseDetails = new HashSet<ContractHouseDetail>();
            FeatureAccesories = new HashSet<FeatureAccesory>();
            FeatureImages = new HashSet<FeatureImage>();
        }

        public int HouseFeatureId { get; set; }

        public int HouseId { get; set; }

        public int FeatureId { get; set; }

        //public int? Unit { get; set; }

        public int HouseFeatureStatusId { get; set; }

        public bool IsRentable { get; set; }

        public bool RowStatus { get; set; }

        //public int? CreatedBy { get; set; }

        //[Column(TypeName = "datetime2")]
        //public DateTime? CreationDate { get; set; }

        //public int? UpdatedBy { get; set; }

        //[Column(TypeName = "datetime2")]
        //public DateTime? UpdatedDate { get; set; }

        [StringLength(50)]
        public string AdditionalAddressInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ContractHouseDetail> ContractHouseDetails { get; set; }

        public virtual EntityStatus EntityStatus { get; set; }

        public virtual Feature Feature { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeatureAccesory> FeatureAccesories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeatureImage> FeatureImages { get; set; }

        public virtual House House { get; set; }

        public decimal RentPrice { get; set; }

        public static HouseFeature Create(int featureId, int userId, int statusDraftId)
        {
            return new HouseFeature()
            {
                AdditionalAddressInfo = string.Empty,
                CreatedBy = userId,
                CreationDate = DateTime.Now,
                FeatureId = featureId,
                HouseFeatureId = 0,
                HouseFeatureStatusId = statusDraftId,
                IsRentable = true,
                RentPrice = 0,
                RowStatus = true
            };
        }
    }
}
