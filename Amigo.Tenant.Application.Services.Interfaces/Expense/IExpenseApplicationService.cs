using Amigo.Tenant.Application.DTOs.Requests.Expense;
using Amigo.Tenant.Application.DTOs.Requests.MasterData;
using Amigo.Tenant.Application.DTOs.Responses.Common;
using Amigo.Tenant.Application.DTOs.Responses.Expense;
using Amigo.Tenant.Application.DTOs.Responses.MasterData;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Amigo.Tenant.Application.Services.Interfaces.Expense
{
    public interface IExpenseApplicationService
    {
        Task<ResponseDTO> RegisterExpenseAsync(ExpenseRegisterRequest request);
        Task<ResponseDTO> UpdateExpenseAsync(ExpenseUpdateRequest expense);
        Task<ResponseDTO> DeleteExpenseAsync(ExpenseDeleteRequest expense);
        Task<ResponseDTO<PagedList<ExpenseSearchDTO>>> SearchExpenseAsync(ExpenseSearchRequest search);
        Task<ResponseDTO<ExpenseRegisterRequest>> GetByIdAsync(int? id);
        Task GenerateDataCsvToReportExcel(Stream outputStream, HttpContent httpContent,
            TransportContext transportContext, ExpenseSearchRequest search);
        Task<ResponseDTO> ChangeStatus(ExpenseChangeStatusRequest expense);

        /*DETAIL */
        Task<ResponseDTO> RegisterExpenseDetailAsync(ExpenseDetailRegisterRequest request);
        Task<ResponseDTO> UpdateExpenseDetailAsync(ExpenseDetailUpdateRequest expenseDetail);
        Task<ResponseDTO> DeleteExpenseDetailAsync(ExpenseDetailDeleteRequest expenseDetail);
        Task<ResponseDTO<PagedList<ExpenseDetailSearchDTO>>> GetDetailByExpenseIdAsync(ExpenseSearchRequest search);
        Task<ResponseDTO<ExpenseDetailRegisterRequest>> GetDetailByExpenseDetailIdAsync(int? id);
        Task GenerateDataCsvToReportExcelDetail(Stream outputStream, HttpContent httpContent, TransportContext transportContext, ExpenseSearchRequest search);
        Task<ResponseDTO> ChangeDetailStatus(ExpenseDetailChangeStatusRequest expense);
    }
}
