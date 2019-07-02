
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
    public class PaymentPeriodUpdateCommandHandler : IAsyncCommandHandler<PaymentPeriodUpdateCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PaymentPeriod> _repositoryPayment;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;


        public PaymentPeriodUpdateCommandHandler(
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
            _repositoryEntityStatus = repositoryEntityStatus;
        }


        public async Task<CommandResult> Handle(PaymentPeriodUpdateCommand command)
        {
            try
            {
                var index = await CreatePaymentPeriod(command);
                
                await _unitOfWork.CommitAsync();

                if (index != 0)
                {
                    command.PaymentPeriodId = index;
                }

                var entityToSave = new PaymentPeriod();
                entityToSave.PaymentPeriodId = index;
                return entityToSave.ToRegisterdResult().WithId(index);

                            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private async Task<int> CreatePaymentPeriod(PaymentPeriodUpdateCommand command)
        {
            var entityToSave = new PaymentPeriod();
            var paymentPeriodPending = await _repositoryEntityStatus.FirstOrDefaultAsync(q => q.EntityCode == Constants.EntityCode.PaymentPeriod && q.Code == Constants.EntityStatus.PaymentPeriod.Pending);

            entityToSave = await _repositoryPayment.FirstOrDefaultAsync(q => q.PaymentPeriodId == command.PaymentPeriodId);
            if (entityToSave != null)
            {
                entityToSave.PaymentAmount = command.PaymentAmount;
                entityToSave.ReferenceNo = command.ReferenceNo;
                entityToSave.Comment = command.Comment;
                entityToSave.Update(command.UserId.Value);
                _repositoryPayment.UpdatePartial(entityToSave, new string[] {
                            "PaymentPeriodId", "PaymentAmount", "ReferenceNo", "Comment", "UpdatedBy", "UpdatedDate"});

            }
            return entityToSave.PaymentPeriodId.Value;
        }
            
    }
}

