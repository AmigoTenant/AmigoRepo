﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.DTOs.Responses.Houses
{
    public class HouseBasicDTO : IEntity
    {
        public int HouseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
