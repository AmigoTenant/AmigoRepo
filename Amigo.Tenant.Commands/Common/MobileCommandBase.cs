using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.Common
{
    public abstract class MobileCommandBase : IMobileCommand
    {

        public int ActivityTypeId { get; set; }
        public bool? IsAutoDateTime { get; set; }
        public bool? IsSpoofingGPS { get; set; }
        public bool? IsRootedJailbreaked { get; set; }
        public string Platform { get; set; }
        public string OSVersion { get; set; }
        public string AppVersion { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? Accuracy { get; set; }
        public string LocationProvider { get; set; }
        public DateTimeOffset? ReportedActivityDate { get; set; }
        public string ReportedActivityTimeZone { get; set; }
    }
}
