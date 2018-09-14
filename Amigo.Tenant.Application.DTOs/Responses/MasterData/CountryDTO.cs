using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.MasterData
{
    public class CountryDTO : IEntity
    {
        public int CountryId { get; set; }
        public string ISOCode { get; set; }
        public string Name { get; set; }
        public bool RowStatus { get; set; }    
    }
}
