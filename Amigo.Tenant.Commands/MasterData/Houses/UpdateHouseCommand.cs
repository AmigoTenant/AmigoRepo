using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.MasterData.Houses
{
    public class UpdateHouseCommand : AuditBaseCommand, IAsyncRequest<CommandResult>
    {
        public int HouseId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string ShortName { get; set; }
        public decimal RentPrice { get; set; }
        public decimal RentDeposit { get; set; }
        public int HouseTypeId { get; set; }
        public int HouseStatusId { get; set; }
        public int? CityId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool RowStatus { get; set; }
    }
}
