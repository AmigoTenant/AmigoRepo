﻿using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amigo.Tenant.Commands.PaymentPeriod
{
    public class PaymentPeriodDeleteMassCommand : IAsyncRequest<CommandResult>
    {
        public int PeriodId { get; set; }
        public int ContractId { get; set; }
    }
}
