using Amigo.Tenant.Application.DTOs.Requests.Expense;
using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Expense;
using Amigo.Tenant.Application.Services.Interfaces.Expense;
using Amigo.Tenant.Application.Services.Interfaces.MasterData;
using Amigo.Tenant.Commands.Expense;
using Amigo.Tenant.Commands.ExpenseDetail;
using Amigo.Tenant.Common;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Amigo.Tenant.Application.DTOs.Requests.PaymentPeriod;
using Amigo.Tenant.Application.Services.Interfaces.Tracking;
using Amigo.Tenant.Application.DTOs.Requests.Leasing;
using Amigo.Tenant.Application.DTOs.Responses.Leasing;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;

namespace Amigo.Tenant.Application.Services.Expense
{
    public class ExpenseApplicationService : IExpenseApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ExpenseSearchDTO> _expenseSearchDataAccess;
        private readonly IQueryDataAccess<ExpenseDetailSearchDTO> _expenseDetailSearchDataAccess;
        private readonly IQueryDataAccess<ExpenseRegisterRequest> _expenseDataAccess;
        private readonly IQueryDataAccess<ExpenseDTO> _expenseDtoDataAccess;
        private readonly IQueryDataAccess<ExpenseDetailRegisterRequest> _expenseDetailDataAccess;
        private readonly IEntityStatusApplicationService _entityStatusApplicationService;
        private readonly IGeneralTableApplicationService _generalTableApplicationService;
        private readonly IPeriodApplicationService _periodApplicationService;
        private readonly IQueryDataAccess<ContractDTO> _contractDtoDataAccess;

        public ExpenseApplicationService(IBus bus,
            IQueryDataAccess<ExpenseSearchDTO> expenseSearchDataAccess,
            IQueryDataAccess<ExpenseRegisterRequest> expenseDataAccess,
            IQueryDataAccess<ExpenseDetailRegisterRequest> expenseDetailDataAccess,
            IQueryDataAccess<ExpenseDetailSearchDTO> expenseDetailSearchDataAccess,
            IEntityStatusApplicationService entityStatusApplicationService,
            IPeriodApplicationService periodApplicationService,
            IMapper mapper,
            IQueryDataAccess<ExpenseDTO> expenseDtoDataAccess,
            IGeneralTableApplicationService generalTableApplicationService,
            IQueryDataAccess<ContractDTO> contractDtoDataAccess
            )
        {
            if (bus == null) throw new ArgumentNullException(nameof(bus));
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            _bus = bus;
            _expenseSearchDataAccess = expenseSearchDataAccess;
            _expenseDataAccess = expenseDataAccess;
            _mapper = mapper;
            _entityStatusApplicationService = entityStatusApplicationService;
            _periodApplicationService = periodApplicationService;
            _expenseDetailSearchDataAccess = expenseDetailSearchDataAccess;
            _expenseDetailDataAccess = expenseDetailDataAccess;
            _expenseDtoDataAccess = expenseDtoDataAccess;
            _generalTableApplicationService = generalTableApplicationService;
            _contractDtoDataAccess  = contractDtoDataAccess;
        }

        public async Task<ResponseDTO> RegisterExpenseAsync(ExpenseRegisterRequest request)
        {
            var command = _mapper.Map<ExpenseRegisterRequest, ExpenseRegisterCommand>(request);
            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp, command.ExpenseId, "");
        }

        private async Task<int?> GetStatusByCodeAsync(string entityCode, string statusCode)
        {
            var entityStatus = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(entityCode, statusCode);
            if (entityStatus != null)
                return entityStatus.EntityStatusId.Value;
            return null;
        }

        public async Task<ResponseDTO> UpdateExpenseAsync(ExpenseUpdateRequest expense)
        {
            //Map to Command
            var command = _mapper.Map<ExpenseUpdateRequest, ExpenseUpdateCommand>(expense);
            if (true)
            {
                var resp = await _bus.SendAsync(command);
                return ResponseBuilder.Correct(resp);
            }
            return null;
        }

        public async Task<ResponseDTO> DeleteExpenseAsync(ExpenseDeleteRequest expense)
        {
            //Map to Command
            var command = _mapper.Map<ExpenseDeleteRequest, ExpenseDeleteCommand>(expense);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<PagedList<ExpenseSearchDTO>>> SearchExpenseAsync(ExpenseSearchRequest search)
        {
            List<OrderExpression<ExpenseSearchDTO>> orderExpressionList = new List<OrderExpression<ExpenseSearchDTO>>();
            orderExpressionList.Add(new OrderExpression<ExpenseSearchDTO>(OrderType.Asc, p => p.ExpenseDate));
            Expression<Func<ExpenseSearchDTO, bool>> queryFilter = c => true;

            //APPLICATIONDATE
            if (search.ExpenseDateFrom.HasValue && search.ExpenseDateTo.HasValue)
            {
                var toPlusADay = search.ExpenseDateTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ExpenseDate.Value >= search.ExpenseDateFrom);
                queryFilter = queryFilter.And(p => p.ExpenseDate.Value < toPlusADay);
            }
            else if (search.ExpenseDateFrom.HasValue && !search.ExpenseDateTo.HasValue)
            {
                queryFilter = queryFilter.And(p => p.ExpenseDate.Value >= search.ExpenseDateFrom);
            }
            else if (!search.ExpenseDateFrom.HasValue && search.ExpenseDateTo.HasValue)
            {
                var toPlusADay = search.ExpenseDateTo.Value.AddDays(1);
                queryFilter = queryFilter.And(p => p.ExpenseDate.Value < toPlusADay);
            }

            if (search.HouseTypeId.HasValue)
                queryFilter = queryFilter.And(p => p.HouseTypeId == search.HouseTypeId);

            if (search.PeriodId.HasValue)
                queryFilter = queryFilter.And(p => p.PeriodId == search.PeriodId);

            if (search.HouseId.HasValue)
                queryFilter = queryFilter.And(p => p.HouseId == search.HouseId);

            if (search.PaymentTypeId.HasValue)
                queryFilter = queryFilter.And(p => p.PaymentTypeId == search.PaymentTypeId);

            if (!string.IsNullOrEmpty(search.ReferenceNo))
                queryFilter = queryFilter.And(p => p.ReferenceNo == search.ReferenceNo);

            if (search.ExpenseDetailStatusId.HasValue)
                queryFilter = queryFilter.And(p => p.ExpenseDetailStatusId == search.ExpenseDetailStatusId);

            if (search.ConceptId.HasValue)
                queryFilter = queryFilter.And(p => p.ConceptId == search.ConceptId);

            if (!string.IsNullOrEmpty(search.Remark))
                queryFilter = queryFilter.And(p => p.Remark == search.Remark);

            var expense = await _expenseSearchDataAccess.ListPagedAsync(queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<ExpenseSearchDTO>()
            {
                Items = expense.Items,
                PageSize = expense.PageSize,
                Page = expense.Page,
                Total = expense.Total
            };

            return ResponseBuilder.Correct(pagedResult);
        }

        public async Task<ResponseDTO<ExpenseDTO>> GetByIdAsync(int? id)
        {
            Expression<Func<ExpenseDTO, bool>> queryFilter = c => true;

            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.ExpenseId == id);

            var expense = await _expenseDtoDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(expense);
        }

        /*EXCEL REPORT*/

        public async Task GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent,
            TransportContext transportContext, ExpenseSearchRequest search)
        {

            //var list = await SearchExpenseAsync(search);
            //try
            //{
            //    //if (list.Data.Items.Count > 0)
            //    //    using (var writer = new StreamWriter(outputStream))
            //    //    {
            //    //        var headers = new List<string>
            //    //            {
            //    //                "Period",
            //    //                "Expense Date",
            //    //                "Property Type",
            //    //                "Property",
            //    //                "Tenant",
            //    //                "Payment Type",
            //    //                "Reference No",
            //    //                "Status",
            //    //                "Concept",
            //    //                "Start"
            //    //            };
            //    //        await writer.WriteLineAsync(ExcelHelper.GetHeaderDetail(headers));
            //    //        foreach (var item in list.Data.Items)
            //    //            await writer.WriteLineAsync(ProcessCellDataToReport(item));
            //    //    }
            //}
            //catch (HttpException ex)
            //{
            //}
            //finally
            //{
            //    outputStream.Close();
            //}
            throw new NotImplementedException();
        }

        private string ProcessCellDataToReport(ExpenseSearchDTO item)
        {
            var expenseDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.ExpenseDate) ?? "";
            //var finishDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.EndDate) ?? "";
            //var product = !string.IsNullOrEmpty(item.ProductName) ? item.ProductName.Replace(@",", ".") : "";
            //var total = string.Format("{0:0.00}", item.DriverPay);
            //var rentPrice = string.Format("{0:0.00}", item.RentPrice);
            //var rentDeposit = string.Format("{0:0.00}", item.RentDeposit);
            //var unpaidPeriods = string.Format("{0:0}", item.UnpaidPeriods);
            //var nextDueDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.NextDueDate) ?? "";
            //var nextDaystoCollect = string.Format("{0:0}", item.NextDaysToCollect);

            var textProperties = ExcelHelper.StringToCSVCell(expenseDate) + "," + "";
                                     //ExcelHelper.StringToCSVCell(item.PeriodCode) + "," +
                                     ////ExcelHelper.StringToCSVCell(item.TenantFullName) + "," +
                                     ////ExcelHelper.StringToCSVCell(item.HouseName) + "," +
                                     //ExcelHelper.StringToCSVCell(startDate) + "," +
                                     //ExcelHelper.StringToCSVCell(finishDate) + "," +
                                     //ExcelHelper.StringToCSVCell(rentDeposit) + "," +
                                     //ExcelHelper.StringToCSVCell(rentPrice) + "," +
                                     //ExcelHelper.StringToCSVCell(unpaidPeriods) + "," +
                                     //ExcelHelper.StringToCSVCell(nextDueDate) + "," +
                                     //ExcelHelper.StringToCSVCell(nextDaystoCollect);

            return textProperties;
        }

        private async Task<int?> GetPeriodByCode(string periodCode)
        {
            var period = await _periodApplicationService.GetPeriodByCodeAsync(periodCode);
            if (period.Data != null)
                return period.Data.PeriodId;

            return null;
        }

        public Task<ResponseDTO> ChangeStatus(ExpenseChangeStatusRequest expense)
        {
            throw new NotImplementedException();
        }


        
        
        
        /* DETAIL */

        public async Task<ResponseDTO> RegisterExpenseDetailAsync(ExpenseDetailRegisterRequest request)
        {
            var response = new ResponseDTO();
            var command = _mapper.Map<ExpenseDetailRegisterRequest, ExpenseDetailRegisterCommand>(request);
            var applyTo = await _generalTableApplicationService.GetGeneralTableByEntityAndCodeAsync(Constants.GeneralTableName.ApplyTo, Constants.GeneralTableCode.ApplyTo.AllTenants);
            var expense = await _expenseDtoDataAccess.FirstOrDefaultAsync(q => q.ExpenseId.Value == request.ExpenseId);
            if (request.ApplyTo.HasValue && request.ApplyTo.Value == applyTo.GeneralTableId && (expense ==null || !expense.HouseId.HasValue))
            {
                response.IsValid = false;
                response.Messages = new List<ApplicationMessage>
                {
                    new ApplicationMessage()
                    {
                        Key = "Error",
                        Message = "Imposible grabar los detalles para todos los tenants si no especifica la propiedad a la que se hara el ingreso"
                    }
                };

                return response;
            }

            command.PeriodId = expense.PeriodId.Value;
            command.HouseId = expense.HouseId.Value;

            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp, command.ExpenseId, "");
        }
        
        public async Task<ResponseDTO> UpdateExpenseDetailAsync(ExpenseDetailUpdateRequest expenseDetail)
        {
            //Map to Command
            var command = _mapper.Map<ExpenseDetailUpdateRequest, ExpenseDetailUpdateCommand>(expenseDetail);
            if (true)
            {
                var resp = await _bus.SendAsync(command);
                return ResponseBuilder.Correct(resp);
            }
            return null;
        }

        public async Task<ResponseDTO> DeleteExpenseDetailAsync(ExpenseDetailDeleteRequest expenseDetail)
        {
            //Map to Command
            var command = _mapper.Map<ExpenseDetailDeleteRequest, ExpenseDetailDeleteCommand>(expenseDetail);

            //Execute Command
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp);
        }

        public async Task<ResponseDTO<PagedList<ExpenseDetailSearchDTO>>> GetDetailByExpenseIdAsync(int? id)
        {
            List<OrderExpression<ExpenseDetailSearchDTO>> orderExpressionList = new List<OrderExpression<ExpenseDetailSearchDTO>>();
            orderExpressionList.Add(new OrderExpression<ExpenseDetailSearchDTO>(OrderType.Asc, p => p.ConceptName));
            Expression<Func<ExpenseDetailSearchDTO, bool>> queryFilter = c => true;
            
            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.ExpenseId == id);

            var expense = (await _expenseDetailSearchDataAccess.ListAsync(queryFilter, orderExpressionList.ToArray())).ToList();

            var pagedResult = new PagedList<ExpenseDetailSearchDTO>()
            {
                Items = expense,
                PageSize = 500,
                Page = 0,
                Total = expense.Count
            };

            return ResponseBuilder.Correct(pagedResult);
        }
        public async Task<ResponseDTO<ExpenseDetailRegisterRequest>> GetDetailByExpenseDetailIdAsync(int? id)
        {
            Expression<Func<ExpenseDetailRegisterRequest, bool>> queryFilter = c => true;

            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.ExpenseId == id);

            var expense = await _expenseDetailDataAccess.FirstOrDefaultAsync(queryFilter);

            return ResponseBuilder.Correct(expense);
        }


        public async Task GenerateDataCsvToReportExcelDetail(Stream outputStream, HttpContent httpContent,
            TransportContext transportContext, ExpenseSearchRequest search)
        {

            //var list = await SearchExpenseAsync(search);
            //try
            //{
            //    //if (list.Data.Items.Count > 0)
            //    //    using (var writer = new StreamWriter(outputStream))
            //    //    {
            //    //        var headers = new List<string>
            //    //            {
            //    //                "Period",
            //    //                "Expense Date",
            //    //                "Property Type",
            //    //                "Property",
            //    //                "Tenant",
            //    //                "Payment Type",
            //    //                "Reference No",
            //    //                "Status",
            //    //                "Concept",
            //    //                "Start"
            //    //            };
            //    //        await writer.WriteLineAsync(ExcelHelper.GetHeaderDetail(headers));
            //    //        foreach (var item in list.Data.Items)
            //    //            await writer.WriteLineAsync(ProcessCellDataToReport(item));
            //    //    }
            //}
            //catch (HttpException ex)
            //{
            //}
            //finally
            //{
            //    outputStream.Close();
            //}
            throw new NotImplementedException();
        }

        private string ProcessCellDataToReportDetail(ExpenseDetailSearchDTO item)
        {
            //var expenseDate = string.Format("{0:MM/dd/yyyy HH:mm}", item.ExpenseDate) ?? "";
            var totalAmount = item.TotalAmount;
            var textProperties = ExcelHelper.StringToCSVCell(totalAmount.ToString()) + "," + "";

            return textProperties;
        }


        public async Task<ResponseDTO> ChangeDetailStatusAsync(ExpenseDetailChangeStatusRequest expenseDetailList)
        {
            var expenseRequest = await GetByIdAsync(expenseDetailList.ExpenseId);

            await CreateContractDetail(expenseDetailList, expenseRequest.Data);

            var command = _mapper.Map<ContractRegisterRequest, ContractChangeStatusCommand>(expenseRequest.Data);

            command.ContractStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.Contract, Constants.EntityStatus.Contract.Formalized);
            command.UserId = request.UserId.Value;
            var resp = await _bus.SendAsync(command);

            return ResponseBuilder.Correct(resp, command.ContractId, command.ContractCode);

        }


        private async Task CreateContractDetail(ExpenseDetailChangeStatusRequest expenseDetailList, ExpenseDTO request)
        {
            var isLastPeriod = false;
            var period = (await _periodApplicationService.GetPeriodByIdAsync(request.PeriodId)).Data;
            //var contractEndDate = request.EndDate;
            var currentPeriodDueDate = period.DueDate.Value.AddMonths(1);
            var id = -1;
            var contractDetails = new List<ContractDetailRegisterRequest>();

            var paymentsPeriod = new List<PaymentPeriodRegisterRequest>();
            //var rentConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Rent);
            //var depositConceptId = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Deposit);
            //var entityStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);
            //var paymentTypeRentId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Rent);
            //var paymentTypeDepositId = await GetGeneralTableIdByTableNameAndCode(Constants.GeneralTableName.PaymentType, Constants.GeneralTableCode.PaymentType.Deposit);

            var detailList = await _expenseDetailDataAccess.ListAsync(q => expenseDetailList.ChangeStatusList.Select(r => r.ExpenseDetailId).Contains(q.ExpenseDetailId.Value));

            foreach (var item in expenseDetailList.ChangeStatusList)
            {
                

                var expenseDetailRegisterRequest = detailList.FirstOrDefault(q => q.ExpenseDetailId.Value == item.ExpenseDetailId.Value);

                if (expenseDetailRegisterRequest != null)
                {
                    var contract = await _contractDtoDataAccess.FirstOrDefaultAsync(q => q.TenantId == expenseDetailRegisterRequest.TenantId 
                                        && q.PeriodId == request.PeriodId.Value.ToString()
                                        && q.ContractStatusId == 10); //TODO: Corregir esto: tOsTRING Y cONTRACTiD

                    SetPaymentsPeriod(paymentsPeriod, request, period, id, isLastPeriod, rentConceptId, entityStatusId, paymentTypeRentId, depositConceptId, paymentTypeDepositId, expenseDetailRegisterRequest);

                }
                
            }


        }

        private void SetPaymentsPeriod(List<PaymentPeriodRegisterRequest> paymentsPeriod, ContractRegisterRequest request, PeriodDTO period, int id, bool isLastPeriod, int? rentId, int? entityStatusId, int? paymentTypeId, int? depositConceptId, int? paymentTypeDepositId, ExpenseDetailRegisterRequest expenseDetailRegisterRequest)
        {
            ///////////////////
            //SETTING FOR  DEPOSIT
            ///////////////////

            //if (Math.Abs(id) == 1)
            //{
            //    var paymentPeriodDeposit = new PaymentPeriodRegisterRequest();
            //    paymentPeriodDeposit.PaymentPeriodId = id;
            //    paymentPeriodDeposit.ConceptId = depositConceptId; //"CODE FOR CONCEPT"; //TODO:
            //    paymentPeriodDeposit.ContractId = request.ContractId;
            //    paymentPeriodDeposit.TenantId = request.TenantId;
            //    paymentPeriodDeposit.PeriodId = period.PeriodId;
            //    paymentPeriodDeposit.PaymentPeriodStatusId = entityStatusId; //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
            //    paymentPeriodDeposit.RowStatus = true;
            //    paymentPeriodDeposit.CreatedBy = request.UserId;
            //    paymentPeriodDeposit.CreationDate = DateTime.Now;
            //    paymentPeriodDeposit.UpdatedBy = request.UserId;
            //    paymentPeriodDeposit.UpdatedDate = DateTime.Now;
            //    paymentPeriodDeposit.PaymentTypeId = paymentTypeDepositId;
            //    paymentPeriodDeposit.PaymentAmount = request.RentDeposit;
            //    paymentPeriodDeposit.DueDate = period.DueDate;
            //    paymentsPeriod.Add(paymentPeriodDeposit);
            //}

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
            paymentPeriodRent.PaymentAmount = expenseDetailRegisterRequest.TotalAmount;
            paymentPeriodRent.DueDate = period.DueDate;
            paymentsPeriod.Add(paymentPeriodRent);


        }



    }

}
