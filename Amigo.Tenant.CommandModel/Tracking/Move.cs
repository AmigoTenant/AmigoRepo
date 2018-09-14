using System;
using Amigo.Tenant.CommandModel.Abstract;

namespace Amigo.Tenant.CommandModel.Tracking
{
    public class Move : EntityBase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int DriverId { get; set; }
        public int CostId { get; set; }
        public DateTimeOffset RegisteredDate { get; set; }
    }
}