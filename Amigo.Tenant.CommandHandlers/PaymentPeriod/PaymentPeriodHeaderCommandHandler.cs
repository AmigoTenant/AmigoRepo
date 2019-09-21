
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
        private readonly IRepository<InvoiceDetail> _repositoryInvoiceDetail;
        private readonly IRepository<EntityStatus> _repositoryEntityStatus;
        private readonly IRepository<Concept> _repositoryConcept;

        public PaymentPeriodHeaderCommandHandler(
         IBus bus,
         IMapper mapper,
         IUnitOfWork unitOfWork,
         IRepository<PaymentPeriod> repositoryPayment,
         IRepository<Invoice> repositoryInvoice,
         IRepository<EntityStatus> repositoryEntityStatus,
         IRepository<InvoiceDetail> repositoryInvoiceDetail,
         IRepository<Concept> repositoryConcept)
        {
            _bus = bus;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repositoryPayment = repositoryPayment;
            _repositoryInvoice = repositoryInvoice;
            _repositoryEntityStatus = repositoryEntityStatus;
            _repositoryInvoiceDetail = repositoryInvoiceDetail;
            _repositoryConcept = repositoryConcept;
        }


        public async Task<CommandResult> Handle(PaymentPeriodHeaderCommand message)
        {
            int invoiceId = 0;
            var invoiceEntity = new Invoice();
            try
            {
                var paymentPeriodPayed = await _repositoryEntityStatus.FirstOrDefaultAsync(q => q.EntityCode == Constants.EntityCode.PaymentPeriod && q.Code == Constants.EntityStatus.PaymentPeriod.Payed);
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
                    invoiceEntity = new Invoice();
                    invoiceEntity.InvoiceId = -1;
                    invoiceEntity.ContractId = firstDetail.ContractId;
                    invoiceEntity.InvoiceDate = DateTime.Now;
                    invoiceEntity.Comment = message.Comment;
                    invoiceEntity.PaymentTypeId = null; // Deberia venir informacion desde la pantalla TAB Other information;
                    invoiceEntity.PaymentOperationNo = message.ReferenceNo;
                    invoiceEntity.CustomerName = message.TenantFullName;
                    invoiceEntity.InvoiceNo = maxInvoice != null ? (int.Parse(maxInvoice.InvoiceNo) + 1).ToString("D9") : "000000001";
                    invoiceEntity.InvoiceStatusId = invoicePayed != null ? (int?)invoicePayed.EntityStatusId : null; //INPAYED
                    invoiceEntity.TotalAmount = message.TotalAmount;
                    invoiceEntity.TotalDeposit = message.TotalDeposit;
                    invoiceEntity.TotalFine = message.TotalFine;
                    invoiceEntity.TotalLateFee = message.TotalLateFee;
                    invoiceEntity.TotalOnAcount = message.TotalOnAcount;
                    invoiceEntity.TotalRent = message.TotalRent;
                    invoiceEntity.TotalService = message.TotalService;
                    invoiceEntity.PaymentTypeId = message.PaymentTypeId;

                    invoiceEntity.RowStatus = true;
                    invoiceEntity.CreationDate = DateTime.Now;
                    invoiceEntity.CreatedBy = message.UserId;
                    invoiceEntity.UpdatedDate = DateTime.Now;
                    invoiceEntity.UpdatedBy = message.UserId;
                    invoiceEntity.TenantId = message.TenantId;
                    invoiceEntity.PeriodId = message.PeriodId;

                    
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
                    }

                    //TODO: TRAER EL CODIGO DE LOS CONCEPTOS QUE RESTAN
                    if (message.IsPayInFull)
                    {
                        var accountConcept = await _repositoryConcept.FirstOrDefaultAsync(q=> q.Code == Constants.ConceptCode.OnAccount);
                        var existOnlyOnAccountCpt = message.PPDetail.Count(q => q.ConceptId != accountConcept.ConceptId); //SI EXISTEN CONCEPTOS PARA RESTAR ACCOUNTS

                        if (existOnlyOnAccountCpt > 0)
                        {
                            //Ingresara solo si existen otros conceptos de Pago como Renta o Deposito u otro,
                            //Caso contrario no grabara los onaccounts en negativo en los Invoice
                            var paymentsForDiscount = await _repositoryPayment.ListAsync(q => q.PeriodId == message.PeriodId
                            && q.TenantId == message.TenantId && q.RowStatus && q.ConceptId == accountConcept.ConceptId && q.PaymentPeriodStatusId == paymentPeriodPayed.EntityStatusId);
                            // 12: PPPAYED
                            // 23: ONACCOUNT Concept
                            var existDiscount = false;
                            foreach (var payment in paymentsForDiscount)
                            {
                                var existsOnAccountsOnInvoices = await _repositoryInvoiceDetail.AnyAsync(q => q.PaymentPeriodId == payment.PaymentPeriodId
                                                                                                         && q.TotalAmount.Value < 0
                                                                                                         && q.RowStatus.Value);
                                //Valida que no exista en otra factura los descuentos (ONACCOUNT)
                                //Aqui esta fallando para los onaccounts nuevos, ya que estos si los esta agregando ya 
                                //que al ser nuevos aun no existen en una factura (CORREGIR!!!)
                                if (!existsOnAccountsOnInvoices)
                                {
                                    existDiscount = true;
                                    --c;
                                    var invoiceDetailEntity = new InvoiceDetail();
                                    invoiceDetailEntity.ConceptId = payment.ConceptId;
                                    invoiceDetailEntity.Qty = 1;
                                    invoiceDetailEntity.UnitPrice = payment.PaymentAmount;
                                    invoiceDetailEntity.InvoiceDetailId = c;
                                    invoiceDetailEntity.InvoiceId = -1;
                                    invoiceDetailEntity.TotalAmount = payment.PaymentAmount * -1;
                                    invoiceDetailEntity.RowStatus = true;
                                    invoiceDetailEntity.CreationDate = DateTime.Now;
                                    invoiceDetailEntity.CreatedBy = message.UserId;
                                    invoiceDetailEntity.UpdatedDate = DateTime.Now;
                                    invoiceDetailEntity.UpdatedBy = message.UserId;
                                    invoiceDetailEntity.PaymentPeriodId = payment.PaymentPeriodId;
                                    invoiceDetailsEntity.Add(invoiceDetailEntity);
                                    invoiceEntity.TotalAmount += invoiceDetailEntity.TotalAmount;
                                }
                            }
                            if (existDiscount)
                                invoiceEntity.Comment = string.Format("On Account concepts were applied ");
                        }
                    }

                    invoiceEntity.InvoiceDetails = invoiceDetailsEntity;

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
                    invoiceId = invoiceEntity.InvoiceId.Value;
                    message.PaymentPeriodId = index;
                }

                var entityToSave = new PaymentPeriod();
                entityToSave.PaymentPeriodId = index;
                return entityToSave.ToRegisterdResult().WithId(invoiceId);

            }
            catch (Exception ex)
            {
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
                entityToSave.HouseId = item.HouseId;
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

