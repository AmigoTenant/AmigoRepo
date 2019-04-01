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
using Amigo.Tenant.Commands.PaymentPeriod;
using model = Amigo.Tenant.CommandModel.Models;

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
        //private readonly IQueryDataAccess<ContractDTO> _contractDtoDataAccess;
        private readonly IConceptApplicationService _conceptApplicationService;
        private readonly IRepository<model.ExpenseDetail> _repositoryExpenseDetail;
        private readonly IRepository<model.PaymentPeriod> _repositoryPaymentPeriod;

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
            //IQueryDataAccess<ContractDTO> contractDtoDataAccess,
            IConceptApplicationService conceptApplicationService,
            IRepository<model.ExpenseDetail> repositoryExpenseDetail,
            IRepository<model.PaymentPeriod> repositoryPaymentPeriod
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
            //_contractDtoDataAccess  = contractDtoDataAccess;
            _conceptApplicationService = conceptApplicationService;
            _repositoryExpenseDetail = repositoryExpenseDetail;
            _repositoryPaymentPeriod = repositoryPaymentPeriod;
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
            orderExpressionList.Add(new OrderExpression<ExpenseSearchDTO>(OrderType.Desc, p => p.PeriodCode));
            orderExpressionList.Add(new OrderExpression<ExpenseSearchDTO>(OrderType.Asc, p => p.PaymentTypeName));
            orderExpressionList.Add(new OrderExpression<ExpenseSearchDTO>(OrderType.Asc, p => p.ExpenseDate));
            orderExpressionList.Add(new OrderExpression<ExpenseSearchDTO>(OrderType.Asc, p => p.TenantFullName));
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


        public async Task<ResponseDTO> ChangeDetailStatusAsync(ExpenseDetailChangeStatusRequest expenseDetailChangeStatusRequest)
        {
            var expense = await GetByIdAsync(expenseDetailChangeStatusRequest.ExpenseId);
            //await CreatePaymentInformation(expenseDetailChangeStatusRequest, expense.Data);
            var command = await CreatePaymentInformation(expenseDetailChangeStatusRequest, expense.Data);
            command.UserId = expenseDetailChangeStatusRequest.UserId.Value;
            var resp = await _bus.SendAsync(command);
            return ResponseBuilder.Correct(resp, command.ExpenseId, "NONE");
        }


        private async Task<ExpenseDetailChangeStatusCommand> CreatePaymentInformation(ExpenseDetailChangeStatusRequest expenseDetailList, ExpenseDTO expenseDto)
        {
            var period = (await _periodApplicationService.GetPeriodByIdAsync(expenseDetailList.PeriodId)).Data;

            var expenseDetailStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.Expense, Constants.EntityStatus.Expense.Pending);

            var expenseDetailChangeStatusCommand = new ExpenseDetailChangeStatusCommand() {
                ExpenseId = expenseDto.ExpenseId,
                ExpenseDetailStatusId = expenseDetailStatusId
            };

            var expenseDetailUpdateCommandList = new List<ExpenseDetailUpdateCommand>();
            var paymentStatusId = await GetStatusbyEntityAndCodeAsync(Constants.EntityCode.PaymentPeriod, Constants.EntityStatus.PaymentPeriod.Pending);

            //var detailList = await _expenseDetailDataAccess.ListAsync(q => expenseDetailList.ExpenseDetailListId.Contains(q.ExpenseDetailId.Value));

            string[] includes = new string[] { "Concept" };
            var detailList = await _repositoryExpenseDetail.ListAsync(q => expenseDetailList.ExpenseDetailListId.Contains(q.ExpenseDetailId.Value), includes: includes);
            var id = -1;

            foreach (var item in expenseDetailList.ExpenseDetailListId)
            {

                var expenseDetailRegisterRequest = detailList.FirstOrDefault(q => q.ExpenseDetailId.Value == item);

                if (expenseDetailRegisterRequest != null)
                {
                    //var concept = await GetConceptIdByCode(Constants.GeneralTableCode.ConceptType.Rent);

                    //TODO: SE ESTA CAYENDO EN ESTA CONSULTA

                    var rentaPayment = await _repositoryPaymentPeriod.FirstOrDefaultAsync(q => q.TenantId == expenseDetailRegisterRequest.TenantId
                                        && q.PeriodId == expenseDetailList.PeriodId
                                        && q.ConceptId == 15  //Cambiar 15 por el codigo del concepto RENTA
                                        ); 

                    var expenseDetailUpdateCommand = new ExpenseDetailUpdateCommand()
                    {
                        ExpenseDetailId = item,
                        ExpenseDetailStatusId = expenseDetailStatusId
                    };

                    var paymentPeriodRegisterCommand = new PaymentPeriodRegisterCommand()
                    {
                        PaymentPeriodId = id,
                        ConceptId = expenseDetailRegisterRequest.ConceptId, //"CODE FOR CONCEPT"; //TODO:
                        ContractId = rentaPayment.ContractId,
                        TenantId = expenseDetailRegisterRequest.TenantId,
                        PeriodId = period.PeriodId,
                        PaymentPeriodStatusId = paymentStatusId, //TODO: PONER EL CODIGO CORRECTO PARA EL CONTRACTDETAILSTATUS
                        RowStatus = true,
                        CreatedBy = expenseDetailList.UserId,
                        CreationDate = DateTime.Now,
                        UpdatedBy = expenseDetailList.UserId,
                        UpdatedDate = DateTime.Now,
                        PaymentTypeId = expenseDetailRegisterRequest.Concept.PayTypeId, //revisar si no traer del concepto
                        PaymentAmount = expenseDetailRegisterRequest.TotalAmount,
                        DueDate = period.DueDate,
                    };

                    expenseDetailUpdateCommand.PaymentPeriodRegister = paymentPeriodRegisterCommand;
                    expenseDetailUpdateCommandList.Add(expenseDetailUpdateCommand);
                    id--;
                }
            }
            expenseDetailChangeStatusCommand.ExpenseDetailUpdateCommand = expenseDetailUpdateCommandList;
            return expenseDetailChangeStatusCommand;
        }

        private async Task<int?> GetStatusbyEntityAndCodeAsync(string entityCode, string statusCode)
        {
            var entityStatus = await _entityStatusApplicationService.GetEntityStatusByEntityAndCodeAsync(entityCode, statusCode);
            if (entityStatus != null)
                return entityStatus.EntityStatusId.Value;

            return null;
        }

        private async Task<int?> GetConceptIdByCode(string conceptCode)
        {
            var entity = await GetConceptByCode(conceptCode);
            if (entity != null)
                return entity.ConceptId;
            return null;
        }

        private async Task<ConceptDTO> GetConceptByCode(string conceptCode)
        {
            var entity = await _conceptApplicationService.GetConceptByCodeAsync(conceptCode);
            return entity.Data;
        }

    }

}
