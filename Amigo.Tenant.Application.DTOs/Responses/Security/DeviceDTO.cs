using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class DeviceDTO
    {
       
        public int DeviceId { get; set; }
        public string Identifier { get; set; }
        public string WIFIMAC { get; set; }

        public string CellphoneNumber { get; set; }

        public int? OSVersionId { get; set; }
        public string OSVersion { get; set; }
        public string OSVersionName { get; set; }


        public int? PlatformId { get; set; }
        public string PlatformName { get; set; }


        public int? AppVersionId { get; set; }
        public string AppVersion { get; set; }
        public string AppVersionName { get; set; }





        public int? ModelId { get; set; }
        public string ModelName { get; set; }
        public int? BrandId { get; set; }
        public string BrandName { get; set; }



        public bool? IsAutoDateTime { get; set; }
        public bool? IsSpoofingGPS { get; set; }
        public bool? IsRootedJailbreaked { get; set; }


        public int? AssignedAmigoTenantTUserId { get; set; }
        public string AssignedAmigoTenantTUserUsername { get; set; }


        public bool? RowStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
