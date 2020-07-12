using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.MasterData
{
    public class RentalApplicationFeatureRequest : BaseStatusRequest
    {
        public int? RentalApplicationFeatureId { get; set; }
        public int? RentalApplicationId { get; set; }
        public int? FeatureId { get; set; }

    }
}
