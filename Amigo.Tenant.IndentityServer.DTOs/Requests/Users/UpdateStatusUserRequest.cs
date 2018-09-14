using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.IdentityServer.DTOs.Requests.Users
{
    public class UpdateStatusUserRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool? RowStatus { get; set; }
    }
}
