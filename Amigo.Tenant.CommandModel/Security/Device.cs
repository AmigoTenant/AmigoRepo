using System;
using Amigo.Tenant.CommandModel.Abstract;
using Amigo.Tenant.CommandModel.Models;

namespace Amigo.Tenant.CommandModel.Security
{
    public class Device : EntityBase
    {
        public int DeviceId { get; set; }
        public string Identifier { get; set; }
        public string WIFIMAC { get; set; }
        public string CellphoneNumber { get; set; }
        public int? OSVersionId { get; set; }
        public int? ModelId { get; set; }
        public bool? IsAutoDateTime { get; set; }
        public bool? IsSpoofingGPS { get; set; }
        public bool? IsRootedJailbreaked { get; set; }
        public int? AppVersionId { get; set; }
        public int? AssignedAmigoTenantTUserId { get; set; }
        public bool? RowStatus { get; set; }
        public virtual AppVersion AppVersion { get; set; }
        public virtual Model Model { get; set; }
        public virtual OSVersion OSVersion { get; set; }
        public virtual AmigoTenantTUser AmigoTenantTUser { get; set; }
    }
}