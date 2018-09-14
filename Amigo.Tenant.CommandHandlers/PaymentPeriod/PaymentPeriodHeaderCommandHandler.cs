
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
    public class PaymentPeriodHeaderCommandHandler : IAsyncCommandHandler<PaymentPeriodHeaderCommand>
    {

        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<PaymentPeriod> _repositoryPayment;
        private readonly IRepository<Invoice> _repositoryInvoice;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;


        public PaymentPeriodHeaderCommandHandler(
         IBus bus,
         IMapper mapper,
         IUnitOfWork unitOfWork,
         IRepository<PaymentPeriod> repositoryPayment,
         IRepository<Invoice> repositoryInvoice,
         IRepository<EntityStatus> repositoryEntityStatus)
        {
            _bus = bus;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repositoryPayment = repositoryPayment;
            _repositoryInvoice = repositoryInvoice;
            _repositoryEntityStatus = repositoryEntityStatus;
        }


        public async Task<CommandResult> Handle(PaymentPeriodHeaderCommand message)
        {
            try
            {
                var paymentPeriodPayed = await _repositoryEntityStatus.FirstOrDefaultAsync(q => q.EntityCode == Constants.EntityCode.PaymentPeriod && q.Code == Constants.EntityStatus.PaymentPeriod.Payed);
                //var entityToSave = new PaymentPeriod();
                int c = 0;
                var index = 0;
                if (message.PPDetail.Any(q => q.IsSelected.Value))
                {

                    var invoicePayed = await _repositoryEntityStatus.FirstOrDefaultAsync(q => q.EntityCode == Constants.EntityCode.Invoice && q.Code == Constants.EntityStatus.Invoice.Payed);

                    List<OrderExpression<Invoice>> orderExpressionList = new List<OrderExpression<Invoice>>();
                    orderExpressionList.Add(new OrderExpression<Invoice>(OrderType.Desc, p => p.InvoiceNo));
                    Expression<Func<Invoice, bool>> queryFilter = p => p.RowStatus.Value;

                    var maxInvoice = await _repositoryInvoice.FirstOrDefaultAsync(queryFilter, orderExpressionList.ToArray());

                    var firstDetail = message.PPDetail.First();
                    var invoiceEntity = new Invoice();
                    invoiceEntity.InvoiceId = -1;
                    invoiceEntity.ContractId = firstDetail.ContractId;
                    invoiceEntity.InvoiceDate = DateTime.Now;
                    invoiceEntity.Comment = message.Comment;
                    invoiceEntity.PaymentTypeId = firstDetail.PaymentTypeId;
                    invoiceEntity.PaymentOperationNo = message.ReferenceNo;
                    invoiceEntity.CustomerName = message.TenantFullName;
                    invoiceEntity.InvoiceNo = maxInvoice != null ? (int.Parse(maxInvoice.InvoiceNo) + 1).ToString() : "1";
                    invoiceEntity.InvoiceStatusId = invoicePayed != null ? (int?)invoicePayed.EntityStatusId : null; //INPAYED
                    invoiceEntity.TotalAmount = message.TotalAmount;
                    invoiceEntity.TotalDeposit = message.TotalDeposit;
                    invoiceEntity.TotalFine = message.TotalFine;
                    invoiceEntity.TotalLateFee = message.TotalLateFee;
                    invoiceEntity.TotalOnAcount = message.TotalOnAcount;
                    invoiceEntity.TotalRent = message.TotalRent;
                    invoiceEntity.TotalService = message.TotalService;

                    invoiceEntity.RowStatus = true;
                    invoiceEntity.CreationDate = DateTime.Now;
                    invoiceEntity.CreatedBy = message.UserId;
                    invoiceEntity.UpdatedDate = DateTime.Now;
                    invoiceEntity.UpdatedBy = message.UserId;
                    var invoiceDetailsEntity = new List<InvoiceDetail>();
                    
                    foreach (var item in message.PPDetail.Where(q=> q.IsSelected.Value))
                    {
                        --c;
                        var invoiceDetailEntity = new InvoiceDetail();
                        invoiceDetailEntity.ConceptId = item.ConceptId;
                        invoiceDetailEntity.Qty = 1;
                        invoiceDetailEntity.TotalAmount = item.PaymentAmount;
                        invoiceDetailEntity.UnitPrice = item.PaymentAmount;
                        invoiceDetailEntity.InvoiceDetailId = c;
                        invoiceDetailEntity.InvoiceId = -1;
                        invoiceDetailEntity.TotalAmount = item.PaymentAmount;
                        invoiceDetailEntity.RowStatus = true;
                        invoiceDetailEntity.CreationDate = DateTime.Now;
                        invoiceDetailEntity.CreatedBy = message.UserId;
                        invoiceDetailEntity.UpdatedDate = DateTime.Now;
                        invoiceDetailEntity.UpdatedBy = message.UserId;
                        index = await CreatePaymentPeriod(item, message, c, paymentPeriodPayed);
                        invoiceDetailEntity.PaymentPeriodId = index;
                        invoiceDetailsEntity.Add(invoiceDetailEntity);
                        invoiceEntity.InvoiceDetails = invoiceDetailsEntity;
                    }

                    _repositoryInvoice.Add(invoiceEntity);

                }
                
                
                foreach (var item in message.PPDetail.Where(q=> !q.IsSelected.Value && q.TableStatus == Application.DTOs.Requests.Common.ObjectStatus.Modified))
                {
                    --c;
                    index = await CreatePaymentPeriod(item, message, c, null);
                }
                


                await _unitOfWork.CommitAsync();

                if (index != 0)
                {
                    message.PaymentPeriodId = index;
                }

                var entityToSave = new PaymentPeriod();
                entityToSave.PaymentPeriodId = index;
                return entityToSave.ToRegisterdResult().WithId(index);


                //return null; // entity.ToRegisterdResult().WithId(entity.ContractId);
            }
            catch (Exception ex)
            {
                //await SendLogToAmigoTenantTEventLog(message, ex.Message);

                throw;
            }

        }

        private async Task<int> CreatePaymentPeriod(PaymentPeriodDetailCommand item, PaymentPeriodHeaderCommand message, int c, EntityStatus paymentPeriodPayed)
        {
            var entityToSave = new PaymentPeriod();

            if (item.TableStatus == Application.DTOs.Requests.Common.ObjectStatus.Added)
            {
                entityToSave = new PaymentPeriod();
                entityToSave.PaymentPeriodId = c;
                entityToSave.ConceptId = item.ConceptId;
                entityToSave.ContractId = item.ContractId;
                entityToSave.TenantId = item.TenantId;
                entityToSave.PeriodId = item.PeriodId;
                entityToSave.PaymentAmount = item.PaymentAmount;
                entityToSave.DueDate = item.DueDate;
                entityToSave.RowStatus = true;
                entityToSave.Creation(message.UserId);
                if (paymentPeriodPayed != null)
                {
                    entityToSave.PaymentPeriodStatusId = paymentPeriodPayed.EntityStatusId;
                    entityToSave.PaymentDate = DateTime.Now;
                }
                entityToSave.PaymentTypeId = item.PaymentTypeId;
                entityToSave.Comment = !string.IsNullOrEmpty(item.Comment) ? item.Comment : message.Comment;
                entityToSave.ReferenceNo = message.ReferenceNo;
                entityToSave.PaymentDate = DateTime.Now; ;
                entityToSave.Update(message.UserId);
                _repositoryPayment.Add(entityToSave);
                return c;
            }
            else 
            {
                entityToSave = new PaymentPeriod();
                entityToSave = await _repositoryPayment.FirstOrDefaultAsync(q => q.PaymentPeriodId == item.PaymentPeriodId);
                if (entityToSave != null)
                {
                    entityToSave.PaymentAmount = item.PaymentAmount;
                    if (paymentPeriodPayed != null)
                    {
                        entityToSave.PaymentPeriodStatusId =paymentPeriodPayed.EntityStatusId;
                        entityToSave.PaymentDate = DateTime.Now;
                    }
                    entityToSave.ReferenceNo = message.ReferenceNo;
                    entityToSave.Comment = message.Comment;
                    entityToSave.Update(message.UserId);
                    _repositoryPayment.UpdatePartial(entityToSave, new string[] {
                            "PaymentPeriodId", "PaymentAmount", "PaymentPeriodStatusId",
                            "PaymentDate", "ReferenceNo", "Comment", "UpdatedBy", "UpdatedDate"});

                }
                return entityToSave.PaymentPeriodId.Value;
            }
        }
    }
}

