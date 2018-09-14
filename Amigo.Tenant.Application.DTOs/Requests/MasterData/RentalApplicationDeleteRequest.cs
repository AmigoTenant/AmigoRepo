using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class RentalApplicationDeleteRequest : AuditBaseRequest
    {
        public int? RentalApplicationId { get; set; }

    }
}
