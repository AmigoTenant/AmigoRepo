using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using Amigo.Tenant.Application.DTOs.Requests.Leasing;
using Amigo.Tenant.Common;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Commands.Leasing.Contracts;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Web;
using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;

namespace Amigo.Tenant.Application.Services.Tracking
{
    public class ContractApplicationService: IContractApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ContractSearchDTO> _contractSearchDataAccess;
        private readonly IQueryDataAccess<ContractRegisterRequest> _contractDataAccess;
        private readonly IEntityStatusApplicationService _entityStatusApplicationService;
        private readonly IPeriodApplicationService _periodApplicationService;
        private readonly IConceptApplicationService _conceptApplicationService;
        private readonly IQueryDataAccess<OtherTenantRegisterRequest> _otherTenantDataAccess;
        private readonly IQueryDataAccess<HouseFeatureDetailContractDTO> _houseFeatureDetailContractDataAccess;
        private readonly IGeneralTableApplicationService _generalTableApplicationService;

        public ContractApplicationService(IBus bus,
            IQueryDataAccess<ContractSearchDTO> contractSearchDataAccess,
            IQueryDataAccess<ContractRegisterRequest> contractDataAccess,
            IEntityStatusApplicationService entityStatusApplicationService,
            IPeriodApplicationService periodApplicationService,
            IConceptApplicationService conceptApplicationService,
            IMapper mapper,
            IQueryDataAccess<OtherTenantRegisterRequest> otherTenantDataAccess,
            IQueryDataAccess<HouseFeatureDetailContractDTO> houseFeatureDetailContractDataAccess,
            IGeneralTableApplicationService generalTableApplicationService)
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _contractSearchDataAccess = contractSearchDataAccess;
            _contractDataAccess = contractDataAccess;
            _mapper = mapper;
            _entityStatusApplicationService = entityStatusApplicationService;
            _periodApplicationService = periodApplicationService;
            _conceptApplicationService = conceptApplicationService;
            _otherTenantDataAccess = otherTenantDataAccess;
            _houseFeatureDetailContractDataAccess = houseFeatureDetailContractDataAccess;
            _generalTableApplicationService = generalTableApplicationService;
        }

        public async Task<ResponseDTO> RegisterContractAsync(ContractRegisterRequest request)
        {

            request.ContractCode = await GetNextCode(request);
            request.PeriodId = await GetPeriodByCode(request.ContractCode.Substring(2, 6));
            request.ContractDate = DateTime.Now;

            //TODO: Matodo para agregar informacion a la tabla Obligation, conceptos del tipo obligations
            var response = await ValidateEntityRegister(request);

            if (response.IsValid)
            {
                //await CreateContractDetail(request);

                //await CreateContractDetailObligation(request);

                var command = _mapper.Map<ContractRegisterRequest, ContractRegisterCommand>(request);

                command.ContractStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.Contract, Constants.EntityStatus.Contract.Draft);

                var resp = await _bus.SendAsync(command);
                
                return ResponseBuilder.Correct(resp, command.ContractId, command.ContractCode);
            }

            return response;
        }

        public async Task<ResponseDTO> ChangeStatusContractAsync(ContractChangeStatusRequest request)
        {

            var contractRequest = await GetByIdAsync(request.ContractId);

            await CreateContractDetail(contractRequest.Data);

            var command = _mapper.Map<ContractRegisterRequest, ContractChangeStatusCommand>(contractRequest.Data);

            command.ContractStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.Contract, Constants.EntityStatus.Contract.Formalized);
            command.UserId = request.UserId.Value;
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp, command.ContractId, command.ContractCode);

        }

        private async Task<int?> GetPeriodByCode(string periodCode)
        {
            var period = await _periodApplicationService.GetPeriodByCodeAsync(periodCode);
            if (period.Data != null)
            return period.Data.PeriodId;

            return null;
        }

        private async Task<string> GetNextCode(ContractRegisterRequest request)
        {
            var year = String.Format("{0:D3}", request.BeginDate.Value.Year);
            var month = String.Format("{0:D2}", request.BeginDate.Value.Month);
            var prefixContractCode = "LE" + year + month;
            var lastCode = await GetLastCode(prefixContractCode);
            string value = "001";
            if (lastCode.Data != null)
            {
                var code = lastCode.Data.ContractCode;
                value = String.Format("{0:D3}", int.Parse(code.Substring(code.Length-3, 3)) + 1);
            }
            return prefixContractCode + value;
        }

        public async Task<ResponseDTO<ContractRegisterRequest>> GetLastCode(string contractCode)
        {
            List<OrderExpression<ContractRegisterRequest>> orderExpressionList = new List<OrderExpression<ContractRegisterRequest>>();
            orderExpressionList.Add(new OrderExpression<ContractRegisterRequest>(OrderType.Desc, p => p.ContractCode));
            Expression<Func<ContractRegisterRequest, bool>> queryFilter = c => c.ContractCode.Contains(contractCode);
            var mainTenant = await _contractDataAccess.FirstOrDefaultAsync(queryFilter, orderExpressionList.ToArray());

            return ResponseBuilder.Correct(mainTenant);
        }

        private async Task CreateContractDetailObligation(ContractRegisterRequest request)
        {
            var obligations = await _conceptApplicationService.GetConceptsByTypeCodeAsync(Constants.GeneralTableCode.ConceptType.Obligation);
            var contractDetailObligationList = new List<ContractDetailObligationRegisterRequest>();
            var contractDetailFirst = request.ContractDetails.First();
            var c = 1;
            foreach (var item in obligations.Data)
            {
                var contractDetailObligation = new ContractDetailObligationRegisterRequest();
                contractDetailObligation.ContractDetailId = contractDetailFirst.ContractDetailId;
                contractDetailObligation.ContractDetailObligationId = c * -1;
                contractDetailObligation.ObligationDate = DateTime.Now;
                contractDetailObligation.ConceptId = item.ConceptId;
                contractDetailObligation.EntityStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.ContractDetail, Constants.EntityStatus.ContractDetail.Pending);
                contractDetailObligation.Comment = item.Description;
                contractDetailObligation.InfractionAmount = item.ConceptAmount;
                contractDetailObligation.PeriodId = request.PeriodId;
                contractDetailObligation.TenantId = request.TenantId;
                contractDetailObligation.RowStatus = true;
                contractDetailObligation.CreatedBy = request.UserId;
                contractDetailObligation.CreationDate = DateTime.Now;
                contractDetailObligation.UpdatedBy = request.UserId;
                contractDetailObligation.UpdatedDate = DateTime.Now;
                contractDetailObligationList.Add(contractDetailObligation);
            }

            contractDetailFirst.ContractDetailObligations = contractDetailObligationList;
        }

        private async Task CreateContractDetail(ContractRegisterRequest request)
        {
            var isLastPeriod = false;
            var period = (await _periodApplicationService.GetPeriodByIdAsync(request.PeriodId)).Data;
            var contractEndDate = request.EndDate;
            var currentPeriodDueDate = period.DueDate.Value.AddMonths(1);
            var id = -1;
            var contractDetails = new List<ContractDetailRegisterRequest>();
            var paymentsPeriod = new List<PaymentPeriodRegisterRequest>();
            var rentConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Rent);
            var depositConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Deposit);
            var entityStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);
            var paymentTypeRentId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Rent);
            var paymentTypeDepositId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Deposit);



            while (contractEndDate > currentPeriodDueDate)
            {
                //SetContractDetail(contractDetails, request, period, id, isLastPeriod);
                SetPaymentsPeriod(paymentsPeriod, request, period, id, isLastPeriod, rentConceptId, entityStatusId, paymentTypeRentId, depositConceptId, paymentTypeDepositId);

                //TODO: nO HA SEQUENCE 13 SE ESTA CAYENDO
                period = (await _periodApplicationService.GetPeriodBySequenceAsync(period.Sequence + 1)).Data;
                currentPeriodDueDate = period.DueDate.Value.AddMonths(1);
                id--;

                if (contractEndDate.Value.Year == currentPeriodDueDate.Year && contractEndDate.Value.Month == currentPeriodDueDate.Month)
                {
                    isLastPeriod = true;
                    //SetContractDetail(contractDetails, request, period, id, isLastPeriod);
                    SetPaymentsPeriod(paymentsPeriod, request, period, id, isLastPeriod, rentConceptId, entityStatusId, paymentTypeRentId, depositConceptId, paymentTypeDepositId);
                    //request.ContractDetails = contractDetails;
                    request.PaymentsPeriod = paymentsPeriod;
                    return;
                }
                else if (currentPeriodDueDate.CompareTo(contractEndDate)>0)
                {
                    //request.ContractDetails = contractDetails;
                    request.PaymentsPeriod = paymentsPeriod;
                    return;
                }
            }
        }

        private void SetPaymentsPeriod(List<PaymentPeriodRegisterRequest> paymentsPeriod, ContractRegisterRequest request, PeriodDTO period, int id, bool isLastPeriod, int? rentId, int? entityStatusId, int? paymentTypeId, int? depositConceptId, int? paymentTypeDepositId)
        {
            ///////////////////
            //SETTING FOR  DEPOSIT
            ///////////////////

            if (Math.Abs(id) == 1)
            {
                var paymentPeriodDeposit = new PaymentPeriodRegisterRequest();
                paymentPeriodDeposit.PaymentPeriodId = id;
                paymentPeriodDeposit.ConceptId = depositConceptId; //"CODE FOR CONCEPT"; //TODO:
                paymentPeriodDeposit.ContractId = request.ContractId;
                paymentPeriodDeposit.TenantId = request.TenantId;
                paymentPeriodDeposit.PeriodId = period.PeriodId;
                paymentPeriodDeposit.PaymentPeriodStatusId = entityStatusId; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
                paymentPeriodDeposit.RowStatus = true;
                paymentPeriodDeposit.CreatedBy = request.UserId;
                paymentPeriodDeposit.CreationDate = DateTime.Now;
                paymentPeriodDeposit.UpdatedBy = request.UserId;
                paymentPeriodDeposit.UpdatedDate = DateTime.Now;
                paymentPeriodDeposit.PaymentTypeId = paymentTypeDepositId;
                paymentPeriodDeposit.PaymentAmount = request.RentDeposit;
                paymentPeriodDeposit.DueDate = period.DueDate;
                paymentsPeriod.Add(paymentPeriodDeposit);
            }

            ///////////////////
            //SETTING FOR  RENT
            ///////////////////

            var paymentPeriodRent = new PaymentPeriodRegisterRequest();
            paymentPeriodRent.PaymentPeriodId = id;
            paymentPeriodRent.ConceptId = rentId; //"CODE FOR CONCEPT"; //TODO:
            paymentPeriodRent.ContractId = request.ContractId;
            paymentPeriodRent.TenantId = request.TenantId;
            paymentPeriodRent.PeriodId = period.PeriodId;
            paymentPeriodRent.PaymentPeriodStatusId = entityStatusId; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
            paymentPeriodRent.RowStatus = true;
            paymentPeriodRent.CreatedBy = request.UserId;
            paymentPeriodRent.CreationDate = DateTime.Now;
            paymentPeriodRent.UpdatedBy = request.UserId;
            paymentPeriodRent.UpdatedDate = DateTime.Now;
            paymentPeriodRent.PaymentTypeId = paymentTypeId;

            if (!isLastPeriod)
            {
                paymentPeriodRent.PaymentAmount = Math.Abs(id)==1? CalculateFirstRent(request.BeginDate, request.RentPrice) : request.RentPrice;
            }
            else
            {
                paymentPeriodRent.PaymentAmount = CalculateLastRent(request.EndDate, request.RentPrice);
            }
            paymentPeriodRent.DueDate = period.DueDate;
            paymentsPeriod.Add(paymentPeriodRent);

            
        }

        //private void SetContractDetail(List<ContractDetailRegisterRequest> contractDetails, ContractRegisterRequest request, PeriodDTO period, int id, bool isLastPeriod)
        //{
        //    var contractDetail = new ContractDetailRegisterRequest();
        //    contractDetail.ContractDetailId = id;
        //    contractDetail.ItemNo = id * -1;
        //    contractDetail.Description = period.Code;
        //    contractDetail.ContractDetailStatusId = 8; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
        //    contractDetail.ContractId = request.ContractId;
        //    contractDetail.PeriodId = period.PeriodId;
        //    contractDetail.TotalPayment = contractDetail.Rent;
        //    contractDetail.RowStatus = true;
        //    contractDetail.CreatedBy = request.UserId;
        //    contractDetail.CreationDate = DateTime.Now;
        //    contractDetail.UpdatedBy = request.UserId;
        //    contractDetail.UpdatedDate = DateTime.Now;

        //    if (!isLastPeriod)
        //    {
        //        contractDetail.Rent = contractDetail.ItemNo == 1 ? CalculateFirstRent(request.BeginDate, request.RentPrice) : request.RentPrice;
        //    }
        //    else
        //    {
        //        contractDetail.Rent = CalculateLastRent(request.EndDate, request.RentPrice);
        //    }
        //    contractDetail.DueDate = period.DueDate;
        //    contractDetails.Add(contractDetail);
        //}

        private decimal CalculateFirstRent(DateTime? beginDate, decimal rentPrice)
        {
            var daysNumber = 31 - beginDate.Value.Day;
            var rent = rentPrice / 30 * daysNumber;
            return rent;
        }

        private decimal CalculateLastRent(DateTime? endDate, decimal rentPrice)
        {
            var daysNumber = endDate.Value.Day;
            if (daysNumber >= 28)
                daysNumber = 30;
            var rent = rentPrice / 30 * daysNumber;
            return rent;
        }

        private async Task<int?> GetStatusbyEntityAndCodeAsync(string entityCode, string statusCode)
        {
            var entityStatus = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(entityCode, statusCode) ;
            if (entityStatus != null)
            return entityStatus.EntityStatusId.Value;

            return null;
        }

        private async Task<ConceptDTO> GetConceptByCode(string conceptCode)
        {
            var entity = await _conceptApplicationService.GetConceptByCodeAsync(conceptCode);
            return entity.Data;
        }

        private async Task<int?> GetConceptIdByCode(string conceptCode)
        {
            var entity = await GetConceptByCode(conceptCode);
            if (entity != null)
                return entity.ConceptId;
            return null;
        }

        private async Task<int?> GetGeneralTableIdByTableNameAndCode(string tableName, string tableCode)
        {
            var entity = await _generalTableApplicationService.GetGeneralTableByEntityAndCodeAsync(tableName, tableCode);
            if (entity != null)
                return entity.GeneralTableId;
            return null;
        }

        public async Task<ResponseDTO> UpdateContractAsync(ContractUpdateRequest contract)
        {
            //Map to Command
            
            var command = _mapper.Map<ContractUpdateRequest, ContractUpdateCommand>(contract);

            var response = await ValidateEntityUpdate(contract);

            if (response.IsValid)
            {
                //Execute Command
                var resp = await _bus.SendAsync(command);
                return ResponseBuilder.Correct(resp);
            }

            return response;
        }

        public async Task<ResponseDTO> DeleteContractAsync(ContractDeleteRequest contract)
        {
            //Map to Command
            var command = _mapper.Map<ContractDeleteRequest, ContractDeleteCommand>(contract);
            
            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<PagedList<ContractSearchDTO>>> SearchContractAsync(ContractSearchRequest search)
        {
            List<OrderExpression<ContractSearchDTO>> orderExpressionList = new List<OrderExpression<ContractSearchDTO>>();
            orderExpressionList.Add(new OrderExpression<ContractSearchDTO>(OrderType.Asc, p => p.ContractCode));

            Expression<Func<ContractSearchDTO, bool>> queryFilter = c => true;
            if (search.BeginDate.HasValue && search.EndDate.HasValue)
            {
                var toPlusADay = search.EndDate.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.CreationDate.Value >= search.BeginDate);
                queryFilter = queryFilter.And(p => p.CreationDate.Value < toPlusADay);
            }
            else if (search.BeginDate.HasValue && !search.EndDate.HasValue)
            {
                queryFilter = queryFilter.And(p => p.CreationDate.Value >= search.BeginDate);
            }
            else if (!search.BeginDate.HasValue && search.EndDate.HasValue)
            {
                var toPlusADay = search.EndDate.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.CreationDate.Value < toPlusADay);
            }

            if (!string.IsNullOrEmpty(search.ContractCode))
                queryFilter = queryFilter.And(p => p.ContractCode.Contains(search.ContractCode));

            if (!string.IsNullOrEmpty(search.TenantFullName))
                queryFilter = queryFilter.And(p => p.TenantFullName.Contains(search.TenantFullName));

            if (search.NextDaysToCollect>0 && search.NextDaysToCollect ==1)
            {
                queryFilter = queryFilter.And(p => p.NextDaysToCollect >=0 && p.NextDaysToCollect <= 5);
            }
            if (search.NextDaysToCollect > 0 && search.NextDaysToCollect == 2)
            {
                queryFilter = queryFilter.And(p => p.NextDaysToCollect >= 6 && p.NextDaysToCollect <= 10);
            }
            if (search.NextDaysToCollect > 0 && search.NextDaysToCollect == 3)
            {
                queryFilter = queryFilter.And(p => p.NextDaysToCollect >= 11 && p.NextDaysToCollect <= 15);
            }

            if (search.ContractStatusId.HasValue)
            {
                queryFilter = queryFilter.And(p => p.ContractStatusId == search.ContractStatusId);
            }

            if (search.HouseId.HasValue)
            {
                queryFilter = queryFilter.And(p => p.HouseId == search.HouseId);
            }

            if (search.UnpaidPeriods=="Y")
            {
                queryFilter = queryFilter.And(p => p.UnpaidPeriods> 0);
            }
            if (search.UnpaidPeriods == "N")
            {
                queryFilter = queryFilter.And(p => p.UnpaidPeriods == 0);
            }
            if (search.PeriodId.HasValue)
            {
                queryFilter = queryFilter.And(p => p.PeriodId == search.PeriodId);
            }

            var contract = await _contractSearchDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<ContractSearchDTO>()
            {
                Items = contract.Items,
                PageSize = contract.PageSize,
                Page = contract.Page,
                Total = contract.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<ContractRegisterRequest>> GetByIdAsync(int? id)
        {
            Expression<Func<ContractRegisterRequest, bool>> queryFilter = c => true;

            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.ContractId == id);

            var contract = await _contractDataAccess.FirstOrDefaultAsync(queryFilter);

            //TODO AGREGAR CODIGO PARA TRAER LOS OTHER TENANTS ASOCIADOS AL CONTRACT
            var otherTenants = await _otherTenantDataAccess.ListAsync(r => r.ContractId == id);

            if (contract != null)
                contract.OtherTenants = otherTenants.ToList();


            return ResponseBuilder.Correct(contract);
        }

        public async Task<ResponseDTO> ValidateEntityRegister(ContractRegisterRequest request)
        {
            bool isValid = true;
            Expression<Func<ContractRegisterRequest, bool>> queryFilter = p => true;
            var errorMessage = "";
            queryFilter = queryFilter.And(p => p.ContractCode == request.ContractCode);
            var contract = await _contractDataAccess.FirstOrDefaultAsync(queryFilter);

            if (contract != null)
            {
                isValid = false;
                errorMessage = "Contract already Exists";
            }

            //Existe un Lease para el mismo tenant Activo o Futuro
            if (isValid)
            {
                queryFilter = p => p.TenantId == request.TenantId;
                queryFilter = queryFilter.And(p => p.ContractStatusCode == Constants.EntityStatus.Contract.Formalized && p.RowStatus );
                contract = await _contractDataAccess.FirstOrDefaultAsync(queryFilter);

                if (contract != null)
                {
                    isValid = false;
                    errorMessage = "Already Exists a tenant associated to other Lease Formalized";
                }
            }

            //Validate Period
            if (isValid)
            {
                var period = await _periodApplicationService.GetLastPeriodAsync();

                if (period != null && period.Data.EndDate < request.EndDate)
                {
                    isValid = false;
                    errorMessage = "There is no period configurated to support the end date of this Lease, please create period";
                }
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

        public async Task<ResponseDTO> ValidateEntityUpdate(ContractUpdateRequest request)
        {
            var errorMessage = "";
            Expression<Func<ContractRegisterRequest, bool>> queryFilter = p => p.RowStatus;
            queryFilter = queryFilter.And(p => p.ContractId != request.ContractId);
            queryFilter = queryFilter.And(p => p.TenantId == request.TenantId);
            queryFilter = queryFilter.And(p => p.ContractStatusCode == Constants.EntityStatus.Contract.Draft || p.ContractStatusCode == Constants.EntityStatus.Contract.Formalized);

            bool isValid = true;
            var contract = await _contractDataAccess.FirstOrDefaultAsync(queryFilter);

            if (contract != null)
            {
                isValid = false;
                errorMessage = "Already Exists a tenant associated to other Lease Active or Future";
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
        
        public async Task<ResponseDTO<List<HouseFeatureDetailContractDTO>>> SearchHouseFeatureDetailContractAsync(int? houseId) {

            Expression<Func<HouseFeatureDetailContractDTO, bool>> queryFilter = c => c.HouseId == houseId;
            
            var contract = await _houseFeatureDetailContractDataAccess.ListAsync(queryFilter);

            return ResponseBuilder.Correct(contract.ToList());
        }


        /*EXCEL REPORT*/

        public async Task GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent,
            TransportContext transportContext, ContractSearchRequest search)
        {
            var list = await SearchContractAsync(search);
            try
            {
                if (list.Data.Items.Count > 0)
                    using (var writer = new StreamWriter(outputStream))
                    {
                        var headers = new List<string>
                        {
                            "Contract Code",
                            "Period",
                            "Tenant",
                            "Property",
                            "Start",
                            "Finish",
                            "Deposit",
                            "Rent",
                            "Unpaid periods",
                            "Due Date",
                            "Days to collect"
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

        private string ProcessCellDataToReport(ContractSearchDTO item)
        {
            var startDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.BeginDate) ?? "";
            var finishDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.EndDate) ?? "";
            //var product = !string.IsNullOrEmpty(item.ProductName) ? item.ProductName.Replace(@",", ".") : "";
            //var total = string.Format("{0:0.00}", item.DriverPay);
            var rentPrice = string.Format("{0:0.00}", item.RentPrice);
            var rentDeposit = string.Format("{0:0.00}", item.RentDeposit);
            var unpaidPeriods = string.Format("{0:0}", item.UnpaidPeriods);
            var nextDueDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.NextDueDate) ?? "";
            var nextDaystoCollect = string.Format("{0:0}", item.NextDaysToCollect);

            var textProperties = ExcelHelper.StringToCSVCell(item.ContractCode) + "," +
                                 ExcelHelper.StringToCSVCell(item.PeriodCode) + "," +
                                 ExcelHelper.StringToCSVCell(item.TenantFullName) + "," +
                                 ExcelHelper.StringToCSVCell(item.HouseName) + "," +
                                 ExcelHelper.StringToCSVCell(startDate) + "," +
                                 ExcelHelper.StringToCSVCell(finishDate) + "," +
                                 ExcelHelper.StringToCSVCell(rentDeposit) + "," +
                                 ExcelHelper.StringToCSVCell(rentPrice) + "," +
                                 ExcelHelper.StringToCSVCell(unpaidPeriods) + "," +
                                 ExcelHelper.StringToCSVCell(nextDueDate) + "," +
                                 ExcelHelper.StringToCSVCell(nextDaystoCollect) ;

            return textProperties;
        }

    }
}
