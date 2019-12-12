
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.PaymentPeriod;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Amigo.Tenant.CommandHandlers.PaymentPeriods
{
    public class PaymentPeriodDeleteCommandHandler : IAsyncCommandHandler<PaymentPeriodDeleteCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PaymentPeriod> _repositoryPayment;


        public PaymentPeriodDeleteCommandHandler(
         IBus bus,
         IMapper mapper,
         IUnitOfWork unitOfWork,
         IRepository<PaymentPeriod> repositoryPayment,
         IRepository<EntityStatus> repositoryEntityStatus)
        {
            _bus = bus;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repositoryPayment = repositoryPayment;
        }


        public async Task<CommandResult> Handle(PaymentPeriodDeleteCommand command)
        {
            try
            {
                var payment = await _repositoryPayment.FirstOrDefaultAsync(q=> q.PaymentPeriodId == command.PaymentPeriodId);
                if (payment != null) {
                    _repositoryPayment.Delete(payment);
                    await _unitOfWork.CommitAsync();
                }
                return payment.ToRegisterdResult().WithId(payment.PaymentPeriodId.Value);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
            
    }
}

