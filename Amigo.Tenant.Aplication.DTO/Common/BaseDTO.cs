using System;
using Amigo.Tenant.Application.DTOs.Requests.Common;

namespace Amigo.Tenant.Application.DTOs.Common
{
    public class BaseDTO: BaseStatusRequest
    {
    
            public int? CreatedBy { get; set; }
            public DateTime? CreationDate { get; set; }
            public int? UpdatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public bool? RowStatus { get; set; }
		
    }
}
