﻿using Amigo.Tenant.Application.DTOs.Requests.Common;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Application.DTOs.Requests.Leasing
{
    public class OtherTenantRegisterRequest: BaseStatusRequest
    {

        public int? OtherTenantId { get; set; }

        public int? ContractId { get; set; }

        public int? TenantId { get; set; }

        public bool RowStatus { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string FullName { get; set; }

    }
}
