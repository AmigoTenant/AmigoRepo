using System;
using System.Collections.Generic;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Models
{
    public class AmigoTenantTRole: EntityBase
    {
        public AmigoTenantTRole()
        {
            Permissions = new List<Permission>();
            AmigoTenantTUsers = new List<AmigoTenantTUser>();
        }

        public int AmigoTenantTRoleId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool? RowStatus { get; set; }
        //public int? CreatedBy { get; set; }
        //public DateTime? CreationDate { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedDate { get; set; }
        public virtual ICollection<Permission> Permissions { get; set; }
        public virtual ICollection<AmigoTenantTUser> AmigoTenantTUsers { get; set; }
    }
}