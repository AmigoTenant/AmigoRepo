using System;

namespace Amigo.Tenant.CommandModel.Abstract
{
    public interface IAuditable
    {
        int? CreatedBy { get; set; }
        DateTime? CreationDate { get; set; }
        int? UpdatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
    }
}