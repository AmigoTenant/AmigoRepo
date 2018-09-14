using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class LocationWithCoordinatesDTO: LocationDTO
    {
        public List<LocationCoordinateDTO> Coordinates {get; set;}
    }
}
