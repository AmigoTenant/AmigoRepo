using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Requests.Tracking
{
    public class RegisterLocationCoordinatesRequest
    {
        public List<RegisterLocationCoordinateItem> RegisterLocationCoordinateList { get; set; }
    }


    public class RegisterLocationCoordinateItem
    {
        public string LocationCode { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
    }
}





