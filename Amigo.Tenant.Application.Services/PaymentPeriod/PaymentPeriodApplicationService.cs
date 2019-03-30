using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.PaymentPeriod;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Application.Services.Interfaces.PaymentPeriod;
using Amigo.Tenant.Commands.PaymentPeriod;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Amigo.Tenant.Application.Services.PaymentPeriod
{
    public class PaymentPeriodApplicationService: IPaymentPeriodApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<PPSearchDTO> _paymentPeriodSearchDataAccess;
        private readonly IQueryDataAccess<PaymentPeriodRegisterRequest> _paymentPeriodDataAccess;
        private readonly IEntityStatusApplicationService _entityStatusApplicationService;
        private readonly IPeriodApplicationService _periodApplicationService;
        private readonly IConceptApplicationService _conceptApplicationService;
        private readonly IQueryDataAccess<PPSearchByContractPeriodDTO> _paymentPeriodSearchByContractDataAccess;
        private readonly IQueryDataAccess<PPHeaderSearchByInvoiceDTO> _paymentPeriodSearchByInvoiceDataAccess;
        private readonly IGeneralTableApplicationService _generalTableApplicationService;
        private readonly IAppSettingApplicationService _appSettingApplicationService;
        private readonly IQueryDataAccess<PaymentPeriodGroupedStatusAndConceptDTO> _paymentPeriodGroupedStatusAndConceptDTOApplication;
        
        public PaymentPeriodApplicationService(IBus bus,
            IQueryDataAccess<PPSearchDTO> paymentPeriodSearchDataAccess,
            IQueryDataAccess<PaymentPeriodRegisterRequest> paymentPeriodDataAccess,
            IEntityStatusApplicationService entityStatusApplicationService,
            IPeriodApplicationService periodApplicationService,
            IConceptApplicationService conceptApplicationService,
            IQueryDataAccess<PPSearchByContractPeriodDTO> paymentPeriodSearchByContractDataAccess,
            IMapper mapper,
            IGeneralTableApplicationService generalTableApplicationService,
            IQueryDataAccess<PPHeaderSearchByInvoiceDTO> paymentPeriodSearchByInvoiceDataAccess,
            IAppSettingApplicationService appSettingApplicationService,
            IQueryDataAccess<PaymentPeriodGroupedStatusAndConceptDTO> paymentPeriodGroupedStatusAndConceptDTOApplication)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _paymentPeriodSearchDataAccess = paymentPeriodSearchDataAccess;
            _paymentPeriodDataAccess = paymentPeriodDataAccess;
            _mapper = mapper;
            _entityStatusApplicationService = entityStatusApplicationService;
            _periodApplicationService = periodApplicationService;
            _conceptApplicationService = conceptApplicationService;
            _paymentPeriodSearchByContractDataAccess = paymentPeriodSearchByContractDataAccess;
            _generalTableApplicationService = generalTableApplicationService;
            _paymentPeriodSearchByInvoiceDataAccess = paymentPeriodSearchByInvoiceDataAccess;
            _appSettingApplicationService = appSettingApplicationService;
            _paymentPeriodGroupedStatusAndConceptDTOApplication = paymentPeriodGroupedStatusAndConceptDTOApplication;
        }

        private async Task<int?> GetStatusbyCodeAsync(string entityCode, string statusCode)
        {
            var entityStatus = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(entityCode, statusCode) ;
            if (entityStatus != null)
            return entityStatus.EntityStatusId.Value;

            return null;
        }

        public async Task<ResponseDTO> UpdatePaymentPeriodAsync(PPHeaderSearchByContractPeriodDTO paymentsPeriod)
        {
            //Map to Command
            var response = await ValidateEntityUpdate(paymentsPeriod);
            var entityStatusPayed = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Payed);

            if (response.IsValid)
            {
                //Execute Command
                var command = _mapper.Map<PPHeaderSearchByContractPeriodDTO, PaymentPeriodHeaderCommand>(paymentsPeriod);
                var commandDetails = new List<PaymentPeriodDetailCommand>();                
                foreach (var item in paymentsPeriod.PPDetail)
                {
                    var commandDetail = _mapper.Map<PPDetailSearchByContractPeriodDTO, PaymentPeriodDetailCommand>(item);
                    if (item.PaymentPeriodId <= 0)
                        commandDetail.TableStatus = DTOs.Requests.Common.ObjectStatus.Added;
                    else
                        commandDetail.TableStatus = DTOs.Requests.Common.ObjectStatus.Modified;

                    commandDetails.Add(commandDetail);
                    
                }
                command.PPDetail = commandDetails;
                var resp = await _bus.SendAsync(command);
                return ResponseBuilder.Correct(resp);
            }

            return response;
        }

        public async Task<ResponseDTO<PagedList<PPSearchDTO>>> SearchPaymentPeriodAsync(PaymentPeriodSearchRequest search)
        {
            List<OrderExpression<PPSearchDTO>> orderExpressionList = new List<OrderExpression<PPSearchDTO>>();
            orderExpressionList.Add(new OrderExpression<PPSearchDTO>(OrderType.Asc, p => p.PeriodCode));
            orderExpressionList.Add(new OrderExpression<PPSearchDTO>(OrderType.Asc, p => p.TenantFullName));

            Expression<Func<PPSearchDTO, bool>> queryFilter = c => true;
            
            
            if (search.PeriodId.HasValue)
                queryFilter = queryFilter.And(p => p.PeriodId==search.PeriodId);

            if (search.TenantId.HasValue)
                queryFilter = queryFilter.And(p => p.TenantId == search.TenantId);

            if (search.HouseId.HasValue)
                queryFilter = queryFilter.And(p => p.HouseId == search.HouseId);

            if (search.PaymentPeriodStatusId.HasValue)
                queryFilter = queryFilter.And(p => p.PaymentPeriodStatusId == search.PaymentPeriodStatusId);

            if (search.HasPendingFines.HasValue)
                if (search.HasPendingFines.Value)
                    queryFilter = queryFilter.And(p => p.FinesPending > 0);
                else
                    queryFilter = queryFilter.And(p => p.FinesPending == 0);

            if (search.HasPendingLateFee.HasValue)
                if (search.HasPendingLateFee.Value)
                    queryFilter = queryFilter.And(p => p.LateFeesPending > 0);
                else
                    queryFilter = queryFilter.And(p => p.LateFeesPending == 0);

            if (search.HasPendingServices.HasValue)
                if (search.HasPendingServices.Value)
                    queryFilter = queryFilter.And(p => p.ServicesPending > 0);
                else
                    queryFilter = queryFilter.And(p => p.ServicesPending == 0);

            if (search.HasPendingDeposit.HasValue)
                if (search.HasPendingDeposit.Value)
                    queryFilter = queryFilter.And(p => p.DepositPending > 0);
                else
                    queryFilter = queryFilter.And(p => p.DepositPending == 0);

            if (!string.IsNullOrEmpty(search.ContractCode))
                queryFilter = queryFilter.And(p => p.ContractCode.Contains(search.ContractCode));


            var paymentPeriod = await _paymentPeriodSearchDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<PPSearchDTO>()
            {
                Items = paymentPeriod.Items,
                PageSize = paymentPeriod.PageSize,
                Page = paymentPeriod.Page,
                Total = paymentPeriod.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<PPHeaderSearchByContractPeriodDTO>> SearchPaymentPeriodByContractAsync(PaymentPeriodSearchByContractPeriodRequest search)
        {
            Expression<Func<PPSearchByContractPeriodDTO, bool>> queryFilter = c => true;

            if (search.PeriodId.HasValue)
                queryFilter = queryFilter.And(p => p.PeriodId == search.PeriodId);

            if (search.ContractId.HasValue)
                queryFilter = queryFilter.And(p => p.ContractId == search.ContractId);

            var paymentsPeriod = await _paymentPeriodSearchByContractDataAccess.ListAsync(queryFilter);
            var lateFeePaymenType = await _generalTableApplicationService.GetGeneralTableByEntityAndCodeAsync(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.LateFee);
            var appSettingTenantFavorable = await _appSettingApplicationService.GetAppSettingByCodeAsync(Constants.AppSettingCode.CptToFavTn);
            var isLateFeeTenantFavorable = appSettingTenantFavorable != null && appSettingTenantFavorable.AppSettingValue.Contains(Constants.ConceptCode.LateFee);

            var appSettingLateFeeXDay = await _appSettingApplicationService.GetAppSettingByCodeAsync(Constants.AppSettingCode.LateFeeXDay);
            var lateFeeXDayValue = appSettingLateFeeXDay != null && appSettingLateFeeXDay.AppSettingValue!= string.Empty? decimal.Parse(appSettingLateFeeXDay.AppSettingValue):0;

            var entityStatusPayment = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);
            var concept = await _conceptApplicationService.GetConceptByCodeAsync(Constants.ConceptCode.LateFee);
            
            var ppHeaderSearchByContractPeriodDTO = new PPHeaderSearchByContractPeriodDTO();

            if (paymentsPeriod.Any())
            {
                var header = paymentsPeriod.FirstOrDefault();
                ppHeaderSearchByContractPeriodDTO.HouseName = header.HouseName;
                ppHeaderSearchByContractPeriodDTO.PeriodCode = header.PeriodCode;
                ppHeaderSearchByContractPeriodDTO.PeriodId = header.PeriodId;
                ppHeaderSearchByContractPeriodDTO.TenantFullName = header.TenantFullName;
                ppHeaderSearchByContractPeriodDTO.ContractId = header.ContractId;
                ppHeaderSearchByContractPeriodDTO.DueDate = header.DueDate;
                ppHeaderSearchByContractPeriodDTO.Email = header.Email;
                ppHeaderSearchByContractPeriodDTO.TenantId = header.TenantId;
                ppHeaderSearchByContractPeriodDTO.PaymentTypeId = header.PaymentTypeId;
                ppHeaderSearchByContractPeriodDTO.IsPayInFull = false; //Required To apply OnAccount concept when Tenant is paying (false: pay independent concept, true: pay for the total)
                ppHeaderSearchByContractPeriodDTO.TotalInvoice = header.TotalInvoice;
                ppHeaderSearchByContractPeriodDTO.TotalIncome = header.TotalIncome;


                var detailList = new List<PPDetailSearchByContractPeriodDTO>();
                var lateFeeDetail = new PPDetailSearchByContractPeriodDTO();
                var delayDays = DateTime.Today.Subtract(header.PeriodDueDate.Value).TotalDays;
                var isLateFeeIncluded = false;
                var existLateFeeInDB = paymentsPeriod.Any(q => q.PaymentTypeCode == Constants.GeneralTableCode.PaymentType.LateFee);

                foreach (var item in paymentsPeriod)
                {
                    var detail = new PPDetailSearchByContractPeriodDTO();
                    detail.ContractId = item.ContractId;
                    detail.PaymentPeriodId = item.PaymentPeriodId;
                    detail.PaymentTypeValue = item.PaymentTypeValue;
                    detail.PaymentAmount = item.PaymentAmount;
                    detail.PaymentDescription = item.PaymentDescription;
                    detail.PaymentPeriodStatusCode = item.PaymentPeriodStatusCode;
                    detail.PaymentPeriodStatusName = item.PaymentPeriodStatusName;
                    detail.IsRequired = item.IsRequired;
                    detail.PaymentTypeCode = item.PaymentTypeCode;
                    detail.IsSelected = item.IsRequired.Value && item.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending ? true : false;
                    detail.InvoiceDetailId = item.InvoiceDetailId;
                    detail.InvoiceId = item.InvoiceId;
                    detail.InvoiceNo = item.InvoiceNo;
                    detail.InvoiceDate = item.InvoiceDate;
                    detail.ConceptId = item.ConceptId;
                    detail.ConceptCode = item.ConceptCode;
                    detail.IsTenantFavorable = item.IsTenantFavorable;
                    detailList.Add(detail);

                    if (!existLateFeeInDB
                        && delayDays > 0 
                        && detail.PaymentTypeCode == Constants.GeneralTableCode.PaymentType.Rent)
                    {
                        lateFeeDetail.PaymentPeriodId = -1;
                        lateFeeDetail.ContractId = header.ContractId;
                        lateFeeDetail.PeriodId = header.PeriodId;
                        lateFeeDetail.PaymentAmount = lateFeeXDayValue * (decimal?)delayDays;
                        lateFeeDetail.PaymentTypeId = lateFeePaymenType.GeneralTableId;
                        lateFeeDetail.PaymentTypeValue = lateFeePaymenType.Value;
                        lateFeeDetail.PaymentTypeCode = lateFeePaymenType.Code;
                        lateFeeDetail.PaymentTypeName = lateFeePaymenType.Code;
                        lateFeeDetail.PaymentPeriodStatusCode = Constants.EntityStatus.PaymentPeriod.Pending;
                        lateFeeDetail.PaymentPeriodStatusId = entityStatusPayment.EntityStatusId;
                        lateFeeDetail.IsRequired = false;
                        lateFeeDetail.IsSelected = false;
                        lateFeeDetail.TableStatus = DTOs.Requests.Common.ObjectStatus.Added;
                        lateFeeDetail.PaymentDescription = lateFeePaymenType.Value;
                        lateFeeDetail.ConceptId = concept.Data.ConceptId;
                        lateFeeDetail.ConceptCode = concept.Data.Code;
                        lateFeeDetail.TenantId = header.TenantId;
                        lateFeeDetail.IsTenantFavorable = isLateFeeTenantFavorable;

                        lateFeeDetail.PaymentPeriodStatusName = Constants.EntityStatus.PaymentPeriodStatusName.Pending;

                        if (item.PaymentTypeSequence + 1 == lateFeePaymenType.Sequence)
                        {
                            //Inserting at Final
                            detailList.Add(lateFeeDetail);
                            isLateFeeIncluded = true;
                            ppHeaderSearchByContractPeriodDTO.TotalIncome += lateFeeDetail.PaymentAmount;
                        }
                    }

                }

                //Inserting at Final
                if (!isLateFeeIncluded && lateFeeDetail.PaymentPeriodId.HasValue)
                {
                    ppHeaderSearchByContractPeriodDTO.TotalIncome += lateFeeDetail.PaymentAmount;
                    detailList.Add(lateFeeDetail);
                }


                ppHeaderSearchByContractPeriodDTO.PPDetail = detailList;

                //ReCalculando el Balance

                ppHeaderSearchByContractPeriodDTO.Balance = ppHeaderSearchByContractPeriodDTO.TotalIncome - ppHeaderSearchByContractPeriodDTO.TotalInvoice;

            }

            return ResponseBuilder.Correct(ppHeaderSearchByContractPeriodDTO);
        }

        public async Task<ResponseDTO<List<PPHeaderSearchByInvoiceDTO>>> SearchInvoiceByIdAsync(string invoiceNo)
        {
            Expression<Func<PPHeaderSearchByInvoiceDTO, bool>> queryFilter = c => true;
            queryFilter = queryFilter.And(p => p.InvoiceNo == invoiceNo);
            var paymentsPeriod = await _paymentPeriodSearchByInvoiceDataAccess.ListAsync(queryFilter);
            return ResponseBuilder.Correct(paymentsPeriod.ToList());
        }

        public async Task<ResponseDTO<PPDetailSearchByContractPeriodDTO>> CalculateLateFeeByContractAndPeriodAsync(PaymentPeriodSearchByContractPeriodRequest search)
        {
            Expression<Func<PPSearchByContractPeriodDTO, bool>> queryFilter = c => true;
            if (search.PeriodId.HasValue)
                queryFilter = queryFilter.And(p => p.PeriodId == search.PeriodId);
            if (search.ContractId.HasValue)
                queryFilter = queryFilter.And(p => p.ContractId == search.ContractId);

            var header = await _paymentPeriodSearchByContractDataAccess.FirstOrDefaultAsync(queryFilter);
            var lateFeePaymenType = await _generalTableApplicationService.GetGeneralTableByEntityAndCodeAsync(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.LateFee);
            var concept = await _conceptApplicationService.GetConceptByCodeAsync(Constants.ConceptCode.LateFee);
            var entityStatusPayment = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);
            var lateFeeDetail = new PPDetailSearchByContractPeriodDTO();

            if (header != null)
            {
                //var header = paymentsPeriod.FirstOrDefault();
                var delayDays = DateTime.Today.Subtract(header.PeriodDueDate.Value).TotalDays;

                queryFilter = queryFilter.And(p => p.PaymentTypeCode == Constants.GeneralTableCode.PaymentType.LateFee);
                var existLateFeeInDB = await _paymentPeriodSearchByContractDataAccess.FirstOrDefaultAsync(queryFilter);

                if (header != null)
                {
                    if (existLateFeeInDB == null && delayDays > 0)
                    {
                        lateFeeDetail.PaymentPeriodId = -1;
                        lateFeeDetail.ContractId = header.ContractId;
                        lateFeeDetail.PeriodId = header.PeriodId;
                        lateFeeDetail.PaymentAmount = 25 * (decimal?)delayDays;
                        lateFeeDetail.PaymentTypeId = lateFeePaymenType.GeneralTableId;
                        lateFeeDetail.PaymentTypeValue = lateFeePaymenType.Value;
                        lateFeeDetail.PaymentTypeCode = lateFeePaymenType.Code;
                        lateFeeDetail.PaymentTypeName = lateFeePaymenType.Code;
                        lateFeeDetail.PaymentPeriodStatusCode = Constants.EntityStatus.PaymentPeriod.Pending;
                        lateFeeDetail.PaymentPeriodStatusId = entityStatusPayment.EntityStatusId;
                        lateFeeDetail.IsRequired = false;
                        lateFeeDetail.IsSelected = false;
                        lateFeeDetail.TableStatus = DTOs.Requests.Common.ObjectStatus.Added;
                        lateFeeDetail.PaymentDescription = lateFeePaymenType.Value;
                        lateFeeDetail.ConceptId = concept.Data.ConceptId;
                        lateFeeDetail.TenantId = header.TenantId;
                        lateFeeDetail.PaymentPeriodStatusName = Constants.EntityStatus.PaymentPeriodStatusName.Pending;
                        return ResponseBuilder.Correct(lateFeeDetail);
                    }
                    else if (existLateFeeInDB != null)
                    {
                        var entity = _mapper.Map<PPSearchByContractPeriodDTO, PPDetailSearchByContractPeriodDTO>(existLateFeeInDB);
                        entity.PaymentAmount = 25 * (decimal?)delayDays;
                        entity.TableStatus = DTOs.Requests.Common.ObjectStatus.Modified;
                        return ResponseBuilder.Correct(entity);
                    }
                }
            }
            return null;
        }

        public async Task<ResponseDTO<PPDetailSearchByContractPeriodDTO>> CalculateOnAccountByContractAndPeriodAsync(PaymentPeriodSearchByContractPeriodRequest search)
        {

            Expression<Func<PPSearchByContractPeriodDTO, bool>> queryFilter = c => true;
            if (search.PeriodId.HasValue)
                queryFilter = queryFilter.And(p => p.PeriodId == search.PeriodId);
            if (search.ContractId.HasValue)
                queryFilter = queryFilter.And(p => p.ContractId == search.ContractId);

            var header = await _paymentPeriodSearchByContractDataAccess.FirstOrDefaultAsync(queryFilter);
            var onAccountPaymenType = await _generalTableApplicationService.GetGeneralTableByEntityAndCodeAsync(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.OnAccount);
            var concept = await _conceptApplicationService.GetConceptByCodeAsync(Constants.ConceptCode.OnAccount);
            var entityStatusPayment = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);
            var onAccountDetail = new PPDetailSearchByContractPeriodDTO();

            if (header != null)
            {
                if (header != null)
                {
                        onAccountDetail.PaymentPeriodId = -2;
                        onAccountDetail.ContractId = header.ContractId;
                        onAccountDetail.PeriodId = header.PeriodId;
                        onAccountDetail.PaymentAmount = 0;
                        onAccountDetail.PaymentTypeId = onAccountPaymenType.GeneralTableId;
                        onAccountDetail.PaymentTypeValue = onAccountPaymenType.Value;
                        onAccountDetail.PaymentTypeCode = onAccountPaymenType.Code;
                        onAccountDetail.PaymentTypeName = onAccountPaymenType.Code;
                        onAccountDetail.PaymentPeriodStatusCode = Constants.EntityStatus.PaymentPeriod.Pending;
                        onAccountDetail.PaymentPeriodStatusId = entityStatusPayment.EntityStatusId;
                        onAccountDetail.IsRequired = false;
                        onAccountDetail.IsSelected = false;
                        onAccountDetail.TableStatus = DTOs.Requests.Common.ObjectStatus.Added;
                        onAccountDetail.PaymentDescription = onAccountPaymenType.Value;
                        onAccountDetail.ConceptId = concept.Data.ConceptId;
                        onAccountDetail.TenantId = header.TenantId;
                        onAccountDetail.PaymentPeriodStatusName = Constants.EntityStatus.PaymentPeriodStatusName.Pending;

                        return ResponseBuilder.Correct(onAccountDetail);
                  }
            }
            return null;
        }

        //public async Task<ResponseDTO> ValidateEntityRegister(PaymentPeriodRegisterRequest request)
        //{
        //    bool isValid = true;
        //    Expression<Func<PaymentPeriodRegisterRequest, bool>> queryFilter = p => true;
        //    var errorMessage = "";
        //    queryFilter = queryFilter.And(p => p.PaymentPeriodCode == request.PaymentPeriodCode);
        //    var paymentPeriod = await _paymentPeriodDataAccess.FirstOrDefaultAsync(queryFilter);

        //    if (paymentPeriod != null)
        //    {
        //        isValid = false;
        //        errorMessage = "PaymentPeriod already Exists";
        //    }

        //    //Existe un Lease para el mismo tenant Activo o Futuro
        //    if (isValid)
        //    {
        //        queryFilter = p => p.TenantId == request.TenantId;
        //        queryFilter = queryFilter.And(p => p.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Active || p.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Future);
        //        paymentPeriod = await _paymentPeriodDataAccess.FirstOrDefaultAsync(queryFilter);

        //        if (paymentPeriod != null)
        //        {
        //            isValid = false;
        //            errorMessage = "Already Exists a tenant associated to other Lease Active or Future";
        //        }
        //    }

        //    //Validate Period
        //    if (isValid)
        //    {
        //        Expression<Func<PeriodDTO, bool>> queryFilterPeriod = p => true;
        //        queryFilterPeriod = queryFilterPeriod.And(p => p.EndDate < request.EndDate);
        //        var period = await _periodApplicationService.GetLastPeriodAsync();

        //        if (period != null && period.Data.EndDate < request.EndDate)
        //        {
        //            isValid = false;
        //            errorMessage = "There is no period configurated to support the end date of this Lease, please create period";
        //        }
        //    }


        //    var response = new ResponseDTO()
        //    {
        //        IsValid = string.IsNullOrEmpty(errorMessage),
        //        Messages = new List<ApplicationMessage>()
        //    };

        //    response.Messages.Add(new ApplicationMessage()
        //    {
        //        Key = string.IsNullOrEmpty(errorMessage) ? "Ok" : "Error",
        //        Message = errorMessage
        //    });

        //    return response;
        //}

        public async Task<ResponseDTO> ValidateEntityUpdate(PPHeaderSearchByContractPeriodDTO request)
        {
            var errorMessage = "";
            //Expression<Func<PaymentPeriodRegisterRequest, bool>> queryFilter = p => p.RowStatus;
            //queryFilter = queryFilter.And(p => p.PaymentPeriodId != request.PaymentPeriodId);
            //queryFilter = queryFilter.And(p => p.TenantId == request.TenantId);
            //queryFilter = queryFilter.And(p => p.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Active || p.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Future);

            bool isValid = true;

            if (!request.PPDetail.Any(q=> q.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending && q.IsSelected.Value))
            {
                isValid = false;
                errorMessage = "No existe ningun pendiente que este seleccionado";
            }

            if (request.PPDetail.Any(q => 
                    q.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending 
                    && q.PaymentTypeCode == Constants.GeneralTableCode.PaymentType.OnAccount
                    && (!q.IsSelected.Value || !q.IsSelected.HasValue)))
            {
                isValid = false;
                errorMessage = "No puedes grabar un Payment On Account sin seleccionarlo";
            }

            if (request.PPDetail.Any(q =>
                    q.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending
                    && q.PaymentAmount == 0))
            {
                isValid = false;
                errorMessage = "No puedes grabar un Payment con monto en cero";
            }

            var onAccountConcept = await _conceptApplicationService.GetConceptByCodeAsync(Constants.ConceptCode.OnAccount);
            var existJustOneOnAccountRecord = request.PPDetail.Count(q => q.ConceptId == onAccountConcept.Data.ConceptId && q.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending);
            if (existJustOneOnAccountRecord>1)
            {
                isValid = false;
                //errorMessage = "Please group the OnAccount Concept in one record and save it";
                errorMessage = "Por favor agrupa los pagos acuenta en uno solo";
            }


            var onAccountAmountClient = request.PPDetail.Where(q=> q.ConceptId == onAccountConcept.Data.ConceptId && q.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending).Sum(q=> q.PaymentAmount);
            var amountsInDB = await _paymentPeriodGroupedStatusAndConceptDTOApplication.FirstOrDefaultAsync(q => q.TenantId == request.TenantId && q.PeriodId == request.PeriodId);
            var totalIncomeInDB = amountsInDB != null ? amountsInDB.TotalDepositAmountPending + amountsInDB.TotalRentAmountPending + amountsInDB.TotalFineInfracAmountPending + amountsInDB.TotalLateFeeAmountPending + amountsInDB.TotalSvcEnergyAmountPending + amountsInDB.TotalSvcWaterAmountPending: 0;
            var totalOnAccountInDB = amountsInDB != null ? amountsInDB.TotalOnAccountAmountPayed : 0;

            if (amountsInDB != null && onAccountAmountClient + totalOnAccountInDB > totalIncomeInDB )
            {
                isValid = false;
                //No debes poder agregar A Cuenta mas de lo que esta pendiente por Ingresos
                //Si tienes renta y deposito por 2000, no debes crear registros OnAccount en el Cliente y en BD por mas de esta cifra.
                //errorMessage = "OnAccount amount must be leaser or equal than other income concepts ";
                errorMessage = "El total de los pagos a cuenta no deben exceder al total de los conceptos de ingresos pendiente (Renta, Deposito, etc)";
            }

            var exist = request.PPDetail.Any(q => q.ConceptId != onAccountConcept.Data.ConceptId && q.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending && q.IsSelected.Value);
            if (amountsInDB != null && onAccountAmountClient + totalOnAccountInDB == totalIncomeInDB && !exist)
            {
                isValid = false;
                //Cuando lo unico que se ha seleccionado es un concepto OnAccount y el monto que se quiere pagar YA es Igual al Total de Ingresos (Renta, Deposito, etc)
                //Selecciona todos los conceptos de pago por ingresos en ve de ingresar un concepto por pago a cuenta, por que hemos detectado que quieres pagar el total del periodo.
                //errorMessage = "Select all income concepts allowed instead of OnAccount concept, because we have detected that you wanna to pay in full the period";
                errorMessage = "Hemos detectado que con el pago a cuenta que quieres ingresar, estas saldando la deuda del periodo, para hacer esto, deberias seleccionar los conceptos de ingreso pendientes, en lugar de intentar registrar pagos a cuenta.";
            }

            var existOnACcountConcepts = request.PPDetail.Any(q => q.ConceptId == onAccountConcept.Data.ConceptId && q.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending && q.IsSelected.Value);
            var existIncomeConcepts = request.PPDetail.Any(q => q.ConceptId != onAccountConcept.Data.ConceptId && q.PaymentPeriodStatusCode == Constants.EntityStatus.PaymentPeriod.Pending && q.IsSelected.Value);

            if (amountsInDB != null && existOnACcountConcepts && request.IsPayInFull)
            {
                isValid = false;
                errorMessage = "Estas intentando pagar la totalidad de la deuda, pero agregando pagos a cuenta adicionales, elimina el pago a cuenta y selecciona todos los conceptos de ingreso.";
            }

            var response = new ResponseDTO()
            {
                IsValid = string.IsNullOrEmpty(errorMessage),
                Messages = new List<ApplicationMessage>()
            };

            response.Messages.Add(new ApplicationMessage()
            {
                Key = string.IsNullOrEmpty(errorMessage) ? "Ok" : "Error",
                Message = errorMessage
            });

            return response;
        }

        /*EXCEL REPORT*/

        public async Task GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent,
            TransportContext transportContext, PaymentPeriodSearchRequest search)
        {
            var list = await SearchPaymentPeriodAsync(search);
            try
            {
                if (list.Data.Items.Count > 0)
                    using (var writer = new StreamWriter(outputStream))
                    {
                        var headers = new List<string>
                        {
                            "Period",
                            "DueDate",
                            "Contract",
                            "Tenant",
                            "House",
                            "Rent Status",
                            "Rent",
                            "Pending Deposit",
                            "Pending Fine",
                            "Pending Services",
                            "Pending Fee"
                        };
                        await writer.WriteLineAsync(ExcelHelper.GetHeaderDetail(headers));
                        foreach (var item in list.Data.Items)
                            await writer.WriteLineAsync(ProcessCellDataToReport(item));
                    }
            }
            catch (HttpException ex)
            {
            }
            finally
            {
                outputStream.Close();
            }
        }

        private string ProcessCellDataToReport(PPSearchDTO item)
        {
            var dueDate = string.Format("{0:MM/dd/yyyy}", item.DueDate) ?? "";
            var pendingRent = string.Format("{0:0.00}", item.PaymentAmount);
            var pendingDeposit = string.Format("{0:0.00}", item.DepositAmountPending);
            var pendingService = string.Format("{0:0.00}", item.ServicesAmountPending);
            var pendingLateFee = string.Format("{0:0.00}", item.LateFeesAmountPending);
            var pendingFine = string.Format("{0:0.00}", item.FinesAmountPending);

            var textProperties = ExcelHelper.StringToCSVCell(item.PeriodCode) + "," +
                                 ExcelHelper.StringToCSVCell(dueDate) + "," +
                                 ExcelHelper.StringToCSVCell(item.ContractCode) + "," +
                                 ExcelHelper.StringToCSVCell(item.TenantFullName) + "," +
                                 ExcelHelper.StringToCSVCell(item.HouseName) + "," +
                                 ExcelHelper.StringToCSVCell(item.PaymentPeriodStatusCode) + "," +
                                 ExcelHelper.StringToCSVCell(pendingRent) + "," +
                                 ExcelHelper.StringToCSVCell(pendingDeposit) + "," +
                                 ExcelHelper.StringToCSVCell(pendingFine) + "," +
                                 ExcelHelper.StringToCSVCell(pendingService) + "," +
                                 ExcelHelper.StringToCSVCell(pendingLateFee);

            return textProperties;
        }



    }
}
