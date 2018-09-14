using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.MasterData.Houses
{
    public class UpdateHouseFeatureCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int HouseFeatureId { set; get; }
        public int HouseId { set; get; }
        public int FeatureId { set; get; }
        public int HouseFeatureStatusId { set; get; }
        public bool IsRentable { set; get; }
        public bool RowStatus { set; get; }
        public string AdditionalAddressInfo { set; get; }
        public decimal RentPrice { set; get; }

        public int CreatedBy { set; get; }
        public DateTime CreationDate { set; get; }
        public string UserName { set; get; }
    }
}
