using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class HouseFeatureRequest : AuditBaseRequest
    {
        public int HouseFeatureId {set; get; }
        public int HouseId {set; get; }
        public int FeatureId {set; get; }
        public int HouseFeatureStatusId {set; get; }
        public bool IsRentable {set; get; }
        public bool RowStatus {set; get; }
        public string AdditionalAddressInfo { set; get; }
        public decimal RentPrice {set; get; }
        
        public int CreatedBy {set; get; }
        public DateTime CreationDate {set; get; }
        public string UserName {set; get; }
    }
}
