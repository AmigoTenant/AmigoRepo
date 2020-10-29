
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
    public class PaymentPeriodDeleteMassCommandHandler : IAsyncCommandHandler<PaymentPeriodDeleteMassCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PaymentPeriod> _repositoryPayment;


        public PaymentPeriodDeleteMassCommandHandler(
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


        public async Task<CommandResult> Handle(PaymentPeriodDeleteMassCommand command)
        {
            try
            {
                var payments = await _repositoryPayment.ListAsync(q=> q.PeriodId == command.PeriodId && q.ContractId == command.ContractId && q.RowStatus);
                foreach (var payment in payments)
                {
                    payment.RowStatus = false;
                    payment.UpdatedDate = new DateTime();
                    _repositoryPayment.Update(payment);
                }

                await _unitOfWork.CommitAsync();

                return payments.First().ToRegisterdResult().WithId(payments.First().PaymentPeriodId.Value);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
            
    }
}

