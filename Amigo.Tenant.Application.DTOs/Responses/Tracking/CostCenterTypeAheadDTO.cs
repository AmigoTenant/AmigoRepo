﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Tracking
{

    public class CostCenterTypeAheadDTO : IEntity
    {
        public int CostCenterIdId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
