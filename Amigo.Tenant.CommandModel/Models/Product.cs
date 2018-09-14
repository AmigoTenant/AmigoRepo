using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Models
{
    public class Product: EntityBase
    {
        public Product()
        {
            AmigoTenantTServices = new List<AmigoTenantTService>();
        }

        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string IsHazardous { get; set; }
        public bool? RowStatus { get; set; }
        public virtual ICollection<AmigoTenantTService> AmigoTenantTServices { get; set; }
    }
}