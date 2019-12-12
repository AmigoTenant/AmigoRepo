using Amigo.Tenant.Application.DTOs.Requests.Common;
using Amigo.Tenant.Commands.Common;
using MediatR;
using System;
using System.Collections.Generic;

namespace Amigo.Tenant.Commands.PaymentPeriod
{
    public class PaymentPeriodDeleteCommand : IAsyncRequest<CommandResult>
    {
        public int PaymentPeriodId { get; set; }

    }
}
