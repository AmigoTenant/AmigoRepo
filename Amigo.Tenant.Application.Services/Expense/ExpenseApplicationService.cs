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

namespace Amigo.Tenant.Application.Services.Expense
{
    public class ExpenseApplicationService : IExpenseApplicationService
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IQueryDataAccess<ExpenseSearchDTO> _expenseSearchDataAccess;
        private readonly IQueryDataAccess<ExpenseDetailSearchDTO> _expenseDetailSearchDataAccess;
        private readonly IQueryDataAccess<ExpenseRegisterRequest> _expenseDataAccess;
        private readonly IQueryDataAccess<ExpenseDetailRegisterRequest> _expenseDetailDataAccess;
        private readonly IEntityStatusApplicationService _entityStatusApplicationService;
        private readonly IPeriodApplicationService _periodApplicationService;

        public ExpenseApplicationService(IBus bus,
            IQueryDataAccess<ExpenseSearchDTO> expenseSearchDataAccess,
            IQueryDataAccess<ExpenseRegisterRequest> expenseDataAccess,
            IQueryDataAccess<ExpenseDetailRegisterRequest> expenseDetailDataAccess,
            IQueryDataAccess<ExpenseDetailSearchDTO> expenseDetailSearchDataAccess,
            IEntityStatusApplicationService entityStatusApplicationService,
            IPeriodApplicationService periodApplicationService,
            IMapper mapper
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

        public async Task<ResponseDTO<ExpenseRegisterRequest>> GetByIdAsync(int? id)
        {
            Expression<Func<ExpenseRegisterRequest, bool>> queryFilter = c => true;

            if (id.HasValue)
                queryFilter = queryFilter.And(p => p.ExpenseId == id);

            var expense = await _expenseDataAccess.FirstOrDefaultAsync(queryFilter);

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
            var command = _mapper.Map<ExpenseDetailRegisterRequest, ExpenseDetailRegisterCommand>(request);
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

        public async Task<ResponseDTO<PagedList<ExpenseDetailSearchDTO>>> GetDetailByExpenseIdAsync(ExpenseSearchRequest search)
        {
            List<OrderExpression<ExpenseDetailSearchDTO>> orderExpressionList = new List<OrderExpression<ExpenseDetailSearchDTO>>();
            orderExpressionList.Add(new OrderExpression<ExpenseDetailSearchDTO>(OrderType.Asc, p => p.ConceptName));
            Expression<Func<ExpenseDetailSearchDTO, bool>> queryFilter = c => true;
            
            if (search.ExpenseId.HasValue)
                queryFilter = queryFilter.And(p => p.ExpenseId == search.ExpenseId);


            var expense = await _expenseDetailSearchDataAccess.ListPagedAsync
                (queryFilter, search.Page, search.PageSize, orderExpressionList.ToArray());

            var pagedResult = new PagedList<ExpenseDetailSearchDTO>()
            {
                Items = expense.Items,
                PageSize = expense.PageSize,
                Page = expense.Page,
                Total = expense.Total
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


        public Task<ResponseDTO> ChangeDetailStatus(ExpenseDetailChangeStatusRequest expense)
        {
            throw new NotImplementedException();
        }

    }

}
