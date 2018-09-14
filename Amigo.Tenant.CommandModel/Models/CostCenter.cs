using System;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Models
{
    public class CostCenter : EntityBase
    {
        public int CostCenterId
        {
            get; set;
        }
        public string Code
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public bool? RowStatus
        {
            get; set;
        }
    }
}