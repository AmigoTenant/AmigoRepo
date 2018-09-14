using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{

    public class LocationCoordinateDTO
    {
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public string LocationCode { get; set; }
    }

}
