using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{
    public class CityDTO
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string CountryISOCode { get; set; }
        public string CountryName { get; set; }

    }
}
