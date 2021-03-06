﻿using System;
using System.Collections.Generic;
using Amigo.Tenant.Application.DTOs.Common;

namespace Amigo.Tenant.Application.DTOs.Responses.Security
{
    public class AmigoTenantTUserStatusDTO 
    {
        public int AmigoTenantTUserId { get; set; }
        public string UserName { get; set; }
        public int RowStatus { get; set; }
        private int ModifiedBy { get; set; }
        private DateTime ModifiedDate { get; set; }
    }
}
